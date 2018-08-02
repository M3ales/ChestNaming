using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ChestNaming.UI
{
    public class HoverText : Frame
    {
        private FrameContent Content;
        public ShadowText Text;
        public HoverText(int x, int y, Color color, FrameAnchor anchor = FrameAnchor.MidMid, List<IFrameDrawable> components = null) : base(x, y, color, anchor, FrameAnchor.TopLeft, components, null)
        {
            Text = new ShadowText("", Game1.smallFont, Game1.textColor);
            Content = new FrameContent(x, y, color, FrameAnchor.TopLeft, FrameAnchor.TopLeft, xPadding: 20, yPadding: 20)
            {
                components = new List<IFrameDrawable>()
                {
                    Text
                }
            };
            this.components = new List<IFrameDrawable>() { Content };
            this.edgeTolerance = 1980;
            Content.edgeTolerance = 1980;

        }
    }
}