
using GXPEngine;
using Physics;
using System.Drawing;

namespace gxpengine_template.MyClasses.TankGame
{
    public class WhiteHole : StaticObj
    {
        readonly float _forcePower;
        readonly float _radius;
        public WhiteHole(Vec2 startPos, int radius, float forcePower) : base(null, true)
        {
            _forcePower = forcePower;
            _radius = radius;
            SetCollider(new Circle(this,startPos,radius));
            var visual = new EasyDraw(radius*2, radius*2, false);
            visual.SetOrigin(radius, radius);
            AddChild(visual);
            visual.Fill(Color.Red);
            visual.Ellipse(radius,radius,radius*2,radius*2);
        }

        public override void OnTrigger(Collider collider)
        {
            if(collider.owner is Mover mover)
            {
                var dir = collider.position - myCollider.position;
                var opposingForce = dir.Normalized();
                mover.acceleration += opposingForce * _forcePower / mover.Mass  * (_radius / dir.Length);
            }
        }
    }
}
