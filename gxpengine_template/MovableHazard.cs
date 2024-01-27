using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class MovableHazard : MovableObject
    {

        readonly IAct[] _movePattern;
        int _currPatternIndex;
        bool _performedPattern;

        Sprite _deathCollider;
        public MovableHazard(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            collider.isTrigger = true;

            _movePattern = MovePatternFactory(data);

            _deathCollider = new Sprite("square.png");
            _deathCollider.collider.isTrigger = true;
            _deathCollider.SetCenterOrigin();

            //offset collider to only hit objects in front and above
            _deathCollider.width = width;
            _deathCollider.height = height;

            AddChild(_deathCollider);
            _deathCollider.x = 3;
            _deathCollider.y = -3;
            _deathCollider.alpha = 0;

            BeatManager.Instance.Beat += OnBeat;
        }

        IAct[] MovePatternFactory(TiledObject data)
        {
            var moveLeft = new MoveAct(this, -1);
            var moveRight = new MoveAct(this, 1);
            var sitStill = new MoveAct(this, 0);

            IAct GetActByName(string name)
            {
                switch (name)
                {
                    case "moveLeft":
                        return moveLeft;
                    case "moveRight":
                        return moveRight;
                    case "stay":
                        return sitStill;
                    default:
                        throw new Exception("name from property isn't supported to return an IAct instance for " + name);
                }
            }

            return data.GetStringProperty("MovePattern").Split(',').Select(x => GetActByName(x)).ToArray();
        }

        private void OnBeat()
        {
            if (_movePattern.Length == 0 || _movePattern[0] == null)
                return;
            _movePattern[_currPatternIndex].Perform();

            if (++_currPatternIndex == _movePattern.Length)
                _currPatternIndex = 0;
            var safetyCheck = 0;
            while (_movePattern[_currPatternIndex].IsInstant)
            {
                _movePattern[_currPatternIndex++].Perform();
                if (_currPatternIndex == _movePattern.Length)
                    _currPatternIndex = 0;
                if (safetyCheck++ > 50)
                    throw new Exception($"you can't have all move patterns of {name} be instant");
            }

            _performedPattern = true;

        }
        protected override void OnUpdate()
        {
            if(IsFacingRight)
                _deathCollider.x = 3;
            else
                _deathCollider.x = -3;

            var healthHolder = _deathCollider.GetCollisions().FirstOrDefault(o => o is IHealthHolder) as IHealthHolder;
            if (healthHolder == null) return;
            healthHolder.Health -= 9999;
        }
        protected override void OnDestroy()
        {
            BeatManager.Instance.Beat -= OnBeat;

        }

        protected override void HandleInput()
        {
            if (_performedPattern)
            {
                _performedPattern = false;
                return;
            }

            DirectionInput = Vector2.zero;

        }
    }
}
