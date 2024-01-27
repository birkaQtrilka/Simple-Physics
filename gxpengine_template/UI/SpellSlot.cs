using GXPEngine;
using System;
using System.Collections;
using System.Drawing;
using System.Security.Policy;
using TiledMapParser;

namespace gxpengine_template
{
    public class SpellSlot : AnimationSprite, IUserInterface
    {
        
        public Spell EquipedSpell
        {
            get
            {
                return _equipedSpell;
            }
            set
            {
                _spellImage?.LateDestroy();

                if (value == null)
                    _emptyMenuSprite.visible = true;
                else
                {
                    //value is prefab
                    var newSprite = value.MenuImage.CreateSprite();
                    newSprite.ResizePreservingAspectRatio(_width);
                    newSprite.SetCenterOrigin();

                    AddChild(newSprite);
                    newSprite.visible = true;
                    newSprite.SetXY(0, 0);
                    _spellImage = newSprite;
                    _emptyMenuSprite.visible = false;
                }
                _equipedSpell = value;
            }
        }
        Player _player;
        readonly Sprite _emptyMenuSprite;
        readonly float _warningTimer;
        readonly int _width;
        readonly int _height;
        Spell _equipedSpell;
        Sprite _spellImage;
        Coroutine _warningCoroutine;
        public SpellSlot(TiledObject data) : base("circle.png",1,1,addCollider:false)
        {
            var emptyMenuSprite = new Sprite(data.GetStringProperty("EmptyMenuSpriteFileName"), addCollider: false);
            _warningTimer = data.GetFloatProperty("WarningTimer",0.1f);

            AddChild(emptyMenuSprite);
            _emptyMenuSprite = emptyMenuSprite;
            emptyMenuSprite.SetCenterOrigin();
            emptyMenuSprite.SetXY(0,0);
            alpha = 0;
            _width = Mathf.Floor(data.Width);
            _height = Mathf.Floor(data.Height);
            //this.ResizePreservingAspectRatio(_width);
        }
        
        public void Init()
        {
            var level = ((MyGame)MyGame.main).CurrentLevel;
            _player = level.Player;
        }
        void StopRoutine()
        {
            _warningCoroutine?.LateDestroy();
            _warningCoroutine = null;
        }
        private void Update()
        {
            if (Input.GetKeyDown(Key.Q) && EquipedSpell != null)
            {
                if(!EquipedSpell.TryPerform(_player))    
                {
                    StopRoutine();
                    
                    _warningCoroutine = new Coroutine(ShowUnableUse());
                    AddChild(_warningCoroutine);
                }

            }
        }
        IEnumerator ShowUnableUse()
        {
            _spellImage.color = (uint)Color.Red.ToArgb();
            yield return new WaitForSeconds(_warningTimer);
            _spellImage.color = (uint)Color.White.ToArgb();
            _warningCoroutine = null;
        }
    }
}
