using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public class Animation
    {
        public event Action AnimationLoopEnd;
        public event Action AnimationExit;
        readonly AnimationSprite _animSprite;
        //int _frames;
        readonly int _startFrame;
        readonly int _endFrame;
        readonly int _exitTime;
        readonly bool _loop;
        bool _endedAnimation;
        int _currExitTime;
        public Animation(Sprite context, AnimationSprite animSprite, int startFrame, int frames, byte animDelay, bool loop = true, int exitTime = 750) 
        {
            animSprite.SetCycle(startFrame, frames, animDelay);
            context.AddChild(animSprite);
            animSprite.SetOrigin(animSprite.width / 2, animSprite.height / 2);
            animSprite.SetXY(0, -animSprite.height / 2 + context.height / 2);
            animSprite.visible = false;

            //_frames = frames;
            _startFrame = startFrame;
            _endFrame = frames + startFrame;
            _loop = loop;
            _animSprite = animSprite;
            _exitTime = exitTime;
            _currExitTime = exitTime;
        }
        

        public void Update()
        {
            if (!_endedAnimation && !_loop && _animSprite.currentFrame == _endFrame - 1)
            {
                if ((_currExitTime -= Time.deltaTime) > 0) return;
                    
                EndAnim();
                AnimationLoopEnd?.Invoke();
                return;
            }
            _animSprite.Animate();
        }
        public void StartAnim()
        {
            _endedAnimation = false;
            _animSprite.SetFrame(_startFrame);
            _animSprite.visible = true;
            _currExitTime = _exitTime;
        }
        public void EndAnim()
        {
            AnimationExit?.Invoke();
            _endedAnimation = true;
            _animSprite.visible = false;
        }
        public void Mirror(bool horizontally, bool vertically)
        {
            _animSprite.Mirror(horizontally, vertically);
        }
    }
}
