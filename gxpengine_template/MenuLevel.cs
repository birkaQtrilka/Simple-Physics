using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class MenuLevel : Level
    {
        public MenuLevel(string fileName) : base(fileName)
        {
        }
        public override void Init()
        {
            var loader = new TiledLoader(Name, game, false, autoInstance: true);
            //bg
            loader.LoadImageLayers();
            var sky = game.FindObjectOfType<Sprite>();
            sky.scale = 0.4f;
            sky.x = game.width / 2;
            sky.y = game.height / 2;
            //ui
            loader.addColliders = false;
            loader.LoadObjectGroups();
            var btn = game.FindObjectOfType<Button>();
            btn.x = game.width / 2;
            btn.y = game.height / 2;
        }
    }
}
