using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections;
using System.Linq;

namespace gxpengine_template
{
    public class Attacker : GameObject
    {
        public event Action Attacked;

        readonly int _damage;
        readonly float _critMult;
        readonly MovableObject _parent;
        readonly Sprite _attackImg;
        readonly Sprite _critImg;
        readonly int _coolDown;
        readonly float _visualStayTime;

        Sprite _selectedImage;
        int _coolDownResetTime;

        Coroutine _currCoroutine;
        public Attacker(int damage, float critMult, MovableObject parent, Sprite attackImg, Sprite critImg, float visualStayTime, int coolDown)
        {
            _damage = damage;
            _critMult = critMult;
            _parent = parent;
            _attackImg = attackImg;
            _critImg = critImg;
            _visualStayTime = visualStayTime;
            _coolDown = coolDown;

            _attackImg.visible = false;
            _attackImg.collider.isTrigger = true;

            _critImg.visible = false;
            _critImg.collider.isTrigger = true;

        }

        public void Perform(bool oppositeWay = false)
        {
            if (!CheckIfCanAttack()) return;

            int damage = _damage;

            if (_selectedImage != null)
            {
                _selectedImage.visible = false;
                RemoveChild(_selectedImage);
            }

            if (BeatManager.Instance.OnBeat)
            {
                _selectedImage = _critImg;
                damage = Mathf.Floor(damage * _critMult);
            }
            else
                _selectedImage = _attackImg;

            var toRight = oppositeWay ? !_parent.IsFacingRight : _parent.IsFacingRight;
            _selectedImage.Mirror(!toRight, false);
            Vector2 pos = toRight ?
                new Vector2(_parent.x + _parent.width / 2 + _selectedImage.width / 2, _parent.y) :
                new Vector2(_parent.x - _parent.width / 2 - _selectedImage.width / 2, _parent.y);

            MyUtils.MyGame.CurrentLevel.AddChild(_selectedImage);
            _selectedImage.visible = true;
            _selectedImage.SetXY(pos.x, pos.y);

            foreach (var victim in _selectedImage.GetCollisions().OfType<IHealthHolder>())//maybe add a filter for who to hit
                if (victim != _parent)
                    victim.Health -= damage;

            Attacked?.Invoke();
            _coolDownResetTime = Time.time;

            _currCoroutine?.Destroy();
            _currCoroutine = new Coroutine(DisableVisualAfterTime());
            game.AddChild(_currCoroutine);
        }
        IEnumerator DisableVisualAfterTime()
        {
            yield return new WaitForSeconds(_visualStayTime);

            if (_selectedImage != null) _selectedImage.visible = false;

        }
        
        bool CheckIfCanAttack()
        {
            int currTime = Time.time;

            if (currTime - _coolDownResetTime > _coolDown && _parent.CurrentState != State.Fall)
            {
                _coolDownResetTime = currTime;
                return true;
            }
            return false;
        }
    }
}
