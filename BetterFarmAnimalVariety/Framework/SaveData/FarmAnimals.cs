using System.Collections.Generic;
using System.Linq;
using PariteeCore = Paritee.StardewValley.Core;

namespace BetterFarmAnimalVariety.Framework.SaveData
{
    public class FarmAnimals
    {
        public List<FarmAnimal> Animals =  new List<FarmAnimal>();

        public FarmAnimals() { }

        public void Write()
        {
            Helpers.Mod.WriteSaveData<FarmAnimals>(Constants.Mod.FarmAnimalsSaveDataKey, this);
        }

        public bool HasAnyAnimals()
        {
            return this.Animals.Any();
        }

        public FarmAnimal GetAnimal(long id)
        {
            return this.Animals.FirstOrDefault(o => o.Id == id);
        }

        public void AddAnimals(List<FarmAnimal> animalToBeAdded)
        {
            foreach (FarmAnimal animal in animalToBeAdded)
            {
                this.AddAnimal(animal);
            }
        }

        public void AddAnimal(FarmAnimal animalToBeAdded)
        {
            FarmAnimal existingAnimal = this.GetAnimal(animalToBeAdded.Id);

            if (existingAnimal == null)
            {
                this.Animals.Add(animalToBeAdded);
            }
            else
            {
                // Update the list record
                existingAnimal = animalToBeAdded;
            }
        }

        public void AddAnimal(Decorators.FarmAnimal animalToBeAdded)
        {
            TypeLog typeLog = new TypeLog(animalToBeAdded.GetTypeString(), animalToBeAdded.GetTypeString());

            this.AddAnimal(animalToBeAdded, typeLog);
        }

        public void AddAnimal(Decorators.FarmAnimal animalToBeAdded, TypeLog typeLog)
        {
            FarmAnimal animal = new FarmAnimal(animalToBeAdded.GetUniqueId(), typeLog);

            this.AddAnimal(animal);
        }

        public void RemoveAnimals(List<long> ids)
        {
            if (!ids.Any())
            {
                return;
            }

            // Clean up the data file
            this.Animals = this.Animals
                .Where(o => !ids.Contains(o.Id))
                .ToList();
        }

        public void RemoveAnimal(long id)
        {
            this.RemoveAnimals(new List<long>() { id });
        }

        public bool AnimalExists(long myId)
        {
            return this.Animals.Exists(o => o.Id == myId);
        }

        public string GetSavedTypeOrDefault(StardewValley.FarmAnimal animal)
        {
            Decorators.FarmAnimal moddedAnimal = new Decorators.FarmAnimal(animal);

            return this.GetSavedTypeOrDefault(moddedAnimal);
        }

        public string GetSavedTypeOrDefault(Decorators.FarmAnimal moddedAnimal)
        {
            return this.GetSavedTypeOrDefault(moddedAnimal.GetUniqueId(), moddedAnimal.IsCoopDweller());
        }

        public string GetSavedTypeOrDefault(long animalId, bool isCoop)
        {
            FarmAnimal animal = this.GetAnimal(animalId);

            return animal == null
                ? PariteeCore.Characters.FarmAnimal.GetDefaultType(isCoop)
                : animal.GetSavedType();
        }

        public TypeLog GetTypeLog(long myId)
        {
            FarmAnimal animal = this.GetAnimal(myId);

            return animal?.TypeLog;
        }

        public void OverwriteFarmAnimal(ref Decorators.FarmAnimal moddedAnimal, string requestedType)
        {
            // WARNING!
            // Don't sanitize a farm animal's type by blue/void/brown cow chance
            // etc. or BFAV config existence here. These checks should be done 
            // in the menus, etc.

            if (!moddedAnimal.HasName())
            {
                return;
            }

            // NOTE:
            // Even for vanilla animals we want to overwrite because the vanilla 
            // constructor could turn a "White Chicken" into a "Brown Chicken" 
            // based on chance

            // Check the save entry for reloaded animals that may have their 
            // vanilla replacements saved which can't be used
            TypeLog typeHistory = this.GetTypeLog(moddedAnimal.GetUniqueId());

            // If there's a save data entry, use that; otherwise this might be 
            // an animal created before being saved (ie. created in current day)
            string currentType = typeHistory == null ? (requestedType ?? moddedAnimal.GetTypeString()) : typeHistory.Current;

            // Set the animal with the new type's data values
            moddedAnimal.UpdateFromData(currentType);
        }
    }
}
