using GXPEngine.Core;
using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public  class JumpableObject : MovableObject
    {

        float _atPeakTime;
        bool _reachedPeak;

        //configs
        readonly float _jumpLag = 80f;
        readonly float _jumpHeight = 64f;
        float JumpCurve(float t) => EaseFunc.EaseOutSin(t);

        public JumpableObject(string filename, int cols, int rows, TiledObject data, int frames = -1, bool keepInCache = false, bool addCollider = true) : base(filename, cols, rows, data, frames, keepInCache, addCollider)
        {
            _jumpLag = data.GetFloatProperty("JumpLag");
            _jumpHeight = data.GetFloatProperty("JumpHeight");
            ActiveStates.Add(State.Jump, HandleJump);
        }



        protected override void HandleIdle()
        {
            
            if (!Grounded)
                EnterFall();
            else if (DirectionInput.x != 0 && !_wasPressed)
            {
                EnterWalk();
                _wasPressed = true;
            }
            else if (DirectionInput.y > 0 && Grounded && !_wasPressed)
            {
                EnterJump();
                _wasPressed = true;
            }

            if (DirectionInput.x == 0 && DirectionInput.y == 0)
                _wasPressed = false;

        }

        protected virtual void EnterJump()
        {
            CurrentState = State.Jump;
            _startPos = new Vector2(x, y);
            _endPos = new Vector2(x, y - _jumpHeight);

            _beatDuration = BeatManager.Instance.BeatDuration * _amountOfBeatsForCompletion;
            _resetTime = Time.time;
            _reachedPeak = false;
        }
        protected virtual void HandleJump()
        {
            if (_reachedPeak)
            {
                if (Time.time - _atPeakTime > _jumpLag)
                    EnterFall();
                return;
            }

            _progress = Mathf.Clamp((Time.time - _resetTime) / _beatDuration, 0, 1);
            var easing = JumpCurve(_progress);
            var lerp = Vector2.Lerp(_startPos, _endPos, easing);
            SetXY(lerp.x, lerp.y);

            if (_progress != 1) return;

            _atPeakTime = Time.time;
            _reachedPeak = true;

        }
    }
}
