using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public class PickUper : GameObject
    {
        readonly Player _player;
        public PickUper(Player owner) 
        {
            _player = owner;
            _player.TriggerStay += OnTrigger;
        }

        private void OnTrigger(GameObject obj)
        {
            if(obj is PickUp pick)
            {
                pick.Take(_player);
            }
        }

        protected override void OnDestroy()
        {
            _player.TriggerStay -= OnTrigger;
        }

    }
}
