# Paritee's Better Farm Animal Variety (BFAV)

Customize the types and species of farm animals you can raise without needing to replace the default farm animal types

## Contents

- [Get Started](#get-started)
- [Content Packs](#content-packs)
- [API](#api)
- [Integrations](#integrations)

## Get Started

### Install

1. Install the latest version of [SMAPI](https://smapi.io/)
2. Download the [Paritee's Better Farm Animal Variety](https://www.nexusmods.com/stardewvalley/mods/3273) mod files from Nexus Mods
3. Unzip the mod folder into `Stardew Valley/Mods`
4. Run the game using SMAPI

### Configure

You can configure your mod at `Stardew Valley/Mods/Paritee's Better Farm Animal Variety/config.json`

| Field | Description | Default |
| --- | --- | --- |
| `Format` | The format must match the major version of the BFAV mod (ex. `"3"`). | `<major version number>` |
| `IsEnabled` | Determines if the mod should be loaded or not - can be `true` or `false`. | `true` |

### Uninstall

1. Delete the mod folder from `Stardew Valley/Mods`

## Content Packs

### Install a Content Pack

1. [Follow the Install BFAV instructions](#install)
2. Download the content pack
4. Unzip the mod folder into `Stardew Valley/Mods`
5. Run the game using SMAPI

### Create a Content Pack

#### Overview

A content pack is a folder with these files:

- A `manifest.json` (see [content packs](https://stardewvalleywiki.com/Modding:Content_packs) guide)
- A `content.json` which includes the animal categories you want to modify
- Any image assets required by your categories

The `content.json` file has a list of `Categories`. Each entry is called a `category` with an associated `action`. There isn't a limit to the number of categories you can include.

Here is an example of a `content.json` with multiple actions:

```js
{
  "Format": "1.0",
  "Categories": [
  
    // Update an existing category with a new type
    {
      "Action": "Update",
      "Category": "Dairy Cow",
      "Types": [
        {
          "Type": "White Bull",
          "Data": "1/5/184/186/cow/36/64/64/64/36/64/64/64/1/false/Barn/32/32/32/32/15/5/-1/639/1500/White Bull/Barn",
          "Sprites": {
            "Adult": "assets/White Bull.png",
            "Baby": "assets/BabyWhite Bull.png",
          },
          "Localization": {
            "de": [ "Weißer Stier", "Stall" ],
          },
        },
      ],
    },
  
    // Create a brand new category with a new type
    {
      "Action": "Create",
      "Category": "Sport Horse",
      "Types": [
        {
          "Type": "Sport Horse",
          "Data": "3/4/440/-1/grunt/36/64/84/64/36/64/84/64/1/true/Barn/32/32/32/32/15/5/Shears/644/8000/Sport Horse/Barn",
          "Sprites": {
            "Adult": "assets/Sport Horse.png",
            "Baby": "assets/BabySport Horse.png",
            "ReadyForHarvest": "assets/ShearedSport Horse.png",
          },
        },
      ],
      "Buildings": [
        "Deluxe Barn"
      ],
      "AnimalShop": {
        "Name": "Sport Horse",
        "Description": "They love cave carrots and being groomed.",
        "Icon": "assets/AnimalShop.png",
        "Exclude": []
      }
    },
  ],
}
```

#### Common Fields

All categories support these common fields:

| Field | Description |
| --- | --- |
| `Action` | Can be `Create`, `Update`, or `Remove`. |
| `Category` | The unique name of the category you want to action on. Pre-loaded categories are: `Chicken`, `Dairy Cow`, `Goat`, `Duck`, `Sheep`, `Rabbit`, `Pig`, and `Dinosaur`. |

#### Actions

##### Create

Create a new, unique category (`"Action": "Create"`).

| Field | Required | Description |
| --- | --- | --- |
| `Buildings` | _Required_ | A list of animal houses that this category's types can live in. Vanilla options are: `Barn`, `Big Barn`, `Deluxe Barn`, `Coop`, `Big Coop`, `Deluxe Coop`. Buildings are added to an existing category's types unless `ForceOverrideBuildings` is used. |
| `Types` | _Required_ | A list of types that belong to this category. Types are added to an existing category's types unless `ForceOverrideTypes` is used. There isn't a limit to the number of types you can list. |
| `Types.Type` | _Required_ | The unique name of the type. The same type can belong to multiple categories. |
| `Types.Data` | _Optional if Type exists_ | A string of data values for the type. This is required if the type does not already exist. See the [Modding Animal Data](https://stardewvalleywiki.com/Modding:Animal_data) guide for help. |
| `Types.DeluxeLuckChance` | _Optional_ | A numeric value (`double`) that greatly reduces the drop rate of deluxe produce (ex. `Rabbits` have `0.02` and `Ducks` have `0.01`). |
| `Types.Sprites` |  _Optional if Type exists_ | The relative paths to the sprites. |
| `Types.Sprites.Adult` |  _Optional if Type exists_ | The relative path to the adult sprite image. Must be a `.png`. |
| `Types.Sprites.Baby` |  _Optional if Type exists_ | The relative path to the baby sprite image. Must be a `.png`. |
| `Types.Sprites.ReadyForHarvest` | _Optional if Type exists or if texture does not change when ready for harvest_ | The relative path to the sheared sprite image. Must be a `.png`. See the [Modding Animal Data](https://stardewvalleywiki.com/Modding:Animal_data) guide for help. |
| `Types.Localization` | _Optional_ | A dictionary of locales and list of display type and display house substitutions. (Ex. `"Localization": { "de": [ "Weißer Stier", "Stall" ] }`) |
| `AnimalShop` | _Optional_ | Include if you would like this animal category to be available in [Marnie's Ranch](https://stardewvalleywiki.com/Marnie%27s_Ranch). |
| `AnimalShop.Name` | _Required if AnimalShop is included_ | The name of the category in the purchase animal menu. |
| `AnimalShop.Description` | _Required if AnimalShop is included_ | The description of the category in the purchase animal menu. |
| `AnimalShop.Icon` | _Required if AnimalShop is included_ | The relative path to the icon of the category in the purchase animal menu. Must be a `.png`. BFAV comes with some animal shop icons in `assets/AnimalShop` you can base this icon on. |
| `AnimalShop.Exclude` | _Optional_ | List of names of types that should be ecxluded from the shop. Types are added to an existing category's excludes unless `ForceOverrideExclude` is used. `Void Chicken` is excluded from the `Chicken` category by default. |

##### Update

Update an existing category (`"Action": "Update"`).

| Field | Required | Description |
| --- | --- | --- |
| `Buildings` | _Optional_ | A list of animal houses that this category's types can live in. Vanilla options are: `Barn`, `Big Barn`, `Deluxe Barn`, `Coop`, `Big Coop`, `Deluxe Coop`. Buildings are added to an existing category's types unless `ForceOverrideBuildings` is used. |
| `Types` | _Optional_ | A list of types that belong to this category. Types are added to an existing category's types unless `ForceOverrideTypes` is used. There isn't a limit to the number of types you can list. |
| `Types.Type` | _Required if Types is included_ | The unique name of the type. The same type can belong to multiple categories. |
| `Types.Data` | _Optional if Type exists_ | A string of data values for the type. This is required if the type does not already exist. See the [Modding Animal Data](https://stardewvalleywiki.com/Modding:Animal_data) guide for help. |
| `Types.DeluxeLuckChance` | _Optional_ | A numeric value (`double`) that greatly reduces the drop rate of deluxe produce (ex. `Rabbits` have `0.02` and `Ducks` have `0.01`). |
| `Types.Sprites` |  _Optional if Type exists_ | The relative paths to the sprites. |
| `Types.Sprites.Adult` |  _Optional if Type exists_ | The relative path to the adult sprite image. Must be a `.png`. |
| `Types.Sprites.Baby` |  _Optional if Type exists_ | The relative path to the baby sprite image. Must be a `.png`. |
| `Types.Sprites.ReadyForHarvest` | _Optional if Type exists or if texture does not change when ready for harvest_ | The relative path to the sheared sprite image. Must be a `.png`. See the [Modding Animal Data](https://stardewvalleywiki.com/Modding:Animal_data) guide for help. |
| `Types.Localization` | _Optional_ | A dictionary of locales and list of display type and display house substitutions. (Ex. `"Localization": { "de": [ "Weißer Stier", "Stall" ] }`) |
| `AnimalShop` | _Optional_ | Include if you would like this animal category to be available in [Marnie's Ranch](https://stardewvalleywiki.com/Marnie%27s_Ranch). |
| `AnimalShop.Name` | _Optional_ | The name of the category in the purchase animal menu. |
| `AnimalShop.Description` | _Optional_ | The description of the category in the purchase animal menu. |
| `AnimalShop.Icon` | _Optional_ | The relative path to the icon of the category in the purchase animal menu. Must be a `.png`. BFAV comes with some animal shop icons in `assets/AnimalShop` you can base this icon on. |
| `AnimalShop.Exclude` | _Optional_ | List of names of types that should be ecxluded from the shop. Types are added to an existing category's excludes unless `ForceOverrideExclude` is used. (`Void Chicken` is excluded from the `Chicken` category) |
| `ForceRemoveFromShop` | _Optional_ | Always remove this category from the shop regardless of the existing caetegory's setting. Has no effect on `Create`. Can be `true` or `false`. |
| `ForceOverrideTypes` | _Optional_ | Use the types listed in this category as the only types for the category. Has no effect on `Create`. Can be `true` or `false`. |
| `ForceOverrideBuildings` | _Optional_ | Use the buildings listed in this category as the only buildings for the category. Has no effect on `Create`. Can be `true` or `false`. |
| `ForceOverrideExclude` | _Optional_ | Use the excluded types listed in this category as the only types to be excluded from the shop for the category. Has no effect on `Create`. Can be `true` or `false`. |

##### Remove

This action will remove an existing category (`"Action": "Remove"`). It is not permanent and the targeted category can be restored if removed. No extra fields required. This should not be frequently used.

## API

See [SMAPI Modder Guide](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Integrations#Using_an_API) for usage. Requires the  [Paritee.StardewValley.Core](https://github.com/paritee/Paritee.StardewValley.Core) (also available as a [Nuget Package](https://www.nuget.org/packages/Paritee.StardewValley.Core)).

### Version 3.x

```c#
/// <summary>Determine if the mod is enabled.</summary>
/// <returns>Returns bool</returns>
bool IsEnabled();

/// <summary>Get all farm animal categories that have been loaded.</summary>
/// <returns>Returns Dictionary<string, List<string>></returns>
Dictionary<string, List<string>> GetFarmAnimalCategories();

/// <param name="farm">StardewValley.Farm</param>
/// <summary>Get all livestock options.</summary>
/// <returns>Returns List<StardewValley.Object></returns>
List<StardewValley.Object> GetAnimalShopStock(StardewValley.Farm farm);

/// <summary>Get all livestock icons.</summary>
/// <returns>Returns Dictionary<string, Texture2D></returns>
Dictionary<string, Texture2D> GetAnimalShopIcons();

/// <param name="category">string</param>
/// <param name="farmer">StardewValley.Farmer</param>
/// <summary>Get a random farm animal type from the animal shop category.</summary>
/// <returns>Returns string</returns>
string GetRandomAnimalShopType(string category, StardewValley.Farmer farmer);

/// <summary>Get the farm animal's types from the save data.</summary>
/// <returns>Returns Dictionary<long, KeyValuePair<string, string>></returns>
Dictionary<long, KeyValuePair<string, string>> GetFarmAnimalTypeHistory();
```

## Integrations

Supported integrations are:

| Mod | Version Support |
| --- | --- |
| [Animal Chooser](https://www.nexusmods.com/stardewvalley/mods/2573) | `BFAV 3.x` => `AC 1.4-unofficial.1` |
| [Farm Expansion](https://www.nexusmods.com/stardewvalley/mods/130) |  `BFAV 3.x` => `FE 3.3.1-unofficial.1` |
| [More Animals](https://www.nexusmods.com/stardewvalley/mods/2274) | `BFAV 3.x` => `MA 3.x` |
