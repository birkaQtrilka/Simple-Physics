using System;
using GXPEngine;
using TiledMapParser;

namespace gxpengine_template
{
    public class LoadLevelButton : Button
    {
        readonly string _levelName;
        public LoadLevelButton(string fileName, int c, int r, TiledObject data) : base(fileName, c, r, data)
        {
            _levelName = data.GetStringProperty("LevelName");
            OnButtonPress += LoadLevel;
        }

        private void LoadLevel()
        {
            MyUtils.MyGame.LoadLevel(_levelName);
        }

        protected override void OnDestroy()
        {
            OnButtonPress -= LoadLevel;

        }
    }
}
