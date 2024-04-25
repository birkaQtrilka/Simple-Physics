using GXPEngine;

namespace gxpengine_template.MyClasses.SpacePartitioning
{
    public struct Boundary
    {
        public Vec2 Size;
        public Vec2 Position;

        public Boundary(Vec2 size, Vec2 position)
        {
            Size = size;
            Position = position;
        }

        public bool Contains(Vec2 point)
        {
            return point.x >= Position.x - Size.x * .5f && point.x <= Position.x + Size.x * .5f
                && point.y >= Position.y - Size.y * .5f && point.y <= Position.y + Size.y * .5f;
        }
    }
}
