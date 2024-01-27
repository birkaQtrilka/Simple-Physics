using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class SkyConfig : Sprite
    {
        public readonly int RepeatX;
        public float ScalingY;
        public SkyConfig(TiledObject data) : base("square.png", false, false)
        {
            RepeatX = data.GetIntProperty("RepeatX");
            ScalingY = data.GetFloatProperty("Scale");
        }
    }
}
