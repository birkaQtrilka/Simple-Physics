using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class CheckPoint : AnimationSprite
    {
        bool _activated;
        Player _player;
        public CheckPoint(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows)
        {
            collider.isTrigger = true;
        }

        void Update()
        {
            if (_player == null)
                _player = MyUtils.MyGame.CurrentLevel.Player;
            if (_activated) return;
            
            if(_player.CurrentColliders?.FirstOrDefault( c => c == this) != null)
            {
                //I round because for some reason the parser returns values that are off by 0.5f
                MyUtils.MyGame.SetCheckPoint(new GXPEngine.Core.Vector2(Mathf.Round(x), y));
                SetFrame(1);
                _activated = true;
            }
        }
    }
}
