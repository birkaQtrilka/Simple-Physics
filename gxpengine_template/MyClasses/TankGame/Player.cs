using GXPEngine;
using Physics;
using System.Drawing;
using System.Linq;

namespace gxpengine_template.MyClasses.TankGame
{
    public class Player : Mover
    {
        public readonly int Radius;
        public float accelerationSpeed = .5f;
        public float maxSpeed = 10;

        public int Score { get; private set; }
        public override float Mass
        {
            get
            {
                return Radius * Radius;
            }
        }
        readonly Tower _tower;
        public Health Health { get; }
        public Player(Vec2 startVelocity, Vec2 startPos, int radius, int health = 3) : base(null, startVelocity)
        {
            Radius = radius;
            Health = new Health(health);
            AddChild(Health);
            SetCollider(new Circle(this, startPos, radius));

            var visual = new EasyDraw(radius*2, radius*2);
            visual.SetOrigin(radius, radius);
            AddChild(visual);
            visual.Ellipse(radius,radius,radius*2,radius*2);
            
            _tower = new Tower(Color.DarkGreen,.15f);
            _tower.SetOrigin(_tower.width / 2 - 23, _tower.height / 2);

            AddChild(_tower);
        }

        protected override void AfterPhysicsStep()
        {
            var input = Vec2.zero;
            if (Input.GetKey(Key.W))
            {
                input.y -= accelerationSpeed;
            }
            if(Input.GetKey(Key.S))
            {
                input.y += accelerationSpeed;
            }
            if( Input.GetKey(Key.D))
            {
                input.x += accelerationSpeed;
            }
            if(Input.GetKey(Key.A))
            {
                input.x -= accelerationSpeed;
            }

            var a = (velocity + input).Length;
            if (velocity.Dot(input) > 0.5 && a > maxSpeed)
                acceleration = Vec2.zero;
            else
                acceleration = input.Normalized();
            
            _tower.LookAtSmooth(new Vec2(Input.mouseX, Input.mouseY));
            if(Input.GetKeyDown(Key.SPACE))
            {
                Bullet blt = _tower.Shoot();
                blt.bounciness = .98f;
                blt.OnHit(OnBulletHit, this);
            }
            
        }

        private void OnBulletHit(GameObject other)
        {
            if (other is Enemy enemy)
            {
                enemy.Health.Decrease(1);
                Score++;
                GameManager.Instance.ScoreTextMesh.ClearTransparent();
                GameManager.Instance.ScoreTextMesh.Text("Score: " + Score);
            }
            else if(other is MovingBall)
            {
                var wallHealth = other.GetChildren().FirstOrDefault(x => x is Health) as Health;
                wallHealth?.Decrease(1);
            }
        }
    }
}
