# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

- Nothing yet

## [3.0.0] - 2019-03-21
### Added
- Support for [BFAV content packs](https://github.com/paritee/Paritee.StardewValley.Frameworks/tree/bfav-3.0.0/BetterFarmAnimalVariety#content-packs)
- Full [harvest type](https://github.com/paritee/Paritee.StardewValley.FarmAnimals/tree/bfav-3.0.0#harvest-types) (i.e. `lay`, `grab`, `find`, `none`) support for any animal type via its data value string
- Applied the `<Days To Produce>` [data value](https://stardewvalleywiki.com/Modding:Animal_data#Basic_format) to all harvest types instead of just the `lay` type
- Full language support
- Integration with [MoreAnimals 3.x](https://github.com/Entoarox/StardewMods/tree/master/MoreAnimals) for animal skins
- Integration with [JsonAssets 1.x](https://github.com/spacechase0/JsonAssets) for animal produce and meat by item name
- Support for integration with [FarmExpansion 3.x](https://github.com/AdvizeGH/FarmExpansion)
- Farm animal data sanitation on game save to preserve clean saves and recover from dirty saves using save data
- The following APIs: `IsEnabled`, `GetFarmAnimalCategories`, `GetAnimalShopStock`, `GetAnimalShopIcons`, `GetRandomAnimalShopType`, `GetFarmAnimalTypeHistory`
- Support for backwards-compatible `config.json` migration from `Format:2` to  `Format:3`
- A [changelog](https://github.com/paritee/Paritee.StardewValley.Frameworks/tree/bfav-3.0.0/BetterFarmAnimalVariety)

### Changed
- Farm animal categories are managed through [BFAV content packs](https://github.com/paritee/Paritee.StardewValley.Frameworks/tree/bfav-3.0.0/BetterFarmAnimalVariety#content-packs) and not the `config.json`
- All baby animals' types are chosen based on category and produce restrictions. [Non-producing animals](https://github.com/paritee/Paritee.StardewValley.FarmAnimals/tree/bfav-3.0.0#harvest-types) are still supported while specifying the default and deluxe produce to support this change
- In the purchase animals menu, use the lowest price to determine if the player can afford the category. Animals that the player cannot afford will not be in the rotation. If at least one type, but not all, is available for purchase, the totals will be displayed in the description of the category on hover
- The animal type's price will be the actual cost of the animal instead of the category cost. The animal type's price is displayed in a scroll on the building selection screen
- Messages for hatched egg Coop event are type-agnostic
- Animals that already exist will have content pack updates automatically applied to them instead of only being applied to new animals
- `bfav_list` command has been renamed to `livestock_categories`

### Removed
- Configuration for `VoidFarmAnimalsInShop`; should be handled by other content packs and mods
- Configuration for `RandomizeNewbornFromCategory`, `RandomizeHatchlingFromCategory` and `IgnoreParentProduceCheck`
- All commands except for `bfav_list`
- The following APIs: `GetGroupedFarmAnimals`, `GetBlueFarmAnimals`, `GetVoidFarmAnimals`, `GetAnimalShop`

## [2.2.6] - 2019-03-04
### Fixed
- Unable to purchase/hatch blue chickens in the same game session as receiving the event

## [2.2.5] - 2019-03-03
### Removed
- Automatic save fixing

## [2.2.4] - 2019-02-25
### Fixed
- Animal not becoming available if required building built within the same session

## [2.2.3] - 2019-02-18
### Fixed
- Crash when navigating away from saves screen without tapping a save to load

## [2.2.2] - 2019-02-08
### Changed
- Better checking of one save at a time. Handles scenarios where one save may want to use the mod and another does not

## [2.2.1] - 2019-02-08
### Added
- Guards to commands that shouldn't be done after a save is loaded
- Substitute any farm animals whose types cannot be found in `Data/FarmAnimals` on game launch. A crash would happen if the patch's mod folder was deleted, but the animals remained

## [2.2.0] - 2019-02-07
### Added
- `IsEnabled` API

## [2.1.0] - 2019-02-06
### Added
- Commands for managing the BFAV `config.json` file from SMAPI without having to manually update the JSON
- More configurations for determining whether or not a newborn/hatchling can be a type within its category and not just from the parent/producer. If IgnoreParentProduceCheck is off, this means that a white egg could hatch into a Brown Chicken. If randomize is est to true, non-producing animals will be included like bulls and roosters
- Format and enable flag to `config.json`
- Versioning and `GetBreedFarmAnimal` to the API

## [2.0.0] - 2019-02-02
### Added
- Ability to add new categories to the animal shop menu

### Changed
- Contains breaking changes in the API and `config.json` from 1.x.x

## [1.1.0] - 2019-01-26
### Added
- API for modders to update their mods to support more animal variety

## [1.0.0] - 2019-01-20
### Added
- Customize the types and species of farm animals you can raise without needing to replace the default farm animal types
