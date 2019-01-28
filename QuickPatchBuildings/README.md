# Paritee's Quick Patch Buildings

Customize the types and species of farm animals you can raise without needing to replace the default farm animal types

## Contents

- [Get Started](#get-started)
- [For Modders](#for-modders)

## Get Started

### Install

1. Install the latest version of [SMAPI](https://smapi.io/)
2. Download the [Paritee's Quick Patch Buildings](https://www.nexusmods.com/stardewvalley/mods/0000) (QPB) mod files from Nexus Mods
4. Unzip the mod files into `Stardew Valley/Mods`
5. Run the game using SMAPI


### Add a Building

The following section explains how to add a new building with the [QPB](https://www.nexusmods.com/stardewvalley/mods/0000) mod. In this example we will be using [Paritee's Animal Troughs](https://www.nexusmods.com/stardewvalley/mods/0000) mod, but this can be done with any building type that has been loaded into `Data/Blueprint`.

2. Install the latest version of [QPB](#install)
2. Install the latest version of [Content Patcher](https://www.nexusmods.com/stardewvalley/mods/1915)
2. Unzip the [Paritee's Animal Troughs](https://www.nexusmods.com/stardewvalley/mods/0000) folder into `Stardew Valley/Mods`
4. Run the game using SMAPI


## For Modders

### Skins

It is possible to override the default spritesheets for the QPB buildings so that you can use your favourite packs (ex. add seasonal support). You need to make sure your Content Patcher skin mod adheres to the following.

#### Example

I'll be using [Paritees's Seasonal Animal Troughs](https://www.nexusmods.com/stardewvalley/mods/0000) as the Content Patcher skin mod and [Paritee's Animal Troughs](https://www.nexusmods.com/stardewvalley/mods/0000) as the QPB building in the example.

1. Add your target QPB building's mod `UniqueID` (found in `manifest.json`) as a required dependency in your Content Patcher skin mod's `manifest.json`.

```json
{
   "Name": "Paritee's Seasonal Animal Troughs",
   ..
   "ContentPackFor": {
      "UniqueID": "Pathoschild.ContentPatcher"
   },
   "Dependencies": [
      {
         "UniqueID": "Paritee.AnimalTroughs",
         "IsRequired": true
      }
   ]
}
```

2. In your Content Patcher skin mod's `content.json`, all assets should be loaded with `"Action": "EditImage"` and the `Target` should be the same as in your QPB building's `content.json`. Below is what your Content Patcher skin mod's `content.json` should look like. In this example, the mod has images in the `assets` folder for each season.

```json
{
  "Format": "1.3",
  "Changes": [
    {
      "Action": "EditImage",
      "Target": "Buildings/Trough",
      "FromFile": "assets/trough_{{season}}.png",
    },    
  ]
}
```


### Content Packs

`manifest.json`

```json
{
   .. // Standard SMAPI manifest.json format
   "ContentPackFor": {
      "UniqueID": "Paritee.QuickPatchBuildings" // Must include ContentPackFor target towards Paritee.QuickPatchBuildings
   }
}
```

`content.json`

```json
{
  "Format": "1.0",
  "Changes": [
    {
      "Type": "<unique building name>", // Must be unique against Data/Blueprints
      "Data": "<Data/Blueprints formatted value string>", // ex. of well: "390 75/3/3/-1/-1/-1/-1/null/Well/Provides a place for you to refill your watering can./Buildings/none/32/32/-1/null/Farm/1000/false",
      "Asset": "assets/<filename>.png", // All lowercase
      "Seasonal": false, // Recommend setting this to false and using Content Patcher to apply the seasonal variations
    }
    // Can have multiple entries
  ]
}
```

`/assets`

Folder to contain the building's assets - all lowercase and typically `.pngs`. If using the seasonal capability of QPB, your filename format must be `<filename>_<season>.png`.