using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public class Walk : MovementState
    {
        //public Walk(Player context) : base(context) { }
        //float endPosX;
        //public override void OnEnter()
        //{
        //    var sign = context.DirectionInput.x * context.forward;
        //    sign = sign < 0 ? -1 : 1;
        //    context.forward *= sign;

        //    //var hit = Physics2D.Raycast(context.transform.position, context.transform.right, 1f, 1 << LayerMask.NameToLayer("Enemy"));
        //    endPosX = context.x + context.forward;
        //    if (context.collider.HitTestPoint(context.x + context.width, context.y))
        //    {
        //        context.TransitionToState(typeof(Idle));
        //    }
        //}
        //public override void Update()
        //{
        //    context.x = Utils.Lerp(context.x, endPosX, context.WalkStep * Time.deltaTime);
        //    if (Mathf.Abs(context.x - endPosX) < 0.1f)
        //    {
        //        context.x = endPosX;

        //        context.TransitionToState(typeof(Idle));
        //    }

        //}
        public Walk(MovementStateMachine context) : base(context) { }
        Vector2 _startPos;
        Vector2 _endPos;
        float _resetTime;
        float _beatTime;

        public override void OnEnter()
        {
            context.IsFacingRight = context.DirectionInput.x > 0;
            var dir = context.IsFacingRight ? 1 : -1;
            var distance = context.WalkStep * dir;
            _endPos = new Vector2(context.x + distance, context.y);

            _startPos = new Vector2(context.x, context.y);
            _beatTime = 1f / (BeatManager.Instance.BPM / 60000f) * context.BeatsForCompletion;
            _resetTime = Time.time;

            //if something is in front, don't move
            //var hit = Physics2D.Raycast(context.transform.position, context.transform.right, context.WalkStep, ~(1 << context.gameObject.layer));
            //if (hit.collider != null)
            //{
            //    context.TransitionToState(typeof(Idle));
            //}
            
            
        }

        public override void Update()
        {
            var normalizedProgress = Mathf.Clamp((Time.time - _resetTime) / _beatTime, 0, 1);
            var easing = context.WalkCurve(normalizedProgress);

            var lerp = Vector2.Lerp(_startPos, _endPos, easing);
            context.SetXY(lerp.x,lerp.y);
            //move untill collision
            /*code not made*/

            if (normalizedProgress == 1)
            {
                if (!context.Grounded)
                {
                    context.TransitionToState(typeof(Fall));
                }
                else
                    context.TransitionToState(typeof(Idle));
            }


        }

    }
}
