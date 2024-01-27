using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TiledMapParser;

namespace gxpengine_template
{
    public abstract class GravityObject : AnimationSprite
    {
        public event Action<GameObject> TriggerStay;
        public event Action<GameObject> CollisionStay;
        public event Action<State> StateChange;
        public State CurrentState
        {
            get => _currentState;
            protected set
            {
                if (_currentState != value)
                    StateChange?.Invoke(value);
                _currentState = value;
            }
        }
        public bool Grounded { get; protected set; }

        protected Dictionary<State, Action> ActiveStates;

        public Vector2 DirectionInput;
        protected Vector2 _startPos;
        protected Vector2 _endPos;
        protected float _progress;
        protected float _beatDuration;
        protected float _resetTime;

        //configs
        protected readonly float _fallAccelerationBuildUpSpeed = .07f;
        protected readonly float _maxFallSpeed = 1;
        protected virtual float FallCurve(float t) => EaseFunc.EaseOutSin(t);
        private State _currentState;

        public GravityObject(string filename, int cols, int rows, TiledObject data, int frames = -1, bool keepInCache = false, bool addCollider = true) : base(filename, cols, rows, frames, keepInCache, addCollider)
        {
            CurrentState = State.Idle;
            _fallAccelerationBuildUpSpeed = data.GetFloatProperty("FallBuildUpSpeed", 0.07f);
            _maxFallSpeed = data.GetFloatProperty("MaxFallSpeed", 1);

            ActiveStates = new Dictionary<State, Action>()
            {
                {State.Idle, HandleIdle},
                {State.Fall, HandleFall}
            };

        }
        
        protected void Update()
        {
            HandleInput();
            ActiveStates[CurrentState]();
            HandleCollisions();
            OnUpdate();
        }
        protected virtual void OnUpdate() {}
        protected virtual bool IgnoreCollision(GameObject go) => false;
        public ReadOnlyCollection<GameObject> CurrentColliders { get;private set; }
        protected void HandleCollisions()
        {
            CurrentColliders = Array.AsReadOnly(GetCollisions(includeTriggers: true));
            var isGroundedNow = false;
            //if platform, don't be grounded until you are above the sprite
            foreach (var go in CurrentColliders)
            {
                if (go.collider.isTrigger)
                {
                    TriggerStay?.Invoke(go);
                    continue;
                }

                if (IgnoreCollision(go)) continue;

                CollisionStay?.Invoke(go);


                if (!(go is Ground ground) || (go is Platform && y > ground.y - ground.height / 2f)) continue;
                //hitting ground with head
                if (y > ground.y - ground.height / 2f)
                {
                    y = ground.y + ground.height / 2f + height / 2.1f;
                    EnterFall();
                    continue;
                }
                //sitting on ground
                if (ground.y - ground.height / 2f <= y + height / 2f)
                {
                    Grounded = true;
                    isGroundedNow = true;
                    y = ground.y - ground.height / 2f - height / 2.1f;
                    continue;
                }


            }
            if (!isGroundedNow)
                Grounded = false;
        }
        protected virtual void HandleInput()
        {
            float x = 0, y = 0;

            if (Input.GetKey(Key.D))
                x = 1;
            else if (Input.GetKey(Key.A))
                x = -1;
            if (Input.GetKey(Key.W))
                y = 1;
            else if (Input.GetKey(Key.S))
                y = -1;
            DirectionInput = new Vector2(x, y);
        }
        protected virtual void EnterIdle()
        {
            CurrentState = State.Idle;
        }
        protected virtual void HandleIdle()
        {
            if (!Grounded)
                EnterFall();
        }
        protected virtual void EnterFall()
        {
            CurrentState = State.Fall;
            _progress = 0;

        }
        protected virtual void HandleFall()
        {
            _progress += _fallAccelerationBuildUpSpeed * Time.deltaTime;
            if (_progress > 1) _progress = 1;

            var buildUp = FallCurve(_progress);
            y += buildUp * _maxFallSpeed * (10f / Time.deltaTime);

            if (Grounded)
                EnterIdle();
        }
    }
}
