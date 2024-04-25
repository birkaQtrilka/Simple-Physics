using GXPEngine;
using System.Collections.Generic;

namespace gxpengine_template.MyClasses.SpacePartitioning
{
    public interface INodeUnit
    {
        Vec2 Position { get;  }
        void Handle(List<INodeUnit> others);
    }
}
