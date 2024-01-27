using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public abstract class AnimationManager : GameObject
    {
        protected readonly struct Trigger
        {
            public readonly string AnimName;
            public readonly byte Priority;

            public Trigger(string animName, byte priority)
            { 
                AnimName = animName;
                Priority = priority;
            }
        }
        protected Dictionary<string, Animation> _animations;
        protected readonly List<Trigger> _triggers = new List<Trigger>();
        protected Animation _currAnimation;
        protected void TransitionToAnim(Animation newAnim)
        {
            _currAnimation.EndAnim();
            _currAnimation = newAnim;
            _currAnimation.StartAnim();
        }
    }
}
