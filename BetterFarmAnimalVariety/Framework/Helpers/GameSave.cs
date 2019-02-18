using Netcode;
using StardewValley;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using BetterFarmAnimalVariety.Framework.SaveData;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    internal class GameSave
    {
        public static void OverwriteFarmAnimal(ref FarmAnimal animal, string requestedType)
        {
            // ==========
            // WARNING:
            // Don't sanitize a farm animal's type by blue/void/brown cow chance
            // etc. or BFAV config existence here. These checks should be done 
            // in the menus, etc.
            // ==========

            if (animal.Name == null)
            {
                // TODO: Debug why this happens on reload on game save
                return;
            }

            if (Api.FarmAnimal.IsVanilla(requestedType))
            {
                // We don't need to do anything for vanilla animals
                // Saving to save data happens on saves
                return;
            }

            // Check the save entry for reloaded animals that may have their 
            // vanilla replacements saved which can't be used
            TypeLog typeHistory = FarmAnimalsSaveData.Deserialize().GetTypeHistory(animal.myID.Value);

            // If there's a save data entry, use that; otherwise this might be 
            // an animal created before being saved (ie. created in current day)
            string currentType = typeHistory == null ? (requestedType ?? animal.type.Value) : typeHistory.CurrentType;

            // Grab the new type's data to override if it exists
            Dictionary<string, string> contentData = Api.Content.LoadData<string, string>(Constants.Content.DataFarmAnimalsContentPath);
            KeyValuePair<string, string> contentDataEntry = Api.Content.GetDataEntry<string, string>(contentData, currentType);

            // Always validate if the type we're trying to use exists
            if (contentDataEntry.Key == null)
            {
                // Get a default type to use
                string defaultType = Api.FarmAnimal.GetDefaultType(animal);

                // Set it to the default before we continue
                contentDataEntry = contentData.FirstOrDefault(kvp => kvp.Key.Equals(defaultType));

                // Do a final check to make sure the default exists; otherwise 
                // we need to kill everything. This should never happen unless 
                // agressive mods are being used to REMOVE vanilla animals.
                if (contentDataEntry.Key == null)
                {
                    throw new KeyNotFoundException($"Could not find {defaultType} to overwrite custom farm animal for saving. This is a fatal error. Please make sure you have {defaultType} in the game.");
                }
            }

            // Set the animal with the new type's data values
            Api.FarmAnimal.UpdateFromData(ref animal, contentDataEntry);
        }

        public static void CleanFarmAnimals(string saveFolder, out Dictionary<long, TypeLog> typesToBeMigrated)
        {
            // Track the types to be migrated for reporting
            typesToBeMigrated = new Dictionary<long, TypeLog>();

            string saveFile = Path.Combine(saveFolder, Path.GetFileName(saveFolder));

            if (!File.Exists(saveFile))
            {
                throw new FileNotFoundException($"{saveFile} does not exist");
            }

            FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

            // Replace barn animals with White Cows and coop animals with White Chickens
            XmlDocument doc = new XmlDocument();

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Load the XML
            doc.Load(saveFile);

            XmlNodeList buildings = doc.SelectNodes("//GameLocation[@xsi:type='Farm']/buildings/Building[@xsi:type='Barn' or @xsi:type='Coop']", namespaceManager);

            // Go through each building
            for (int i = 0; i < buildings.Count; i++)
            {
                // Required to determine which default dweller should be used
                string buildingType = buildings[i].Attributes["xsi:type"].Value;

                // Grab the animals in this building
                XmlNodeList animals = buildings[i].SelectNodes(".//FarmAnimal");

                // Go through each animal
                for (int k = 0; k < animals.Count; k++)
                {
                    // This is the type that's saved... We're going to continue to
                    // associate the dweller's type as saved and this as the current
                    string currentType = animals[k].SelectSingleNode("type").InnerText;
                    long myId = long.Parse(animals[k].SelectSingleNode("myID").InnerText);

                    // We only need to update the animals if they aren't vanilla
                    if (Api.FarmAnimal.IsVanilla(currentType))
                    {
                        // But only if that animal hasn't been logged before; 
                        // otherwise it would endlessly overwrite the animals. 
                        // Non -vanilla migrations get saved later.
                        if (!saveData.GetTypeHistory().ContainsKey(myId))
                        {
                            saveData.AddTypeHistory(myId, currentType, currentType);
                        }

                        // Always skip the migration for vanilla
                        continue;
                    }

                    // Choose the default dweller based on the building
                    string typeToBeSaved = Api.FarmAnimal.GetDefaultType(buildingType);

                    // Clean the node by replace the dirty saved values from the 
                    // content of the default dwellers
                    Helpers.GameSave.CleanDirtyFarmAnimalXmlNode(ref doc, ref animals, k, typeToBeSaved, myId);

                    // Track the migration of this type to save it in the save data
                    typesToBeMigrated.Add(myId, new TypeLog(currentType, typeToBeSaved));
                }
            }

            // Save the migrations
            if (typesToBeMigrated.Any())
            {
                // Save the XmlDocument back to disk
                doc.Save(saveFile);

                // Update the save data
                saveData.AddTypeHistory(typesToBeMigrated);
            }
        }

        private static void CleanDirtyFarmAnimalXmlNode(ref XmlDocument doc, ref XmlNodeList animals, int index, string defaultType, long myId)
        {
            XmlNode animal = animals[index];

            // We will need this to pass to the dweller - myId is the 
            // only one that matters that it's the same between them
            long ownerId = long.Parse(animal.SelectSingleNode("ownerID").InnerText);

            // Passing the myID means this will be saved in the save data
            StardewValley.FarmAnimal dweller = Api.FarmAnimal.CreateFarmAnimal(defaultType, ownerId, null, null, myId);

            // Go through ALL of their child nodes and prepare to 
            // overwrite with the default dwellers
            foreach (XmlNode child in animal.ChildNodes)
            {
                switch (child.Name)
                {
                    case "defaultProduceIndex":
                    case "deluxeProduceIndex":
                    case "currentProduce":
                    case "meatIndex":
                    case "price":
                    case "daysToLay":
                    case "ageWhenMature":
                    case "harvestType":
                    case "showDifferentTextureWhenReadyForHarvest":
                    case "sound":
                    case "type":
                    case "buildingTypeILiveIn":
                    case "toolUsedForHarvest":
                        {
                            string newValue = Helpers.Reflection.GetFieldValue<object>(dweller, child.Name).ToString();

                            // Need to make bools lowercase for XML
                            if (newValue.ToString().Equals("True") || newValue.ToString().Equals("False"))
                            {
                                newValue = newValue.ToLower();
                            }

                            child.InnerText = newValue;

                            break;
                        }
                    case "frontBackBoundingBox":
                    case "sidewaysBoundingBox":
                    case "frontBackSourceRect":
                    case "sidewaysSourceRect":
                        {
                            // TODO: this is gross.
                            XmlElement newChild = doc.CreateElement(child.Name);
                            NetRectangle rectangle = Helpers.Reflection.GetFieldValue<NetRectangle>(dweller, child.Name);

                            XmlElement x = doc.CreateElement("X");
                            x.InnerText = rectangle.X.ToString();
                            newChild.AppendChild(x);

                            XmlElement y = doc.CreateElement("Y");
                            y.InnerText = rectangle.Y.ToString();
                            newChild.AppendChild(y);

                            XmlElement width = doc.CreateElement("Width");
                            width.InnerText = rectangle.Width.ToString();
                            newChild.AppendChild(width);

                            XmlElement height = doc.CreateElement("Height");
                            height.InnerText = rectangle.Height.ToString();
                            newChild.AppendChild(height);

                            XmlElement location = doc.CreateElement("Location");

                            XmlElement locationX = doc.CreateElement("X");
                            locationX.InnerText = rectangle.Value.Location.X.ToString();
                            location.AppendChild(locationX);

                            XmlElement locationY = doc.CreateElement("Y");
                            locationY.InnerText = rectangle.Value.Location.Y.ToString();
                            location.AppendChild(locationY);

                            newChild.AppendChild(location);

                            animal.ReplaceChild(newChild, child);

                            break;
                        }
                    default:
                        {
                            // Do nothing; we don't need to change every 
                            // part of the animal's save data
                            break;
                        }
                }
            }
        }
    }
}