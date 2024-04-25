using GXPEngine;
using gxpengine_template.MyClasses.TankGame;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace gxpengine_template.MyClasses.SpacePartitioning
{

    class JustCircle : EasyDraw, INodeUnit
    {
        public Vec2 Position
        {
            get => new Vec2(x,y);
        }
        public JustCircle(int width, int height) : base(width, height, false)
        {
            Clear(Color.OrangeRed);
        }

        public void Move()
        {
            x += Utils.Random(-4, 4);
            y += Utils.Random(-4, 4);

            if (x > game.width)
                x = 0;
            else if (x < 0)
                x = game.width;

            if (y > game.height)
                y = 0;
            else if (y < 0)
                y = game.height;

        }

        public void Handle(List<INodeUnit> others)
        {

            foreach (var item in others)
            {
                var i = item as GameObject;
                Displayer.Instance.Line(x, y, i.x, i.y);
            }


        }
    }

    public class Displayer : EasyDraw
    {
        QuadTree tree;
        EasyDraw fpsShow;
        public static Displayer Instance;
        public Displayer(int width, int height) : base(width,height,false)
        {
            fpsShow = new EasyDraw(200, 50, false);
            AddChild(fpsShow);
            tree = new QuadTree(new Boundary(new Vec2(game.width, game.height),new Vec2(game.width / 2, game.height / 2)), 4, 4);
            Instance = this;
            ShapeAlign(CenterMode.Center, CenterMode.Center);

        }

        protected override void OnDestroy()
        {
            Instance = null;
        }
        
        List<JustCircle> points = new List<JustCircle>();
        void Update()
        {
            if(Input.GetMouseButton(0) && Input.GetKey(Key.LEFT_SHIFT))
            {
                var circle = new JustCircle(5, 5);
                circle.SetOrigin(2.5f, 2.5f);
                //var circlePos = new Vec2(Utils.Random(0, game.width), Utils.Random(0, game.height));
                AddChild(circle);
                circle.x = Input.mouseX;
                circle.y = Input.mouseY;
                points.Add(circle);
            }
            if(Input.GetMouseButtonDown(0))
            {
                var circle = new JustCircle(5, 5);
                circle.SetOrigin(2.5f, 2.5f);
                //var circlePos = new Vec2(Utils.Random(0, game.width), Utils.Random(0, game.height));
                AddChild(circle);
                circle.x = Input.mouseX;
                circle.y = Input.mouseY;
                points.Add(circle);
            }
            if (Input.GetMouseButtonDown(1))
            {
                foreach (var item in points)
                {
                    item.LateDestroy();
                }
                points.Clear();
            }
            tree = new QuadTree(new Boundary(new Vec2(game.width, game.height), new Vec2(game.width / 2, game.height / 2)), 4, 4);
            ClearTransparent();

            foreach (var p in points)
            {
                p.Move();
                tree.Add(p);
            }
            tree.Update(this);


            fpsShow.Clear(Color.Black);
            fpsShow.TextAlign(CenterMode.Min, CenterMode.Min);
            fpsShow.Text("FPS: " + game.currentFps + "\nBall count: " + points.Count);
        }
    }
}
