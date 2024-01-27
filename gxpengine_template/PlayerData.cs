using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    //persistent
    public class PlayerData
    {
        public int Money { get; set; }
        public SpellMaker SpellMaker { get; }
        public PlayerData(int startMoney, SpellMaker spellMaker) 
        {
            Money = startMoney;
            SpellMaker = spellMaker;
        }
    }
}
