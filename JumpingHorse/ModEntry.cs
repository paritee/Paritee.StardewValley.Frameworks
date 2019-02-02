using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using xTile.Dimensions;

namespace JumpingHorse
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // Events
            this.Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
            {
                return;
            }

            // We only care about action buttons
            if (!e.Button.IsActionButton())
            {
                return;
            }

            // We only care about equestrians
            if (!Game1.player.isRidingHorse())
            {
                this.Monitor.Log("!Game1.player.isRidingHorse()", LogLevel.Debug);
                return;
            }

            Vector2 playerTileLocation = Game1.player.getTileLocation();

            this.Monitor.Log($"Game1.player.FacingDirection: {Game1.player.FacingDirection}", LogLevel.Debug);

            int offsetIndex = 0;

            switch(Game1.player.FacingDirection)
            {
                case (int)Movement.CardinalDirection.North:
                    offsetIndex = 2;
                    break;
                case (int)Movement.CardinalDirection.East:
                    offsetIndex = 0;
                    break;
                case (int)Movement.CardinalDirection.South:
                    offsetIndex = 3;
                    break;
                case (int)Movement.CardinalDirection.West:
                default:
                    offsetIndex = 1;
                    break;
            }



            Game1.player.tryToMoveInDirection(Game1.player.FacingDirection, true, 0, true);


            //Game1.player.setTileLocation(playerTileLocation + (Character.AdjacentTilesOffsets[offsetIndex] * 2));
            //Game1.player.Position += new Vector2(-16f, -96f);

            Game1.player.jump();
            Game1.player.mount.jump();


            //Game1.player.position.X = playerTileLocation.X + 1;
            // Game1.player.position.Y = playerTileLocation.Y + 1;




            //Game1.player.tryToMoveInDirection(Game1.player.FacingDirection, true, 0, false);

            this.Helper.Input.Suppress(e.Button);


            return;


            // No jumping from stand stills
            if (!Game1.player.isMoving())
            {
                // @TODO: move lower in check and okay out refusals?
                this.Monitor.Log("!Game1.player.isMoving()", LogLevel.Debug);
                return;
            }

            // Check to see if they clicked on an object
            Location tileLocation = new Location((int)e.Cursor.GrabTile.X, (int)e.Cursor.GrabTile.Y);

            if (!Game1.player.currentLocation.isObjectAtTile(tileLocation.X, tileLocation.Y))
            {
                this.Monitor.Log("!Game1.player.currentLocation.isObjectAtTile(tileLocation.X, tileLocation.Y)", LogLevel.Debug);
                return;
            }

            StardewValley.Object objectAtTile = Game1.currentLocation.getObjectAtTile(tileLocation.X, tileLocation.Y);



            // Make sure it's a fence
            if (!(objectAtTile is Fence))
            {
                this.Monitor.Log("!(objectAtTile is Fence)", LogLevel.Debug);
                return;
            }

            Fence fence = objectAtTile as Fence;

            // Don't jump gates or other actionable fences
            if (fence.isGate || fence.isActionable(Game1.player))
            {
                this.Monitor.Log("fence.isGate || fence.isActionable(Game1.player)", LogLevel.Debug);
                return;
            }

            // Make sure they're close enough
            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * 64, tileLocation.Y * 64, 64, 64);

            if (!objectAtTile.getBoundingBox(new Vector2(tileLocation.X, tileLocation.Y)).Intersects(rectangle))
            {
                this.Monitor.Log("!objectAtTile.getBoundingBox(new Vector2(tileLocation.X, tileLocation.Y)).Intersects(rectangle)", LogLevel.Debug);
                return;
            }

            bool safeToJump = true;

            // Check if it's safe to jump to two spaces forward
            if (!safeToJump)
            {
                // @TODO: refusal?
                return;
            }

            objectAtTile.isPassable();
            //Object:totemWarp(Farmer who)
            //Child:toss(Farmer who)

            // East: tile.X + 2
            // West: tile.X + 2
            // North: tile.Y - 2
            // South: tile.Y + 2

            // All checks passed! Time to move the horse one spot behind the object they clicked on

            // Get the adjacent tile in the same direction player is moving
            // Game1.player.direction;
            //Game1.currentLocation.isTilePassable(tileLocation, Game1.viewport);

            //Vector2 tileLocation = objectAtTile.getTileLocation();
            //foreach (Vector2 adjacentTilesOffset in Character.AdjacentTilesOffsets
            // Vector2 tileLocation2 = tileLocation1 + adjacentTilesOffset;
            // if (!location.isTileOccupied(tileLocation2, "") && location.isTilePassable(new Location((int) tileLocation2.X, (int) tileLocation2.Y), Game1.viewport) && location.isCharacterAtTile(tileLocation2) == null)
            // !Game1.player.moveUp && !Game1.player.moveDown && (!Game1.player.moveRight && !Game1.player.moveLeft
            // Character.MovePosition


            int newX = 0;
            int newY = 0;

            Point position = new Point(newX, newY);
            //Game1.player.mount.setTilePosition(position);
            //Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle(newX * 64, newY * 64, 64, 64);

            float jumpVelocity = 8.5f;
            int yJumpOffset = -1;

            //Game1.player.mount.Halt();

            Game1.player.jumpWithoutSound();

            //Child.toss
            //Game1.player.mount.yJumpVelocity = jumpVelocity;
            //Game1.player.mount.yJumpOffset = yJumpOffset;
            //Game1.player.yJumpVelocity = 7f;
            //Game1.player.yJumpOffset = -2;
            //Game1.player.mount.setMovingInFacingDirection();
            //Game1.player.tryToMoveInDirection
            //Game1.player.MovePosition
            //Game1.player.mount.MovePosition
            //Game1.player.mount.tryToMoveInDirection

            Game1.currentLocation.playSound("dwop");

            this.Helper.Input.Suppress(e.Button);
        }
    }
}
