using GXPEngine;
using gxpengine_template.MyClasses.TankGame;

namespace gxpengine_template.MyClasses
{
    public class MyGame : Game
    {
        bool Approximate(float givenNumber, float to, float precission = 0.0001f)
        {
            return Mathf.Abs(givenNumber - to) <= precission;
        }
        static void Main()
        {
            new MyGame().Start();
        }

        public MyGame() : base(800, 600,false,false)
        {
           LoadScene();
        }

        void LoadScene()
        {
            LateAddChild(new GameManager(game.width,game.height));
        }

    }
}
