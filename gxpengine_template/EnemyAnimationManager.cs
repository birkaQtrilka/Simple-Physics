using GXPEngine;
using gxpengine_template;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template
{
    public class EnemyAnimationManager : AnimationManager
    {
        public ReadOnlyDictionary<string, Animation> Animations { get; }
        readonly Enemy _context;
        public EnemyAnimationManager(Enemy context)
        {
            _context = context;
            _context.AddChild(this);
            
            byte speed = 4;
            
            _animations = new Dictionary<string, Animation>()
            {
                {"Idle", new Animation( _context, new AnimationSprite("EnemyAnimations/Idle.png", 8, 1, 8, false, false), 0, 8,animDelay: speed) },
                {"Walk", new Animation(_context, new AnimationSprite("EnemyAnimations/Walk.png", 8, 1, 8, false, false), 0, 8,animDelay: speed) },
                {"Fall", new Animation(_context, new AnimationSprite("EnemyAnimations/Jump.png", 13, 1, 13, false, false), 6, 3,animDelay: 3) },
                {"Attack", new Animation(_context, new AnimationSprite("EnemyAnimations/Attack_2.png", 4, 1, 4, false, false), 0, 4,animDelay: 6, loop: false, exitTime: 100 ) },
                {"Damaged", new Animation(_context, new AnimationSprite("EnemyAnimations/Hurt.png", 6, 1, 6, false, false), 0, 6,animDelay: 2, false, exitTime: 150 ) },
                {"Death", new Animation(_context, new AnimationSprite("EnemyAnimations/Dead.png", 3, 1, 3, false, false), 0, 3,animDelay: speed, false,exitTime: 150 ) },
            };
            _context.StateChange += OnStateChange;
            _context.Attacker.Attacked += OnAttack;
            _context.RecievedDamage += OnDamaged;
            _context.Killed += OnZeroHealth;
            //ask how to refactor code
            _animations["Damaged"].AnimationLoopEnd += OnTriggerEnd; 
            _animations["Attack"].AnimationLoopEnd += OnTriggerEnd;
            _animations["Death"].AnimationLoopEnd += OnTriggerEnd;

            _currAnimation = _animations["Idle"];
            _currAnimation.StartAnim();

            Animations = new ReadOnlyDictionary<string, Animation>(_animations);

        }
        protected override void OnDestroy()
        {
            _context.StateChange -= OnStateChange;
            _context.Attacker.Attacked -= OnAttack;
            _context.RecievedDamage -= OnDamaged;
            _context.Killed -= OnZeroHealth;

            _animations["Damaged"].AnimationLoopEnd -= OnTriggerEnd; 
            _animations["Attack"].AnimationLoopEnd -= OnTriggerEnd;
            _animations["Death"].AnimationLoopEnd -= OnTriggerEnd;
        }
        void OnStateChange(State state)
        {
            _prevTrigger = null;

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
            _triggers.Add(new Trigger("Damaged", 1));
        }
        void OnAttack()
        {
            _triggers.Add(new Trigger("Attack", 0));
        }
        void OnZeroHealth()
        {
            _triggers.Add(new Trigger("Death", 2));
        }
        void OnTriggerEnd()
        {
            OnStateChange(_context.CurrentState);
        }
        Trigger? _prevTrigger;
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
