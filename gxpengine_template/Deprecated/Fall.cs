using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public class Fall : MovementState
    {
        //public Fall(Player context) : base(context) { }
        //float currSpeed;
        //public override void OnEnter()
        //{
        //    currSpeed = 0;
        //}
        //public override void Update()
        //{
        //    currSpeed += context.FallAccelerationBuildUpSpeed * Time.deltaTime;
        //    currSpeed = Mathf.Clamp(currSpeed, 0, context.MaxFallSpeed);
        //    //context.Rigidbody.velocity = context.FallCurve.Evaluate(currSpeed) * context.MaxFallSpeed * Time.deltaTime * -context.transform.up;
        //    context.y += currSpeed;
        //    if (context.Grounded)
        //    {
        //        //context.transform.position = context.GroundHitInfo.point + (Vector2)context.transform.up * 0.5f;
        //        context.TransitionToState(typeof(Idle));
        //    }
        //}
        public Fall(MovementStateMachine context) : base(context) { }
        float _normalizedProgress;
        public override void OnEnter()
        {
            _normalizedProgress = 0;
            //~(1 << context.gameObject.layer)
        }
        public override void Update()//how can I relate falling to bpm?// not worth it cuz it gets too fast
        {
            _normalizedProgress += context.FallAccelerationBuildUpSpeed * Time.deltaTime;
            if (_normalizedProgress > 1) _normalizedProgress = 1;

            var dir = 1;//maybe change later
            var buildUp = context.FallCurve(_normalizedProgress);

            //var pos = context.transform.position + buildUp * context.MaxFallSpeed * Time.deltaTime * dir;
            //context.x += buildUp * context.MaxFallSpeed * Time.deltaTime * dir;
            context.y += buildUp * context.MaxFallSpeed * Time.deltaTime * dir;
            if (context.Grounded)
            {
                context.TransitionToState(typeof(Idle));
            }
        }

    }
}
