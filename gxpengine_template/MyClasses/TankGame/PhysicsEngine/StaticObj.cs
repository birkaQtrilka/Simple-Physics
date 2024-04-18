
namespace Physics
{
    public class StaticObj : CollisionInteractor
    {
        public StaticObj(Collider collider, bool isTrigger = false) : base(collider, isTrigger)
        {

        }

        public override void ResolveCollision(CollisionInfo colInfo)
        {

        }

        protected void Update()
        {
            if (isTrigger)
            {
                foreach (var overlap in engine.GetOverlaps(myCollider))
                {
                    OnTrigger(overlap);
                }

            }
        }

    }
}
