using GXPEngine;


namespace Physics
{
    public class CollisionInfo
    {
        public readonly Vec2 normal;
        public readonly GameObject other;
        public readonly Vec2 pointOfImpact;
        public readonly float timeOfImpact;
        public CollisionInfo(Vec2 pNormal, GameObject pOther, float pTimeOfImpact, Vec2 pPointofImpact)
        {
            normal = pNormal;
            other = pOther;
            pointOfImpact = pPointofImpact;
            timeOfImpact = pTimeOfImpact;
        }
    }
}
