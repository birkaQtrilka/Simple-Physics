using GXPEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public abstract class Spell 
    {
        public int[] Combination => _combination;
        readonly int[] _combination;

        public SpriteData MenuImage => _menuImage;
        readonly SpriteData _menuImage;

        public Spell(int[] combination, SpriteData menuImageData) 
        {
            if (combination != null && combination.Length > 4)
            {
                Console.WriteLine($"ERROR! Can't set the combination streak {GetType()} longer than 4");
                _combination = new int[4];
            }
            _combination = combination;
            _menuImage = menuImageData;

            //MyGame.main.AddChild(_menuImage);
            //_menuImage.visible = false;
            //_menuImage.SetOrigin(_menuImage.width / 2, _menuImage.height / 2);
            
            
        }
        
    
        public abstract bool TryPerform(Player player);
    }
}
