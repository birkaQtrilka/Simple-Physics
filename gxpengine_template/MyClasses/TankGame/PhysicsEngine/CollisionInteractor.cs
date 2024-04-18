using GXPEngine;

namespace Physics
{
    public abstract class CollisionInteractor : GameObject
    {
        public Collider Collider => myCollider;

        protected Collider myCollider { get; private set; }
        protected readonly ColliderManager engine;
        protected readonly bool isTrigger;

        public CollisionInteractor(Collider collider, bool isTrigger = false)
        {
            this.isTrigger = isTrigger;
            engine = ColliderManager.main;
            if (collider == null) return;
            SetCollider(collider);
        }

        public void SetCollider(Collider col)
        {
            myCollider = col;
            if (isTrigger)
                engine.AddTriggerCollider(myCollider);
            else
                engine.AddSolidCollider(myCollider);

            x = myCollider.position.x;
            y = myCollider.position.y;
        }

        public virtual void OnCollision(CollisionInfo info) { }

        public virtual void OnTrigger(Collider collider) { }

		public abstract void ResolveCollision(CollisionInfo colInfo);

        protected override void OnDestroy()
        {
            if (isTrigger)
                engine.RemoveTriggerCollider(myCollider);
            else
                engine.RemoveSolidCollider(myCollider);
        }


    }
}
