using GXPEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class ComboUI : AnimationSprite, IUserInterface
    {
        readonly int w;
        readonly int h;
        public ComboUI(TiledObject data) : base("square.png",1,1,-1,false, false)
        {
            h = (int)data.Height;
            w = (int)data.Width;

        }
        public void Init()
        {
            var textHeight = height / 4;
            for (int i = 0; i < _codeElements.Length; i++)
            {
                var tm = new TextMesh("1", width, height/4, textSize: 20);
                MyGame.main.AddChild(tm);
                tm.SetXY(x, y + (i * textHeight - height / 2 + textHeight / 2));
                _codeElements[i] = tm;
                tm.visible = false;
            }
            //alpha = 0;
            visible = false;
            height = h;
            width = w;

            var level = ((MyGame)MyGame.main).CurrentLevel;

            _spellMaker = level.FindObjectOfType<Player>().SpellMaker;

            _spellMaker.FailedCombo += OnComboFail;
            _spellMaker.ComboStep += OnComboStep;
            _spellMaker.CreatedSpell += OnSpellCreate;
            _spellMaker.FailCreatedSpell += OnFailToCreateSpell;
        }
        SpellMaker _spellMaker;
        TextMesh[] _codeElements = new TextMesh[4];
        float _backgroundAlpha = 0.7f;
        uint _defaultColor = (uint)Color.Gray.ToArgb();
        uint _successColor = (uint)Color.Green.ToArgb();
        uint _createColor = (uint)Color.Yellow.ToArgb();
        uint _failColor = (uint)Color.Red.ToArgb();
        uint _failCreateColor = (uint)Color.Black.ToArgb();

        float _colorStayTime = 300;

        int _spellIndex; 

        int _resetTime;
        bool _done = false;
        Action _afterCooldown;
        protected override void OnDestroy()
        {
            _spellMaker.FailedCombo -= OnComboFail;
            _spellMaker.ComboStep -= OnComboStep;
            _spellMaker.CreatedSpell -= OnSpellCreate;
            _spellMaker.FailCreatedSpell -= OnFailToCreateSpell;
        }
       
        void OnSpellCreate(Spell s)
        {
            color = _createColor;
            _spellIndex = 0;
            ResetCooldown();
            _afterCooldown = Hide;
            

        }
        void OnFailToCreateSpell()
        {
            color = _failCreateColor;
            _spellIndex = 0;

            ResetCooldown();
            _afterCooldown = Hide;
            
        }
        void OnComboStep(int code)
        {
            if (_spellIndex == 0)
            {
                foreach (var c in _codeElements)
                    c.visible = false;

                visible = true;
            }

            color = _successColor;
            var codeElement = _codeElements[_spellIndex];
            codeElement.Text = code.ToString();
            codeElement.visible  = true;

            if (++_spellIndex == 4) return;
            ResetCooldown();
            _afterCooldown = () => color = _defaultColor;
            
        }

        void OnComboFail()
        {
            if (_spellIndex == 0) return;
            _spellIndex = 0;
            color = _failColor;
            ResetCooldown();
            _afterCooldown = Hide;
            

        }
        void Hide()
        {
            visible = false;
            foreach (var item in _codeElements)
                item.visible = false;
        }
        void ResetCooldown()
        {
            _resetTime = Time.time;
            _done = false;
        }
        void Update()
        {
            if (!_done && Time.time - _resetTime > _colorStayTime)
            {
                _afterCooldown?.Invoke();
                _done = true;
            }
        }

    }
}
