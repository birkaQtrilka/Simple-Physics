using GXPEngine;
using Physics;

namespace gxpengine_template.MyClasses.TankGame
{
    public class Edge : GameObject
    {
        public Edge(Vec2 start, Vec2 end) 
        {
            var edge1 = new StaticObj(null);
            var edge2 = new StaticObj(null);
            var cap1 = new StaticObj(null);
            var cap2 = new StaticObj(null);

            edge1.SetCollider(new AngledLine(edge1, start, end));
            edge2.SetCollider(new AngledLine(edge2, end, start));
            cap1.SetCollider(new Circle(cap1, start, 0));
            cap2.SetCollider(new Circle(cap2, end, 0));

            var game = GameManager.Instance;

            AddChild(edge1);
            AddChild(edge2);
            AddChild(cap1);
            AddChild(cap2);

            game.Line(start.x, start.y, end.x, end.y);
        }

    }
}
