using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class HealthPickUp : PickUp
    {
        readonly int _healAmount;

        
        public HealthPickUp(string fileName, int c, int r, TiledObject data) : base(fileName, c, r, data)
        {
            _healAmount = data.GetIntProperty("Amount");

            
        }

        

        protected override void Grab(Player player)
        {
            player.Health += _healAmount;
        }
        void Update()
        {
            AnimateFixed();
        }
        
    }
}
