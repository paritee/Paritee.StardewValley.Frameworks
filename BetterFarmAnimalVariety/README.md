# Paritee's Better Farm Animal Variety

Customize the types and species of farm animals you can raise without needing to replace the default farm animal types

## Contents

- [Get Started](#get-started)
- [Configure](#configure)
- [For Modders](#for-modders)

## Get Started

### Install

1. Install the latest version of [SMAPI](https://smapi.io/)
2. Install the latest version of [Content Patcher](https://www.nexusmods.com/stardewvalley/mods/1915)
3. Download the [Paritee's Better Farm Animal Variety](https://www.nexusmods.com/stardewvalley/mods/3273) (BFAV) mod files from Nexus Mods
4. Unzip the mod files into `Stardew Valley/Mods`
5. Run the game using SMAPI

### Add New Farm Animal

See [Farm Animals: Using with BFAV](https://github.com/paritee/Farm-Animals/blob/master/README.md#using-with-bfav)

## Configure

You can configure your mod at `Stardew Valley/Mods/Paritee's Better Farm Animal Variety/config.json`

### Fields

#### VoidFarmAnimalsInShop

| Value  | Description |
| ------------- | ------------- |
| `Never` | Void farm animals will never be available in [Marnie's animal shop](https://stardewvalleywiki.com/Marnie%27s_Ranch#Livestock) (default) |
| `QuestOnly` | Void farm animals will only be available in [Marnie's animal shop](https://stardewvalleywiki.com/Marnie%27s_Ranch#Livestock) if the player has completed the [Goblin Problem](https://stardewvalleywiki.com/Quests#Goblin_Problem) quest |
| `Always` | Void farm animals are always availabe in [Marnie's animal shop](https://stardewvalleywiki.com/Marnie%27s_Ranch#Livestock) |

#### FarmAnimals

##### Chickens, Cows, Dinosaurs, Ducks, Goats, Pigs, Rabbits, Sheep

| Name | Type | Description |
| ------------- | ------------- | ------------- |
| `Name` | `string` | `default` for no change or a custom display name of the species (ex. `"Dairy Cow"`) |
| `Description` | `string` | `default` for no change or a custom description of the farm animal's species (ex. `"Adults can be milked daily. A milk pail is required to harvest the milk."`) |
| `ShopIcon` | `string` | `default` for no change or the filename of your file in `Stardew Valley/Mods/Paritee's Better Farm Animal Variety/assets`. These icons are used for the species options in [Marnie's animal shop](https://stardewvalleywiki.com/Marnie%27s_Ranch#Livestock) menu. The vanilla icons are saved in the `assets` directory for your use (ex. `"animal_shop_dairy_cows.png"`) |
| `Types` | `string []` | The types of farm animals that exist in the game for this species. The types must exist in `Stardew Valley/Content/Data/FarmAnimals` (ex. `["White Cow", "Brown Cow"]`) |

### Default

Here is a sample of a default `config.json` file:

```json
{
  "VoidFarmAnimalsInShop": "Never",
  "FarmAnimals": {
    "Chickens": {
      "Name": "default",
      "Description": "default",
      "ShopIcon": "default",
      "Types": [
        "White Chicken",
        "Brown Chicken",
        "Blue Chicken",
        "Void Chicken"
      ]
    },
    "Cows": {
      "Name": "default",
      "Description": "default",
      "ShopIcon": "default",
      "Types": [
        "White Cow",
        "Brown Cow"
      ]
    },
    "Dinosaurs": {
      "Name": "default",
      "Description": "default",
      "ShopIcon": "default",
      "Types": [
        "Dinosaur"
      ]
    },
    "Ducks": {
      "Name": "default",
      "Description": "default",
      "ShopIcon": "default",
      "Types": [
        "Duck"
      ]
    },
    "Goats": {
      "Name": "default",
      "Description": "default",
      "ShopIcon": "default",
      "Types": [
        "Goat"
      ]
    },
    "Pigs": {
      "Name": "default",
      "Description": "default",
      "ShopIcon": "default",
      "Types": [
        "Pig"
      ]
    },
    "Rabbits": {
      "Name": "default",
      "Description": "default",
      "ShopIcon": "default",
      "Types": [
        "Rabbit"
      ]
    },
    "Sheep": {
      "Name": "default",
      "Description": "default",
      "ShopIcon": "default",
      "Types": [
        "Sheep"
      ]
    }
  }
}
```

### For Modders

#### BetterFarmAnimalVarietyAPI

See [SMAPI Modder Guide](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Integrations#Using_an_API) for usage. Requires [Paritee.StardewValleyAPI](https://github.com/paritee/Paritee.StardewValleyAPI).

```c#

/// <returns>Returns Dictionary<string, string[]> (ex. { "Cows", [ "White Cow", "Brown Cow" ] }</returns>
public Dictionary<string, string[]> GetGroupedFarmAnimals();

/// <param name="player">Paritee.StardewValleyAPI.Players</param>
/// <returns>Returns Paritee.StardewValleyAPI.FarmAnimals.Variations.Blue</returns>
public Blue GetBlueFarmAnimals(Player player);

/// <param name="player">Paritee.StardewValleyAPI.Players</param>
/// <returns>Returns Paritee.StardewValleyAPI.FarmAnimals.Variations.Void</returns>
public Void GetVoidFarmAnimals(Player player);

/// <param name="player">Paritee.StardewValleyAPI.Players</param>
/// <returns>Returns Paritee.StardewValleyAPI.Buidlings.AnimalShop</returns>
public AnimalShop GetAnimalShop(Player player)
```

#### Complimentary Mods

- [Generate Farm Animal Data](https://paritee.github.io/#generate-data-farmanimals-entry)
- [Paritee's Gender-Neutral Farm Animals](https://www.nexusmods.com/stardewvalley/mods/3289)
