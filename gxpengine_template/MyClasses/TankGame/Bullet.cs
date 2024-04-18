using GXPEngine;
using Physics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace gxpengine_template.MyClasses.TankGame
{

    public class Bullet : MovingBall
    {
        readonly struct GameObjectLogic
        {
            public readonly GameObject GameObject;
            public readonly Action<GameObject> Logic;

            public GameObjectLogic(GameObject gameObject, Action<GameObject> logic)
            {
                GameObject = gameObject;
                this.Logic = logic;
            }
        }
        readonly List<GameObjectLogic> listeners = new List<GameObjectLogic>();
        int _bounceAmount;
        
        public Bullet(Vec2 startVelocity, Vec2 startPos, int radius, Color clr, int bounceAmount = 4) : base(startVelocity, startPos, radius, clr)
        {
            _bounceAmount = bounceAmount;
        }

        public override void OnCollision(CollisionInfo info)
        {
            foreach (var item in listeners)
            {
                if (item.GameObject == null) continue;
                item.Logic(info.other);
            }

            if (--_bounceAmount < 1)
                Destroy();
        }

        public void OnHit(Action<GameObject> action, GameObject self)
        {
            listeners.Add(new GameObjectLogic(self, action));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            listeners.Clear();
        }


    }
}
