using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class Spell_Portal : Spell
    {
        readonly Portal _portalPrefab;
        public Spell_Portal(int[] combination, SpriteData menuImageData, Portal portalPrefab) : base(combination, menuImageData)
        {
            _portalPrefab = portalPrefab;
        }
        GameObject[] SendPseudoRay(Player player, float range)
        {

            var tempCol = new Sprite("square.png", true);//basically creating a ray (idk how else to do it in gxp)
            tempCol.collider.isTrigger = true;
            player.AddChild(tempCol);
            tempCol.SetOrigin(0, tempCol.height / 2);
            if (player.IsFacingRight)
                tempCol.SetXY(player.width, 0);
            else
                tempCol.SetXY(-player.height - range, 0);
            tempCol.width = Mathf.Floor(range);
            tempCol.height = 4;
            tempCol.LateDestroy();

            return tempCol.GetCollisions();
        }
        public override bool TryPerform(Player player)
        {
            if (player.CurrentState == State.Walk) return false;

            bool portalAlreadyPlaced = SendPseudoRay(player, _portalPrefab.EnterPortal.width / 2).Any(c => c.name == "enterPortal");
            var collisions = SendPseudoRay(player, _portalPrefab.Range);
            bool isBlocked = collisions.Any(c => c.collider.isTrigger == false && !(c is Player));//can pass through enemy

            if (isBlocked || portalAlreadyPlaced) return false;
        
            var portal = _portalPrefab.Clone() as Portal;
            var pos = player.IsFacingRight ? 
                new Vector2(player.x + player.width / 2, player.y) 
                :
                new Vector2(player.x - player.width / 2, player.y);

            //snap to grid
            var sign = pos.y < 0 ? -1 : 1;
            var snapedPos = new Vector2(pos.x, (Mathf.Floor(Mathf.Abs(pos.y / 32f)) + 0.5f) * sign * 32 );
            portal.SetXY(snapedPos.x,snapedPos.y);

            portal.Mirror(!player.IsFacingRight);

            MyUtils.MyGame.CurrentLevel.AddChild(portal);

            return true;
        }
    }
}
