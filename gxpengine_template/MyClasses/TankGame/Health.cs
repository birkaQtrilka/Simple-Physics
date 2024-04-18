using GXPEngine;

namespace gxpengine_template.MyClasses.TankGame
{
    public class Health : GameObject
    {
        int _curr;
        public Health(int amount) {
            _curr = amount;
        }
        public void Decrease(int amount)
        {
            _curr -= amount;
            if(_curr <= 0)
            {
                parent.Destroy();
            }
        }
    }
}
