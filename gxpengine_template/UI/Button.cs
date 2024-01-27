using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public abstract class Button : AnimationSprite, IUserInterface
    {
        public event Action OnButtonPress;
        public Button(string fileName, int c, int r, TiledObject data) : base(fileName, c, r, addCollider: false)
        {
            SetOrigin(width/2, height/2);
            
        }
        protected void Update()
        {

            //if(Input.GetMouseButtonDown(0))
            //{
            var mouseX = Input.mouseX; var mouseY = Input.mouseY;
            var halfWidth = width / 2; var halfHeight = height / 2;
            if(mouseX > x - halfWidth && mouseX < x + halfWidth && mouseY > y - halfHeight && mouseY < y + halfHeight)
            {
                color = (uint)Color.Green.ToArgb();
                if(Input.GetMouseButtonDown(0)) 
                {
                    OnButtonPress?.Invoke();
                }
            }
            else
            {
                color = (uint)Color.White.ToArgb();

            }
            //}
        }

        public void Init() { }
    }
}
