using GXPEngine.Core;
using GXPEngine;
using System;
using TiledMapParser;
using System.Linq;

namespace gxpengine_template
{
    public abstract class MovableObject : GravityObject
    {
        public bool IsFacingRight// change this to an int that either hold 1 or -1?
        {
            get { return _isFacingRight; }
            set
            {
                _isFacingRight = value;
                Mirror(!_isFacingRight, false);
                //also change pos of front checker
                if (_isFacingRight)
                    _frontChecker.x = width / 2 + _walkStep;
                else
                    _frontChecker.x = -width / 2 - _walkStep;
            }
        }
        protected readonly float _amountOfBeatsForCompletion = .5f;

        protected bool _wasPressed;
        readonly Sprite _frontChecker;

        bool _isFacingRight;
        readonly float _walkStep = 32;
        protected virtual float WalkCurve(float t) => EaseFunc.EaseOutSin(t);
        public MovableObject(string filename, int cols, int rows,TiledObject data, int frames = -1, bool keepInCache = false, bool addCollider = true) : base(filename, cols, rows,data, frames, keepInCache, addCollider )
        {
            //add collider child that checks for objects in front
            _amountOfBeatsForCompletion = data.GetFloatProperty("BeatsPerMove", 0.5f);
            _walkStep = data.GetFloatProperty("WalkStep", 32f);

            _frontChecker = new Sprite("circle.png");
            _frontChecker.name = "fc";
            _frontChecker.collider.isTrigger = true;
            _frontChecker.SetCenterOrigin();
            _frontChecker.SetScaleXY(0.5f);
            _frontChecker.visible = false;
            IsFacingRight = true;


            ActiveStates.Add(State.Walk, HandleWalk);
        }
        
        public void InterruptToIdle() => EnterIdle();

        protected override void EnterIdle()
        {
            base.EnterIdle();
            //if I want to walk non stop while holding down the button, uncomment this
            //_wasPressed = false;

        }
        protected override void HandleIdle()
        {
            if (!Grounded)
                EnterFall();
            else if (DirectionInput.x != 0 && !_wasPressed)//if something solid is in front, don't walk
            {

                EnterWalk();

                _wasPressed = true;
            }
            else if (DirectionInput.x == 0)
                _wasPressed = false;
            
        }
        protected virtual void EnterWalk()
        {
            CurrentState = State.Walk;
            IsFacingRight = DirectionInput.x > 0;
            AddChild(_frontChecker);

            if (_frontChecker.GetCollisions().Any(c => !c.collider.isTrigger || c is Enemy))
            {
                EnterIdle();
                return;
            }

            var dir = IsFacingRight ? 1 : -1;
            var distance = _walkStep * dir;
            _endPos = new Vector2(x + distance, y);

            _startPos = new Vector2(x, y);
            _beatDuration = BeatManager.Instance.BeatDuration * _amountOfBeatsForCompletion;
            _resetTime = Time.time;
            RemoveChild(_frontChecker);
        }
        protected virtual void HandleWalk()
        {
            _progress = Mathf.Clamp((Time.time - _resetTime) / _beatDuration, 0, 1);
            var easing = WalkCurve(_progress);

            var lerp = Vector2.Lerp(_startPos, _endPos, easing);
            SetXY(lerp.x, lerp.y);

            if (_progress != 1) return;

            if (!Grounded) EnterFall();
            else EnterIdle();

        }
    }
}
