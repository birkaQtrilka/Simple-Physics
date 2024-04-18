using GXPEngine;

namespace Physics
{
    public class Mover : CollisionInteractor
    {

        public Vec2 velocity;
        public Vec2 acceleration;
        public float bounciness = .5f;
        public float density = 1.1f;
        public virtual float Mass
        {
            get
            {
                return 10 * density;
            }
        }
        public Vec2 position => new Vec2(x, y);

        public Mover(Collider collider, Vec2 startVelocity) : base(collider, false) 
        {
            velocity = startVelocity;

        }

        protected void Update()
        {
            velocity += acceleration;
            var earliestCollision = ColliderManager.main.MoveUntilCollision(myCollider, velocity);

            if (earliestCollision != null)
            {
                OnCollision(earliestCollision);
                ResolveCollision(earliestCollision);
            }
            else
                myCollider.position += velocity;
            UpdateScreenPosition();
            
            acceleration = Vec2.zero;

            AfterPhysicsStep();
        }

        protected virtual void AfterPhysicsStep()
        {

        }

        void UpdateScreenPosition()
        {
            x = myCollider.position.x;
            y = myCollider.position.y;
        }

        public override void ResolveCollision(CollisionInfo col)
        {
            myCollider.position = col.pointOfImpact;


            if (col.other is Mover otherMover)
            {
                //if velocity is facing same way
                Vec2 relativeVel = velocity - otherMover.velocity;
                float dot = relativeVel.Dot(velocity);
                if (dot < 0) return;

                Vec2 u = VelocityOfCenterOfMass(this, otherMover);
                velocity -= (1 + bounciness) * (velocity - u).Dot(col.normal) * col.normal;
                otherMover.velocity -= (1 + bounciness) * (otherMover.velocity - u).Dot(-col.normal) * -col.normal;
            }
            else
            {
                Vec2 reflection = velocity.Reflect(col.normal, bounciness);
                velocity = reflection;
            }
        }

        Vec2 VelocityOfCenterOfMass(Mover a, Mover b)
        {
            return (a.Mass * a.velocity + b.Mass * b.velocity) / (a.Mass + b.Mass);
        }
    }
}
