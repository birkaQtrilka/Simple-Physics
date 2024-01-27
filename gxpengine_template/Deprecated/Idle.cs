using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public class Idle : MovementState
    {
        public Idle(MovementStateMachine context) : base(context) { }
        bool _wasPressed;
        
        public override void Update()
        {
            if (context.DirectionInput.x != 0 && !_wasPressed)
            {
                context.TransitionToState(typeof(Walk));
                _wasPressed = true;
            }
            else if (context.DirectionInput.x == 0)
                _wasPressed = false;
            if (context.DirectionInput.y > 0 && context.Grounded)
            {
                context.TransitionToState(typeof(Jump));

            }
            if (!context.Grounded)
                context.TransitionToState(typeof(Fall));

        }
    }
}
