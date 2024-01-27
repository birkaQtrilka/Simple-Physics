using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gxpengine_template
{
    public class PlayerAnimationManager : AnimationManager
    {
        readonly Player _context;
        Trigger? _prevTrigger;

        public PlayerAnimationManager(Player context) 
        {
            _context = context;
            
            byte speed = 4;
            _animations = new Dictionary<string, Animation>()
            {
                {"Idle", new Animation( _context, new AnimationSprite("PlayerIdle.png",8,1,8,false,false),0,8,speed) },
                {"Walk", new Animation(_context, new AnimationSprite("PlayerWalk.png", 8, 1, 8, false, false), 0, 8, speed) },
                {"Jump", new Animation(_context, new AnimationSprite("PlayerJump.png", 13, 1, 13, false, false), 0, 5, 3, false, 1000) },
                {"Fall", new Animation(_context, new AnimationSprite("PlayerJump.png", 13, 1, 13, false, false), 5, 4, 3) },
                {"Attack", new Animation(_context, new AnimationSprite("PlayerAttack.png", 4, 1, 4, false, false), 0, 4, 6,false, 450)},
                {"Damaged", new Animation(_context, new AnimationSprite("PlayerHurt.png", 6, 1, 6, false, false), 0, 6, speed,false,150 ) }
            };

            _context.StateChange += OnStateChange;
            _context.Attacker.Attacked += OnAttack;
            _context.RecievedDamage += OnDamaged;
            _animations["Damaged"].AnimationLoopEnd += OnTriggerEnd; ;
            _animations["Attack"].AnimationLoopEnd += OnTriggerEnd;

            _currAnimation = _animations["Idle"];
            _currAnimation.StartAnim();

        }

        protected override void OnDestroy()
        {
            _context.StateChange -= OnStateChange;
            _context.Attacker.Attacked -= OnAttack;
            _context.RecievedDamage -= OnDamaged;

            _animations["Attack"].AnimationLoopEnd -= OnTriggerEnd;
            _animations["Damaged"].AnimationLoopEnd -= OnTriggerEnd; ;
        }

        void OnStateChange(State state)
        {
            _prevTrigger = null;//idk if this is robust

            switch (state)
            {
                case State.Walk:
                    TransitionToAnim(_animations["Walk"]);
                    break;
                case State.Jump:
                    TransitionToAnim(_animations["Jump"]);
                    break;
                case State.Fall:
                    //if from walk, one type, if after jump, different type
                    TransitionToAnim(_animations["Fall"]);
                    break;
                case State.Idle:
                    TransitionToAnim(_animations["Idle"]);
                break;
            }
        }

        void OnDamaged()
        {
            _triggers.Add(new Trigger("Damaged",0));
        }

        void OnAttack()
        {
            _triggers.Add(new Trigger("Attack", 1));
        }
        
        void OnTriggerEnd()
        {
            OnStateChange(_context.CurrentState);
        }

        void Update()
        {
            if (_triggers.Count > 0)
            {
                var maxPriorityTrigger = _triggers.Aggregate((i, j) => i.Priority > j.Priority ? i : j);
                if (_prevTrigger == null || _prevTrigger.Value.Priority < maxPriorityTrigger.Priority)
                {
                    TransitionToAnim(_animations[maxPriorityTrigger.AnimName]);
                    _triggers.Remove(maxPriorityTrigger);
                    _prevTrigger = maxPriorityTrigger;
                }

            }

            _currAnimation.Mirror(!_context.IsFacingRight, false);
            _currAnimation.Update();
        }
    }
}
