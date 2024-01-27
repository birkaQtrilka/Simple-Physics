using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class MoneyUI : AnimationSprite, IUserInterface
    {
        Player _player;
        readonly TextMesh _textMesh;
        public MoneyUI(string fileName, int c, int r, TiledObject data) : base(fileName,c,r,addCollider: false)
        {
            _textMesh = new TextMesh("0", 100, 100, textSize:5);
            SetOrigin(width / 2, height / 2);
            AddChild(_textMesh);
            _textMesh.SetXY(0, 0);
            
        }
        public void Init()
        {
            var level = ((MyGame)MyGame.main).CurrentLevel;
            _player = level.Player;
            _player.MoneyUpdate += OnMoneyUpdate;
            _textMesh.Text = _player.CoinAmount.ToString();
        }
        void OnMoneyUpdate(int newHealth)
        {
            _textMesh.Text = newHealth.ToString();
        }
        
        protected override void OnDestroy()
        {
            _player.MoneyUpdate -= OnMoneyUpdate;
        }

        
    }
}
