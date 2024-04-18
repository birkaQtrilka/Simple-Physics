using GXPEngine;
using Physics;
using System.Drawing;

namespace gxpengine_template.MyClasses.TankGame
{
    public class Enemy : MovingBall
    {
        public Health Health => _health;
        readonly Tower _tower;
        readonly int _cooldown;
        readonly Mover _target;
        readonly Health _health;
        int _currCooldown;
        readonly float _bulletSpeed = 15;

        public Enemy(Vec2 startVelocity, Vec2 startPos, int radius, Mover target, int cooldown, Color clr, int health = 2) : base(startVelocity, startPos, radius, clr)
        {
            _tower = new Tower(Color.DarkRed,bltSpeed: _bulletSpeed);
            _tower.SetOrigin(_tower.width / 2 - 23, _tower.height / 2);
            _health = new Health(health);
            AddChild(_health);
            AddChild(_tower);
            _cooldown = cooldown;
            _currCooldown = cooldown;
            _target = target;
        }

        protected override void AfterPhysicsStep()
        {
            //do ahead aiming here
            if (_target == null) return;

            var u = _target.position - position;
            var a = Mathf.Pow(_target.velocity.Length, 2) - _bulletSpeed * _bulletSpeed;
            var b = 2 * u.Dot(_target.velocity);
            var c = Mathf.Pow(u.Length, 2);

            var delta = b * b - 4 * a * c;
            if(delta > 0)
            {
                var t = (-b - Mathf.Sqrt(delta)) / (2 * a);
                Vec2 angle = u + t * _target.velocity;
                _tower.LookAtInstant(position + angle.Normalized());

            }
            else
                _tower.LookAtSmooth(_target.position);

            if(_currCooldown-- < 0)
            {
                Bullet blt = _tower.Shoot();
                blt.OnHit(OnBulletHit, this);
                _currCooldown = _cooldown;
            }

        }

        void OnBulletHit(GameObject other)
        {
            if (other is Player player)
            {
                player.Health.Decrease(1);
            }
        }
    }
}
