using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class BouncingCoin : Coin, IPrefab
    {
        private const float MIN_MOVEMENT_VALUE = 0.01f;

        //sometimes coin may stay multiple frames in ground so I need to bounce only once
        bool _bounced;
        float _yVel;
        bool _stopMoving;
        float _gravity;
        float _ellasticity;
        float _terminalVel;

        readonly string _fileName;
        readonly int _c, _r;
        readonly TiledObject _data;
        public BouncingCoin(string fileName, int c, int r, TiledObject data) : base(fileName, c, r, data)
        {
            _yVel = data.GetFloatProperty("SpawnVel", -10);
            _ellasticity = data.GetFloatProperty("Ellasticity", .9f);
            _gravity = data.GetFloatProperty("Gravity", .5f);
            _terminalVel = data.GetFloatProperty("TerminalVel", 5);

            _fileName = fileName;
            _c = c;
            _r = r;
            _data = data;
            //this.ResizePreservingAspectRatio(Mathf.Floor(data.Width));
            //this.SetCenterOrigin();
        }

        public GameObject Clone()
        {
            return new BouncingCoin(_fileName, _c, _r, _data);
        }

        public void Config(float force, float ellasticity = 0.7f, float gravity = 0.5f, float terminalVel = 10)
        {
            _yVel = -force;
            _ellasticity = ellasticity;
            _gravity = gravity;
            _terminalVel = terminalVel;
            //solved by implementing a prefab system

            //SetOrigin(width / 2, height / 2);
        }
        void Update()
        {
            AnimateFixed();
            if (_stopMoving) return;
            var ground = GetCollisions(includeTriggers: false).FirstOrDefault(c => c is Ground) as Sprite;

            var grounded = ground != null;

            if (grounded && !_bounced)
            {
                //bounce
                _yVel = -_yVel * _ellasticity;
                _bounced = true;
                _stopMoving = Mathf.Abs(_yVel) < MIN_MOVEMENT_VALUE;

            } 
            else if (!grounded)
            {
                _yVel += _gravity;
                _yVel = Mathf.Min(_yVel, _terminalVel);
                y += _yVel;
                _bounced = false;
                return;
            }


            var groundIsUnder = ground.y > y;
            if (groundIsUnder)
                y = ground.y - ground.height / 2 - height / 2;
            else
            {
                y = ground.y + ground.height / 2 + height / 2;
                _yVel = 0;
            }

        }
    }
}
