
using System;

namespace gxpengine_template
{
    public interface IHealthHolder 
    {
        int Health { get; set; }
        int MaxHealth { get; }
        event Action<int> HealthUpdate;
        event Action Killed;
    }
}
