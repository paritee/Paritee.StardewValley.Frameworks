using BetterFarmAnimalVariety.Editors;
using Paritee.StardewValleyAPI.Buidlings.AnimalShop;
using Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using Paritee.StardewValleyAPI.Menus;
using Paritee.StardewValleyAPI.Players;
using Paritee.StardewValleyAPI.Players.Actions;
using PariteePurchaseFarmAnimalMenu = Paritee.StardewValleyAPI.Menus.PurchaseFarmAnimalMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        public ModConfig Config;

        public Player Player;
        public Blue BlueFarmAnimals;
        public Void VoidFarmAnimals;
        public AnimalShop AnimalShop;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // Config
            this.Config = this.LoadConfig();

            // Asset Editors
            this.Helper.Content.AssetEditors.Add(new AnimalBirthEditor(this));
            this.Helper.Content.AssetEditors.Add(new AnimalShopEditor(this));

            // Events
            this.Helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
            this.Helper.Events.Display.RenderingActiveMenu += this.Display_RenderingActiveMenu;
            this.Helper.Events.Input.ButtonPressed += this.Input_ButtonPressed;
        }

        public override object GetApi()
        {
            return new ModApi(this.Config);
        }

        private ModConfig LoadConfig()
        {
            // Load up the config
            ModConfig config = this.Helper.ReadConfig<ModConfig>();

            // Set up the default values
            config.UpdateFarmAnimalValuesFromAppSettings();

            return config;
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.Player = new Player(Game1.player, this.Helper);

            // Set up everything else
            BlueConfig blueConfig = new BlueConfig(this.Player.HasSeenEvent(Blue.EVENT_ID));
            this.BlueFarmAnimals = new Blue(blueConfig);

            VoidConfig voidConfig = new VoidConfig(this.Config.VoidFarmAnimalsInShop, this.Player.HasCompletedQuest(Void.QUEST_ID));
            this.VoidFarmAnimals = new Void(voidConfig);

            Dictionary<Stock.Name, string[]> available = this.Config.MapFarmAnimalsToAvailableAnimalShopStock();
            StockConfig stockConfig = new StockConfig(available, this.BlueFarmAnimals, this.VoidFarmAnimals);
            Stock stock = new Stock(stockConfig);

            this.AnimalShop = new AnimalShop(stock);
        }

        private void Display_RenderingActiveMenu(object sender, RenderingActiveMenuEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady || Game1.activeClickableMenu == null)
                return;

            StardewValley.Menus.NamingMenu namingMenu = Game1.activeClickableMenu as StardewValley.Menus.NamingMenu;

            if (namingMenu == null)
                return;

            if (namingMenu.GetType() != typeof(StardewValley.Menus.NamingMenu))
                return;

            List<string> loadedTypes = this.Config.GetFarmAnimalTypes();
            BreedFarmAnimalConfig breedFarmAnimalConfig = new BreedFarmAnimalConfig(loadedTypes, this.BlueFarmAnimals);
            BreedFarmAnimal breedFarmAnimal = new BreedFarmAnimal(this.Player, breedFarmAnimalConfig);

            NameFarmAnimalMenu nameFarmAnimalMenu = new NameFarmAnimalMenu(namingMenu, breedFarmAnimal);

            nameFarmAnimalMenu.HandleChange();
        }
        
        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // We only care about left mouse clicks right now
            if (e.Button != SButton.MouseLeft)
                    return;

            ActiveClickableMenu activeClickableMenu = new ActiveClickableMenu(Game1.activeClickableMenu);

            if (!activeClickableMenu.IsOpen())
                return;

            // Purchasing a new animal
            PurchaseAnimalsMenu purchaseAnimalsMenu = activeClickableMenu.GetMenu() as PurchaseAnimalsMenu;

            if (purchaseAnimalsMenu == null)
                return;

            PurchaseFarmAnimal purchaseFarmAnimal = new PurchaseFarmAnimal(this.Player, this.AnimalShop);
            PariteePurchaseFarmAnimalMenu purchaseFarmAnimalMenu = new PariteePurchaseFarmAnimalMenu(purchaseAnimalsMenu, purchaseFarmAnimal);

            purchaseFarmAnimalMenu.HandleTap(e);
        }
    }
}
