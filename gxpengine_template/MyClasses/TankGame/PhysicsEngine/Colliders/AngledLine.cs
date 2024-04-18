using GXPEngine;

namespace Physics
{
    public class AngledLine : Collider
    {
        public Vec2 Start { get; }
        public Vec2 End { get; }

        public AngledLine(GameObject pOwner, Vec2 start, Vec2 end) : base(pOwner, Vec2.Lerp(start, end, .5f))
        {
            Start = start;
            End = end;
        }

        public override CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
        {
            return null;
        }

        public override bool Overlaps(Collider other)
        {
            return false;
        }

    }
}
