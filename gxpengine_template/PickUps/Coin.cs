using TiledMapParser;
using GXPEngine;
namespace gxpengine_template
{
    public class Coin : PickUp
    {
        public Coin(string fileName, int c, int r, TiledObject data) : base(fileName, c,r,data)
        {

        }

        protected override void Grab(Player player)
        {
            player.CoinAmount++;
        }

        void Update()
        {
            AnimateFixed();
        }
        
    }
}
