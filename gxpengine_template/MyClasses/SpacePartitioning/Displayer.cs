using GXPEngine;
using gxpengine_template.MyClasses.TankGame;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace gxpengine_template.MyClasses.SpacePartitioning
{
    class QuadTree
    {
        public const int MAX_DEPTH = 4;
        public const int ALLOWED_UNITS = 4;

        readonly int depth;
        
        readonly int width;
        readonly int height;
        readonly Vec2 pos;

        //topLeft, topRight, bottomRight, bottomLeft
        readonly QuadTree[] children = new QuadTree[4];
        readonly List<INodeUnit> units = new List<INodeUnit>(ALLOWED_UNITS + 1);

        //bool _cleared = true;

        public QuadTree(int width, int height, Vec2 position, int depth = 0)
        {
            this.width = width;
            this.height = height;
            pos = position;
            this.depth = depth;

        }
        
        public void Clear()
        {
            units.Clear();
            //_cleared = true;
            //only for the first quadrants?
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != null)
                {
                    children[i].Clear();
                    children[i] = null;
                }
            }
        }

        public void Add(INodeUnit node)
        {
            if(depth >= MAX_DEPTH)
            {
                //_cleared = false;
                units.Add(node);
                return;
            }
            
            int quadrant = GetQuadrantIndex(node.Position);
            if (children[quadrant] != null /*&& !_cleared*/)
            {
                children[quadrant].Add(node);//go deeper
                return;
            }
            if(units.Count < ALLOWED_UNITS)
            {
                //_cleared = false;
                units.Add(node);
                return;
            }
            
            units.Add(node);
            foreach (var unit in units)
            {
                (int i, Vec2 quadPos) = GetQuadrantIndexAndPos(unit.Position);

                if(children[i] == null)
                    children[i] = new QuadTree(width / 2, height / 2, quadPos, depth + 1);
                
                children[i].Add(unit);
                //children[i]._cleared = false;
            }
            units.Clear();
        }

        (int, Vec2) GetQuadrantIndexAndPos(Vec2 nodePos)
        {
            if (nodePos.x < pos.x)
            {
                //bottom left
                if (nodePos.y > pos.y)
                    return (3, pos + new Vec2(-width * .25f,height * .25f));
                //top left
                return (0, pos + new Vec2(-width * .25f, -height * .25f));
            }
            //bottom right
            if (nodePos.y > pos.y)
                return (2, pos + new Vec2(width * .25f, height * .25f));
            //top right
            return (1, pos + new Vec2(width * .25f, -height * .25f));
        }

        int GetQuadrantIndex(Vec2 nodePos)
        {
            if (nodePos.x < pos.x)
            {
                //bottom left
                if (nodePos.y > pos.y)
                    return 3;
                //top left
                return 0;
            }
            //bottom right
            if (nodePos.y > pos.y)
                return 2;
            //top right
            return 1;
        }

        void DrawSelf(EasyDraw canvas)
        {
            //if(_cleared) return;
            canvas.StrokeWeight(2);
            canvas.Fill(Color.Aqua,50);
            canvas.Stroke(Color.White);
            canvas.ShapeAlign(CenterMode.Center, CenterMode.Center);
            canvas.Rect(pos.x,pos.y, width, height);
        }

        public void Update(EasyDraw canvas)
        {
            
            foreach (var item in children)
                item?.Update(canvas);
            DrawSelf(canvas);

            foreach (var item in units)
                item.Handle(units);

        }
    }

    interface INodeUnit
    {
        Vec2 Position { get;  }
        void Handle(List<INodeUnit> others);
    }
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
            fpsShow = new EasyDraw(100, 50, false);
            AddChild(fpsShow);
            tree = new QuadTree(game.width,game.height,new Vec2(game.width/2,game.height/2));
            Instance = this;
        }

        protected override void OnDestroy()
        {
            Instance = null;
        }
        
        List<JustCircle> points = new List<JustCircle>();
        void Update()
        {
            if(Input.GetMouseButton(0))
            {
                var circle = new JustCircle(16, 16);
                circle.SetOrigin(2.5f, 2.5f);
                //var circlePos = new Vec2(Utils.Random(0, game.width), Utils.Random(0, game.height));
                AddChild(circle);
                circle.x = Input.mouseX;
                circle.y = Input.mouseY;
                points.Add(circle);
            }
            
            tree.Clear();
            ClearTransparent();

            foreach (var p in points)
            {
                p.Move();
                tree.Add(p);
            }
            tree.Update(this);


            fpsShow.Clear(Color.Black);
            fpsShow.TextAlign(CenterMode.Min, CenterMode.Min);
            fpsShow.Text("FPS: " + game.currentFps);
        }
    }
}
