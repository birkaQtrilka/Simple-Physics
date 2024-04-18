using GXPEngine;
using Physics;
using System.Drawing;

namespace gxpengine_template.MyClasses.TankGame
{
    public class MovingBall : Mover
    {
        public readonly int Radius;
        public override float Mass
        {
            get
            {
                return Radius * Radius * density;
            }
        }


        public MovingBall( Vec2 startVelocity, Vec2 startPos, int radius, Color clr, float density = 1.1f) : base(null, startVelocity)
        {
            Radius = radius;
            this.density = density;
            SetCollider(new Circle(this, startPos, radius));

            var visual = new EasyDraw(radius * 2, radius * 2);
            visual.SetOrigin(radius, radius);
            AddChild(visual);
            visual.Fill(clr);
            visual.Stroke(clr);
            visual.Ellipse(radius, radius, radius * 2, radius * 2);

        }

    }
}
