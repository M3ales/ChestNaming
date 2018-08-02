﻿/*Hook into Chest.preformToolAction to track removal, and possibly placement
 *  public override bool performToolAction(Tool t, GameLocation location)
{
if (t != null && t.getLastFarmerToUse() != null && t.getLastFarmerToUse() != Game1.player)
return false;
if ((bool) ((NetFieldBase<bool, NetBool>) this.playerChest))
{
if (t == null || t is MeleeWeapon || (!t.isHeavyHitter() || !base.performToolAction(t, location)))
  return false;
Farmer player = t.getLastFarmerToUse();
if (player != null)
{
  Vector2 c = player.GetToolLocation(false) / 64f;
  c.X = (float) (int) c.X;
  c.Y = (float) (int) c.Y;
  this.mutex.RequestLock((Action) (() =>
  {
    this.clearNulls();
    if (this.items.Count == 0)
    {
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using ChestNaming.UI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using Microsoft.Xna.Framework;

namespace ChestNaming
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            SaveEvents.AfterLoad += LoadData;
            SaveEvents.AfterSave += SaveData;
            LabelledChests = new List<LabelledChest>();
            ChestLabel = new HoverText(0, 0, Color.White);
            GameEvents.QuarterSecondTick += GameEvents_QuarterSecondTick;
            GraphicsEvents.OnPostRenderEvent += GraphicsEvents_OnPostRenderEvent;
        }
        #region Serialization
        private void SaveData(object sender, EventArgs e)
        {
            Monitor.Log("Saving to " + ChestNameSavePath);
            Helper.WriteJsonFile(ChestNameSavePath, LabelledChests);
            Monitor.Log("Complete");
        }

        private void LoadData(object sender, EventArgs e)
        {
            Monitor.Log("Attempting to load " + ChestNameSavePath);
            LabelledChests = Helper.ReadJsonFile<List<LabelledChest>>(ChestNameSavePath);
            if (LabelledChests == null)
            {
                Monitor.Log("ChestNames not found.");
                LabelledChests = new List<LabelledChest>();
                return;
            }
            Monitor.Log("Load successful.");
        }

        private string ChestNameSavePath
        {
            get
            {
                return SaveInfoPath("ChestNames");
            }
        }
        private string SaveInfoPath(string name)
        {
            return $"{Constants.CurrentSavePath}\\{Constants.SaveFolderName}_{ModManifest.UniqueID}_{name}.json";
        }
        #endregion

        private Chest selectedChest;
        private LabelledChest selectedLabelledChest;
        private bool firstTick = true;
        private void GameEvents_QuarterSecondTick(object sender, EventArgs e)
        {
            if (CheckForLabelledChest((int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y, out selectedChest, out selectedLabelledChest))
            {
                if (firstTick)
                {
                    Monitor.Log("Chest under mouse.");
                    firstTick = false;
                }
            }
            else
            {
                firstTick = true;
            }
        }

        public List<LabelledChest> LabelledChests;
        public bool CheckForLabelledChest(int xTile, int yTile, out Chest chest, out LabelledChest label)
        {
            chest = null;
            label = null;
            if (Game1.player == null || Game1.player.currentLocation == null)
                return false;
            StardewValley.Object objectAtTile = Game1.player.currentLocation.getObjectAtTile(xTile, yTile);
            if (objectAtTile != null)
            {
                if (objectAtTile is Chest)
                {
                    chest = (Chest)objectAtTile;
                    if (chest.playerChest.Value)
                    {
                        foreach (LabelledChest c in LabelledChests)
                        {
                            if (c.EqualsChest(chest))
                            {
                                label = c;
                                return true;
                            }
                        }
                    }
                    chest = null;
                }
            }
            else
            {
                objectAtTile = Game1.player.currentLocation.getObjectAtTile(xTile, yTile + 1);
                if (objectAtTile != null)
                {
                    return CheckForLabelledChest(xTile, yTile + 1, out chest, out label);
                }
            }
            return false;
        }

        private void GraphicsEvents_OnPostRenderEvent(object sender, EventArgs e)
        {
            if (selectedLabelledChest != null)
            {
                int offset = selectedChest.boundingBox.Height - 10;
                ChestLabel.Text.text = selectedLabelledChest.ChestName;
                int posX = (selectedChest.boundingBox.X) - Game1.viewport.X;
                int posY = (selectedChest.boundingBox.Y) - Game1.viewport.Y - offset;
                ChestLabel.localX = posX + Game1.tileSize/2;
                ChestLabel.localY = posY;
                ChestLabel.Draw(Game1.spriteBatch, null);
            }
        }

        public HoverText ChestLabel;

    }
}
