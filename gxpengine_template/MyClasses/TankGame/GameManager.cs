using GXPEngine;
using System;
using System.Drawing;

namespace gxpengine_template.MyClasses.TankGame
{
    public class GameManager : EasyDraw
    {
        public static GameManager Instance { get; private set; }

        public EasyDraw ScoreTextMesh { get; private set; }

        bool reload;
        int sceneNum;
        Vec2 topLeft;
        Vec2 topRight;
        Vec2 bottomRight ;
        Vec2 bottomLeft;

        public GameManager(int width, int height) : base(width, height)
        {
            Instance = this;

            topLeft = new Vec2(10, 30);
            topRight = new Vec2(width - 10, 10);
            bottomRight = new Vec2(width - 10, height - 10);
            bottomLeft = new Vec2(10, height - 10);

            MyGame.main.OnAfterStep += LoadSceneIfNeeded;
            LoadScene1();    
        }

        void LoadScene()
        {
            switch (sceneNum)
            {
                case 0:
                    LoadScene1();
                    break;
                case 1:
                    LoadScene2();
                    break;
                default:
                    LoadScene1();
                    break;
            }
        }

        void ChangeScene()
        {
            if(Input.GetKeyDown(Key.ONE))
            {
                sceneNum = 0;
                reload = true;
            }
            else if (Input.GetKeyDown(Key.TWO))
            {
                sceneNum = 1;
                reload = true;

            }
        }

        void LoadScene2()
        {
            var player = new Player(-Vec2.up * 20, new Vec2(100, 100), 40);
            AddChild(player);

            //var whiteHole = new WhiteHole(new Vec2(game.width / 2 + 150, game.height / 2 - 150), 40, .9f);
            //AddChild(whiteHole);

            AddChild(new Edge(topLeft, topRight));
            AddChild(new Edge(topRight, bottomRight));
            AddChild(new Edge(bottomLeft, bottomRight));
            AddChild(new Edge(topLeft, bottomLeft));


            var enemy = new Enemy(Vec2.up * 5, new Vec2(game.width / 2 + 100, game.height / 2 + 100), 40, player, 100, Color.Red);
            AddChild(enemy);
            var enemy2 = new Enemy(Vec2.one * 2, new Vec2(game.width / 2 + 200, game.height / 2 + 100), 40, player, 100, Color.Red);
            AddChild(enemy2);
        }

        void LoadScene1()
        {

            ScoreTextMesh = new EasyDraw(100, 100);
            AddChild(ScoreTextMesh);
            ScoreTextMesh.TextAlign(CenterMode.Min, CenterMode.Min);
            ScoreTextMesh.Stroke(Color.White);
            ScoreTextMesh.Text("Score: 0");

            var player = new Player(-Vec2.up * 5, new Vec2(100, 100), 40);
            AddChild(player);

            var whiteHole = new WhiteHole(new Vec2(game.width / 2 - 200, game.height / 2 - 100), 40, 1000f);
            AddChild(whiteHole);

            var topLeft = new Vec2(10, 30);
            var topRight = new Vec2(width - 10, 10);
            var bottomRight = new Vec2(width - 10, height - 10);
            var bottomLeft = new Vec2(10, height - 10);

            var center1 = new Vec2(width / 2, 10);
            var center2 = new Vec2(center1.x, height / 2 - 100);
            var center3 = new Vec2(center1.x + 100, height - 300);
            var center4 = new Vec2(center1.x, height - 10);
            var center5 = new Vec2(center1.x + 200, center1.y) ;

            AddChild(new Edge(topLeft, topRight));
            AddChild(new Edge(topRight, bottomRight));
            AddChild(new Edge(bottomLeft, bottomRight));
            AddChild(new Edge(topLeft, bottomLeft));
            AddChild(new Edge(center1, center2));
            AddChild(new Edge(center3, center4));
            AddChild(new Edge(center2, center5));

            BuildWall(2, 4, center2 + new Vec2(0, 30));


            var enemy = new Enemy(Vec2.up, new Vec2(game.width / 2 + 200, game.height / 2 - 200), 40, player, 50, Color.Red, health: 6);
            AddChild(enemy);
            var enemy2 = new Enemy(Vec2.one , new Vec2(game.width / 2 + 200, game.height / 2 + 100), 40, player, 50, Color.Red, health: 5);
            AddChild(enemy2);
        }

        void Update()
        {
            if (Input.GetKeyDown(Key.R))
                reload = true;
            ChangeScene();
        }

        void LoadSceneIfNeeded()
        {
            if (!reload) return;
            reload = false;
            foreach (var child in GetChildren())
                child.Destroy();
            ClearTransparent();
            LoadScene();
        }


        void BuildWall(int wallCols, int wallRows, Vec2 offset)
        {
            int radius = 20;
            int diameter = radius * 2;
            int spacing = 3;
            for (int i = 0; i < wallCols; i++)
            {
                for (int j = 0; j < wallRows; j++)
                {
                    var circle = new MovingBall(Vec2.zero, offset + new Vec2((diameter + spacing) * i, (diameter + spacing) * j),radius, Color.Gray, density: 10f);
                    AddChild(circle);
                    var wallHealth = new Health(5);
                    circle.AddChild(wallHealth);
                }
            }
        }

        protected override void OnDestroy()
        {
            MyGame.main.OnBeforeStep -= LoadSceneIfNeeded;
            Console.WriteLine("re");
            Instance = null;
        }
    }
}
