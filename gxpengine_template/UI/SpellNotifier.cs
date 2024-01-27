using GXPEngine;
using System;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template
{
    public class SpellNotifier : AnimationSprite, IUserInterface
    {
        public SpellNotifier(TiledObject data) : base("circle.png",1,1,addCollider:false)
        {
            if (Instance != null && Instance != this)
            {
                Destroy();
                return;
            }
            else
            {
                Instance = this;
            }
            _notifyTimer = data.GetFloatProperty("NotifyTimer");
            _textMesh = new TextMesh("", 300, 300, CenterMode.Min);
            _textMesh.SetOrigin(0, _textMesh.Height/2);
            AddChild( _textMesh );
            visible = false;
            alpha = 0;
            _size = Mathf.Floor(data.Width);
        }
        readonly TextMesh _textMesh;
        readonly float _notifyTimer;
        readonly int _size;
        Sprite _menuImageDisplayer;
        Coroutine _routine;
        public static SpellNotifier Instance { get; private set; }
        public void Init()
        {
        }

        public void Notify(Spell spell)
        {
            var combination = spell.Combination;
            _textMesh.Text = $"New! {spell.GetType().ToString().Substring(5)} Spell!! \nuse combination {combination[0]}, {combination[1]}, {combination[2]}, {combination[3]}";
            //I have to copy it
            _menuImageDisplayer = spell.MenuImage.CreateSprite(_size,_size,true);
            AddChild(_menuImageDisplayer );
            _menuImageDisplayer.SetCenterOrigin();
            _menuImageDisplayer.SetXY(-32, 0);
            _menuImageDisplayer.visible = true;
            visible = true;

            StopRoutine();
            _routine = new Coroutine(DestroyAfterCooldown());
            AddChild( _routine );
        }

        IEnumerator DestroyAfterCooldown()
        {
            yield return new WaitForSeconds(_notifyTimer);
            _menuImageDisplayer?.LateDestroy();
            visible = false;
            StopRoutine();
        }
        void StopRoutine()
        {
            _routine?.LateDestroy();
            _routine = null;
        }
        protected override void OnDestroy()
        {
            if(Instance != null && Instance == this)
            {
                Instance = null;
            }
        }
    }
}
