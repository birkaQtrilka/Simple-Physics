using GXPEngine;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template
{
    public class HealthUI : AnimationSprite, IUserInterface
    {
        IHealthHolder _healthHolder;
        readonly TiledObject _serializedData;
        readonly TextMesh _textMesh;
        public HealthUI(string fileName, int c, int r,TiledObject data = null) : base(fileName,c,r, addCollider: false)
        {
            _serializedData = data;
            _textMesh = new TextMesh("0", 200, 200, textSize:100);
            SetOrigin(width/2, height/2);
            AddChild(_textMesh);
            _textMesh.SetXY(0, 0);
            SetFrame(0);
        }
        public void Init()
        {
            var level = ((MyGame)MyGame.main).CurrentLevel;
            var holderObjName = _serializedData.GetStringProperty("HealthHolderName");
            _healthHolder = level.GetChildren().FirstOrDefault(x => x is IHealthHolder && x.name == holderObjName) as IHealthHolder;
            _healthHolder.HealthUpdate += OnHealthUpdate;
            _textMesh.Text = _healthHolder.Health.ToString();
        }
        void OnHealthUpdate(int newHealth)
        {
            _textMesh.Text = newHealth.ToString();
        }

        protected override void OnDestroy()
        {
            _healthHolder.HealthUpdate -= OnHealthUpdate;
        }

        
    }
}
