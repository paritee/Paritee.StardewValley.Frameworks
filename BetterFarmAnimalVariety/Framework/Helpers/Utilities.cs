using BetterFarmAnimalVariety.Framework.Data;
using Netcode;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace BetterFarmAnimalVariety.Framework.Helpers
{
    internal class Utilities
    {
        public static FieldInfo GetField(object obj, string field, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return obj is Type type
                ? type.GetField(field, bindingAttr)
                : obj.GetType().GetField(field, bindingAttr);
        }

        public static T GetFieldValue<T>(object obj, string field, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            FieldInfo fieldInfo = Helpers.Utilities.GetField(obj, field, bindingAttr);
            obj = obj is Type type ? null : obj;
            
            return (T)fieldInfo.GetValue(obj);
        }

        public static Multiplayer Multiplayer()
        {
            return Helpers.Utilities.GetFieldValue<Multiplayer>(typeof(Game1), "multiplayer", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static string BuildContentPath(string[] parts)
        {
            return String.Join(Helpers.Constants.ContentPathDelimiter, parts);
        }

        public static bool AssetExists<T>(string name)
        {
            return Content.Asset.Load<T>(name) != null;
        }

        public static void FixGameSave(string saveFolder, out List<TypeHistory> typesToBeMigrated)
        {
            // Track the types to be migrated for reporting
            typesToBeMigrated = new List<TypeHistory>();

            string saveFile = Path.Combine(saveFolder, Path.GetFileName(saveFolder));

            if (!File.Exists(saveFile))
            {
                throw new FileNotFoundException($"{saveFile} does not exist");
            }

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

                    // We only need to update the animals if they aren't vanilla
                    if (Framework.Api.FarmAnimal.IsVanillaType(currentType))
                    {
                        continue;
                    }

                    // Choose the default dweller based on the building
                    string typeToBeSaved = Framework.Api.FarmAnimal.GetDefaultType(buildingType);

                    // Clean the node by replace the dirty saved values from the 
                    // content of the default dwellers
                    Framework.Helpers.Utilities.CleanDirtyFarmAnimalXmlNode(ref doc, ref animals, k, typeToBeSaved, out long myId);

                    // Track the migration of this type so we can save it in our save data
                    typesToBeMigrated.Add(new TypeHistory(myId, currentType, typeToBeSaved));
                }
            }

            // Report to the usre if any animals were migrated and save the migrations
            if (typesToBeMigrated.Count > 0)
            {
                // Save the XmlDocument back to disk
                doc.Save(saveFile);

                // Deserialize the data to prepare for update
                FarmAnimalsSaveData saveData = FarmAnimalsSaveData.Deserialize();

                // Update the save data
                saveData.AddTypeHistory(typesToBeMigrated);
            }
        }

        private static void CleanDirtyFarmAnimalXmlNode(ref XmlDocument doc, ref XmlNodeList animals, int index, string defaultType, out long myId)
        {
            XmlNode animal = animals[index];

            // We will need this to pass to the dweller - myId is the 
            // only one that matters that it's the same between them
            myId = long.Parse(animal.SelectSingleNode("myID").InnerText);
            long ownerId = long.Parse(animal.SelectSingleNode("ownerID").InnerText);

            // Passing the myID means this will be saved in the save data
            StardewValley.FarmAnimal dweller = Framework.Api.FarmAnimal.CreateFarmAnimal(defaultType, ownerId, null, null, myId);

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
                            string newValue = Framework.Helpers.Utilities.GetFieldValue<object>(dweller, child.Name).ToString();

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
                            NetRectangle rectangle = Framework.Helpers.Utilities.GetFieldValue<NetRectangle>(dweller, child.Name);

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