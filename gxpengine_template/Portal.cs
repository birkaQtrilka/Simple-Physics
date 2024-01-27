using GXPEngine;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template
{
    public class Portal : AnimationSprite, IPrefab
    {
        public float Range => _range;
        public Sprite EnterPortal => _enterPortal;
        readonly Sprite _enterPortal;
        readonly Sprite _exitPortal;
        readonly SpriteData _enterPortalData, _exitPortalData;
        readonly float _range;
        
        private Portal(SpriteData enterPortal, SpriteData exitPortal, float range) : base("circle.png", 1, 1, keepInCache: true, addCollider: false)
        {
            _enterPortalData = enterPortal;
            _exitPortalData = exitPortal;

            _enterPortal = enterPortal.CreateSprite();
            _exitPortal = exitPortal.CreateSprite();
            _enterPortal.name = "enterPortal";
            _enterPortal.collider.isTrigger = true;

            AddChild(_exitPortal);
            AddChild(_enterPortal);

            _range = range;
            alpha = 0;
            MyUtils.MyGame.CurrentLevel.PortalUndoHandler.PlacedPortals.Add(this);

        }

        public Portal(string filename, int cols, int rows, TiledObject data) : base("circle.png", 1, 1, keepInCache: true,addCollider: false)
        {
            _enterPortalData = new SpriteData
            (
                data.GetStringProperty("EnterPortalFileName"),
                32,
                32,
                true,
                true,
                true
            );
            _exitPortalData = new SpriteData
            (
                data.GetStringProperty("ExitPortalFileName"),
                32,
                32,
                true,
                false,
                true
            );

            _enterPortal = _enterPortalData.CreateSprite();
            _exitPortal = _exitPortalData.CreateSprite();
            _enterPortal.name = "enterPortal";
            _enterPortal.collider.isTrigger = true;

            AddChild(_exitPortal);
            AddChild(_enterPortal);

            _range = data.GetFloatProperty("Range");
            alpha = 0;
            //MyUtils.MyGame.CurrentLevel.PortalUndoHandler.PlacedPortals.Add(this);

        }
        protected override void OnDestroy()
        {
            MyUtils.MyGame.CurrentLevel.PortalUndoHandler?.PlacedPortals.Remove(this);
        }
        public void Mirror(bool xMirror) 
        {
            if(xMirror)
            {
                _enterPortal.SetOrigin(_enterPortal.texture.width, _enterPortal.texture.height / 2);
                _enterPortal.SetXY(0, 0);

                _exitPortal.SetOrigin(_exitPortal.texture.width/2, _exitPortal.texture.height / 2);
                _exitPortal.SetXY(-_range - _exitPortal.width / 2 + _enterPortal.width, 0);
            }
            else
            {
                _enterPortal.SetOrigin(0, _enterPortal.texture.height / 2);
                _enterPortal.SetXY(0, 0);

                _exitPortal.SetOrigin(_exitPortal.texture.height / 2, _exitPortal.texture.height / 2);
                _exitPortal.SetXY(_range + _exitPortal.width / 2 - _enterPortal.width, 0);
            }
        }
        
        void Update()
        {
            if (!(_enterPortal.GetCollisions().FirstOrDefault(c => c is Player) is Player player)) return;
            //this doesn't work. why?
            //var newPos = TransformPoint(_exitPortal.x /*+ _exitPortal.width/2*/, _exitPortal.y );
            //break player movement
            player.InterruptToIdle();
            player.SetXY(x + _exitPortal.x, y + _exitPortal.y);
            LateDestroy();
        }
        
        public GameObject Clone()
        {
            return new Portal(_enterPortalData,_exitPortalData, _range);
        }
    }
}
