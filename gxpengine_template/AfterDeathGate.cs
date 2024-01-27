using GXPEngine;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using TiledMapParser;

namespace gxpengine_template
{
    public class AfterDeathGate : AnimationSprite
    {
        readonly string _healthHolderName;
        readonly string _levelName;
        
        GameObject _parentOnSpawn;
        IHealthHolder _healthHolder;
        bool init;
        bool searchCollisions;
        Player _player;
        public AfterDeathGate(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows)
        {
            collider.isTrigger = true;

            _healthHolderName  = data.GetStringProperty("EventHolderObjectName");
            _levelName = data.GetStringProperty("GoToLevel");
        }
        void Update()
        {
            if (searchCollisions && _player.CurrentColliders != null && _player.CurrentColliders.Contains(this))
            {
                MyUtils.MyGame.LoadLevel(_levelName);
            }

            if (init) return;
            init = true;

            _parentOnSpawn = parent;
            parent.RemoveChild(this);
            _healthHolder = MyUtils.MyGame.CurrentLevel.FindObjectsOfType<GameObject>().FirstOrDefault(o => o.name == _healthHolderName) as IHealthHolder;
            _player = MyUtils.MyGame.CurrentLevel.Player;
            Debug.Assert(_healthHolder != null);
            _healthHolder.Killed += OnVictimDeath;
        }
        private void OnVictimDeath()
        {
            _parentOnSpawn.AddChild(this);
            searchCollisions = true;
            _healthHolder.Killed -= OnVictimDeath;
        }

        protected override void OnDestroy()
        {
            if (_healthHolder != null)
                _healthHolder.Killed -= OnVictimDeath;
        }
    }
}
