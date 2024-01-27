using GXPEngine;
using System.Drawing;
using TiledMapParser;

namespace gxpengine_template
{
    public class BeatVisualiser : AnimationSprite, IUserInterface
    {
        BeatManager _beatManager;
        readonly float _shrinkSpeed;
        public BeatVisualiser(TiledObject data) : base("circle.png", 1,1, addCollider: false)
        {
            float shrinkSpeed = data.GetFloatProperty("ShrinkSpeed");
            _shrinkSpeed = shrinkSpeed;
            SetOrigin(width / 2, height / 2);

        }

        public void Init()
        {
            _beatManager = BeatManager.Instance;
            _beatManager.Beat += OnBeat;

        }
        void Update()
        {
            if (_beatManager.OnBeat)
                color = (uint)Color.Green.ToArgb();
            else
                color = (uint)Color.Red.ToArgb();
            SetScaleXY(scaleX - _shrinkSpeed);
        }
        void OnBeat()
        {
            SetScaleXY(1);
        }
        protected override void OnDestroy()
        {
            _beatManager.Beat -= OnBeat;

        }

        
    }
}
