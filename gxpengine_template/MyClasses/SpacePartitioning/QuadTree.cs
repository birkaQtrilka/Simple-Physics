using GXPEngine;
using System.Collections.Generic;
using System.Drawing;

namespace gxpengine_template.MyClasses.SpacePartitioning
{
    public class QuadTree
    {

        readonly int maxDepth;
        readonly int depth;
        readonly int capacity;

        readonly Boundary box;

        QuadTree topLeft;
        QuadTree topRight;
        QuadTree bottomLeft;
        QuadTree bottomRight;

        readonly List<INodeUnit> units = new List<INodeUnit>(4);
        bool _cleared = true;
        bool _divided;
        public QuadTree(Boundary box, int maxDepth, int capacity, int depth = 0)
        {
            this.box = box;
            this.depth = depth;
            this.maxDepth = maxDepth;
            this.capacity = capacity;
        }
        
        public void Clear()
        {
            units.Clear();
            
            topLeft?.Clear();
            topRight?.Clear();
            bottomLeft?.Clear();
            bottomRight?.Clear();
        }

        public bool Add(INodeUnit node)
        {
            if (!box.Contains(node.Position))
                return false;

            if (units.Count < capacity)
                units.Add(node);
            else
            {
                if (!_divided)
                    Subdivide();
                if (bottomLeft.Add(node))
                    return true;
                else if (bottomRight.Add(node))
                    return true;
                else if(topLeft.Add(node))
                   return true;
                else if(topRight.Add(node))
                   return true;
            }
            return true;
        }
        void Subdivide()
        {
            bottomLeft = new QuadTree(new Boundary(box.Size / 2, box.Position + new Vec2(-box.Size.x * .25f, box.Size.y * .25f)), maxDepth, capacity, depth + 1);
            topLeft = new QuadTree(new Boundary(box.Size / 2, box.Position + new Vec2(-box.Size.x * .25f, -box.Size.y * .25f)), maxDepth, capacity, depth + 1);
            bottomRight = new QuadTree(new Boundary(box.Size / 2, box.Position + new Vec2(box.Size.x * .25f, box.Size.y * .25f)), maxDepth, capacity, depth + 1);
            topRight = new QuadTree(new Boundary(box.Size / 2, box.Position + new Vec2(box.Size.x * .25f, -box.Size.y * .25f)), maxDepth, capacity, depth + 1);
            _divided = true;
        }
        QuadTree SetQuadrant(Vec2 nodePos)
        {
            if (nodePos.x < box.Position.x)
            {
                if (nodePos.y > box.Position.y)
                {
                    if(bottomLeft == null)
                        bottomLeft  = new QuadTree(new Boundary(box.Size / 2, box.Position + new Vec2(-box.Size.x * .25f, box.Size.y * .25f) ), maxDepth, capacity, depth + 1);
                    return bottomLeft;
                }
                if(topLeft == null)
                    topLeft = new QuadTree(new Boundary(box.Size / 2, box.Position + new Vec2(-box.Size.x * .25f, -box.Size.y * .25f)), maxDepth, capacity, depth + 1);
                return topLeft;
            }
            if (nodePos.y > box.Position.y)
            {
                if(bottomRight == null)
                    bottomRight = new QuadTree(new Boundary(box.Size / 2, box.Position + new Vec2(box.Size.x * .25f, box.Size.y * .25f)), maxDepth, capacity, depth + 1);
                return bottomRight;
            }
            if(topRight == null)
                topRight = new QuadTree(new Boundary(box.Size / 2, box.Position + new Vec2(box.Size.x * .25f, -box.Size.y * .25f)), maxDepth, capacity, depth + 1);
            return topRight;
        }

        QuadTree GetQuadrantIndex(Vec2 nodePos)
        {
            if (nodePos.x < box.Position.x)
            {
                //bottom left
                if (nodePos.y > box.Position.y)
                    return bottomLeft;
                //top left
                return topLeft;
            }
            //bottom right
            if (nodePos.y > box.Position.y)
                return bottomRight;
            //top right
            return topRight;
        }

        void DrawSelf(EasyDraw canvas)
        {
            
            canvas.StrokeWeight(2);
            canvas.Fill(Color.Aqua,50);
            canvas.Stroke(Color.White);
            canvas.Rect(box.Position.x,box.Position.y, box.Size.x, box.Size.y);
        }

        public void Update(EasyDraw canvas)
        {
            
                topRight?.Update(canvas);
                topLeft?.Update(canvas);
                bottomRight?.Update(canvas);
                bottomLeft?.Update(canvas);
            //DrawSelf(canvas);

            foreach (var item in units)
                item.Handle(units);

        }
    }
}
