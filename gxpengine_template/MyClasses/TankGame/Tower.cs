using GXPEngine;
using System.Drawing;

namespace gxpengine_template.MyClasses.TankGame
{
    public class Tower : Sprite
    {
        readonly float _rotateSpeed = .1f;
        readonly float _bltSpeed;
        readonly Color _bltColor;

        public Tower(Color bltColr, float rotateSpeed = .1f, float bltSpeed = 15) : base("t34.png")
        {
            _rotateSpeed = rotateSpeed;
            _bltSpeed = bltSpeed;
            _bltColor = bltColr;
        }
        
        public void LookAtSmooth(Vec2 worldPos)
        {
            var worldDir = new Vec2(worldPos.x - parent.x, worldPos.y - parent.y);
            worldDir.RotateDegrees(-parent.rotation - rotation);
            float deltaDegrees = worldDir.GetAngleDegrees();
            rotation += deltaDegrees * _rotateSpeed;
        }

        public void LookAtInstant(Vec2 worldPos)
        {
            var worldDir = new Vec2(worldPos.x - parent.x, worldPos.y - parent.y);
            worldDir.RotateDegrees(-parent.rotation - rotation);
            float deltaDegrees = worldDir.GetAngleDegrees();
            rotation += deltaDegrees;
        }

        public Bullet Shoot()
        {
            var bltWorldRotation = rotation + parent.rotation;
            var bltWorldVector = Vec2.GetUnitVectorDeg(bltWorldRotation);
            var dist = 70;
            var blt = new Bullet(bltWorldVector * _bltSpeed, new Vec2(x + parent.x + bltWorldVector.x * dist, y + parent.y + bltWorldVector.y * dist),10, _bltColor);
            GameManager.Instance.AddChild(blt);
            return blt;
        }
    }
}
