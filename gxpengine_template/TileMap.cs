using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using GXPEngine.Core;                // GXPEngine contains the engine


namespace gxpengine_template
{
    public class TileMap
    {
        float _cellSize;
        int _rows;
        int _columns;
        Sprite[] Tiles;
        public TileMap(float cellSize, float canvasWidth, float canvasHeight, MyGame game)
        {
            int columns = Mathf.Floor(canvasWidth / cellSize);
            int rows = Mathf.Floor(canvasHeight / cellSize);
            _cellSize = cellSize;
            _rows = rows;
            _columns = columns;

            Tiles = new Sprite[columns * rows];
            int i = 0;
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _columns; x++) 
                {
                    var button = new EasyDraw((int)cellSize,(int)cellSize);
                    button.Clear(100, 100, 100, 128);
                    AddCanvas(i, button);
                    game.AddChild(button);
                    i++;
                }
                
            }
        }
        public void AddSprite(int pos, Sprite sprite)
        {
            Tiles[pos] = sprite;
            sprite.SetOrigin(sprite.width/2, sprite.height/2);
            var worldPos = GetPositionOfIndex(pos);
            sprite.SetXY(worldPos.x,worldPos.y);
        }
        public void AddCanvas(int pos, EasyDraw sprite)
        {
            Tiles[pos] = sprite;
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
            var worldPos = GetPositionOfIndex(pos);
            sprite.SetXY(worldPos.x, worldPos.y);
        }
        public Vector2 GetPositionOfIndex(int index)
        {
            var currRow = (index / _columns );
            return new Vector2( _cellSize * (index - currRow * _columns) + _cellSize/2, _cellSize * currRow + _cellSize/2) ; 
        }
    }
}
