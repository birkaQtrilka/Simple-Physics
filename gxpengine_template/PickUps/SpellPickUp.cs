using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class SpellPickUp : PickUp
    {
        readonly Spell _spellPrefab;

        
        public SpellPickUp(string fileName, int c, int r, TiledObject data) : base (fileName, c,r, data)
        {
            _spellPrefab = ((MyGame)MyGame.main).Data[data.GetStringProperty("SpellPrefabName")] as Spell;
        }
        void Update()
        {
            Animate();
        }
        protected override void Grab(Player player)
        {
            var spellMaker = player.SpellMaker;
            spellMaker.AddAvailableIfNone(_spellPrefab);
            player.Slot.EquipedSpell = _spellPrefab;
        }

        
    }

}
