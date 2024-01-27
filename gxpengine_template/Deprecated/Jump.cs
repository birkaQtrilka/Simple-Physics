using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public class Jump : MovementState
    {
        //public Jump(Player context) : base(context) { }
        //float endPosY;
        //bool canPerformJump;
        //float currLagTime;
        //public override void OnEnter()
        //{
        //    endPosY = context.y + 2;
        //    canPerformJump = false;
        //    currLagTime = context.JumpLag;
        //}
        //public override void Update()
        //{
        //    context.y = Utils.Lerp(context.y, endPosY, context.JumpHeight * Time.deltaTime);

        //    if (Mathf.Abs(context.x - endPosY) < 0.1f && !canPerformJump)
        //    {
        //        canPerformJump = true;
        //    }
        //    if (canPerformJump && (currLagTime -= Time.deltaTime) > 0)
        //    {
        //        context.y = endPosY;
        //        context.TransitionToState(typeof(Fall));

        //    }
        //}

        public Jump(MovementStateMachine context) : base(context) { }
        Vector2 _endPos;
        Vector2 _startPos;
        float _resetTime;
        float _beatTime;

        float _atPeakTime;
        bool _reachedPeak;
        public override void OnEnter()
        {
            _startPos = new Vector2(context.x, context.y);
            _endPos = new Vector2(context.x, context.y + context.JumpHeight);

            //_endPos = context.transform.position + context.JumpHeight * context.transform.up;
            _beatTime = 1f / (BeatManager.Instance.BPM / 60000f) * context.BeatsForCompletion;
            _resetTime = Time.time;
            _reachedPeak = false;
        }
        public override void Update()
        {
            if(_reachedPeak)
            {
                //PerformJumpLag();
                if (Time.time - _atPeakTime > context.JumpLag)
                    context.TransitionToState(typeof(Fall));
                return;
            }
            var normalizedProgress = Mathf.Clamp((Time.time - _resetTime) / _beatTime, 0, 1);
            var easing = context.JumpCurve(normalizedProgress);
            var lerp = Vector2.Lerp(_startPos, _endPos, easing);
            context.SetXY(lerp.x, lerp.y);

            if (normalizedProgress == 1)
            {
                _atPeakTime = Time.time;
                _reachedPeak = true;
            }

        }
        
        void PerformJumpLag()
        {
            if(Time.time - _atPeakTime > context.JumpLag)
                context.TransitionToState(typeof(Fall));
        }
    }
}
