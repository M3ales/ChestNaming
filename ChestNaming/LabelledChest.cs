using Microsoft.Xna.Framework;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChestNaming
{
    public class LabelledChest
    {
        public int TileX
        {
            get;
            private set;
        }
        public int TileY
        {
            get;
            private set;
        }
        public string ChestName
        {
            get;
            private set;
        }
        public LabelledChest(int tileX, int tileY, string chestName)
        {
            TileX = tileX;
            TileY = tileY;
            ChestName = chestName;
        }
        public bool EqualsChest(Chest chest)
        {
            return chest != null && chest.boundingBox.X == TileX && chest.boundingBox.Y == TileY;
        }
    }
}
