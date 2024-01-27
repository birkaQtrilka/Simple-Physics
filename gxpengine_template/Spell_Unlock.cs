using GXPEngine.Core;
using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace gxpengine_template
{
    public class Spell_Unlock : Spell
    {
        readonly SpriteData _worldImgData;
        readonly int _range;
        readonly float _visualStayTime;
        public Spell_Unlock(int[] combination, SpriteData menuImgData, SpriteData worldImgData, int range, float visualStayTime) : base(combination, menuImgData)
        {
            _worldImgData = worldImgData;
            _range = range;
            _visualStayTime = visualStayTime;
        }
        GameObject[] SendPseudoRay(Player player)
        {
            var tempCol = new Sprite("square.png", true);//basically creating a ray (idk how else to do it in gxp)
            tempCol.collider.isTrigger = true;
            player.AddChild(tempCol);
            tempCol.SetOrigin(0, tempCol.height / 2);
            if (player.IsFacingRight)
                tempCol.SetXY(player.width, 0);
            else
                tempCol.SetXY(-player.height - _range, 0);
            tempCol.width = _range;
            tempCol.height = 4;
            tempCol.LateDestroy();

            return tempCol.GetCollisions();
        }
        public override bool TryPerform(Player player)
        {
            if (player.CurrentState == State.Walk) return false;
            Vector2 playerExtents = new Vector2(player.width / 2, player.height / 2);

            if (!(SendPseudoRay(player).FirstOrDefault(c => c is IUnlockable) is IUnlockable unlockable)) return false;

            Sprite unlockImg = _worldImgData.CreateSprite();
            unlockImg.SetOrigin(0, unlockImg.height / 2);
            Vector2 pos;

            if (player.IsFacingRight)
                pos = new Vector2(player.x + playerExtents.x, player.y);
            else
            {
                unlockImg.Mirror(true,false);
                pos = new Vector2(player.x - playerExtents.x - unlockImg.width, player.y);

            }

            var level = MyUtils.MyGame.CurrentLevel;
            level.AddChild(unlockImg);
            level.AddChild(new Coroutine(ImageBehaviour(unlockImg)));
            unlockable.Unlock();

            return true;
        }
        IEnumerator ImageBehaviour(Sprite img)
        {
            //maybe play animation
            yield return new WaitForSeconds(_visualStayTime);
            img.LateDestroy();
        }
    }
}
