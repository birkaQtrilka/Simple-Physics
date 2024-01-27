using System;
using System.Linq;
using GXPEngine;
using TiledMapParser;

namespace gxpengine_template
{
    public class Level : GameObject
    {
        
        public event Action<Level> LevelStarted;
        public Player Player => _player;
        public PortalUndoHandler PortalUndoHandler { get; private set; }
        public string Name { get; }

        Player _player;
        public Level(string fileName) 
        {
            Name = fileName;
            
        }
        public virtual void Init()
        {
            var loader = new TiledLoader(Name, MyGame.main,addColliders: false, autoInstance: true);

            //bg
            loader.rootObject = this;
            loader.LoadImageLayers();

            var sky = FindObjectOfType<Sprite>();

            sky.scaleY = 5;
            for (int i = 1; i < 6; i++)
            {
                var repeat = new Sprite(sky.texture, false);
                repeat.SetCenterOrigin();
                AddChild(repeat);
                repeat.SetXY(sky.x + sky.width * i, sky.y);
                repeat.scaleY = 5;
            }
            //Console.WriteLine(loader.map.ObjectGroups);
            //tiles
            loader.addColliders = false;
            loader.LoadTileLayers();

            //ground
            loader.rootObject = this;
            loader.addColliders = true;
            loader.LoadObjectGroups(6);
            loader.LoadObjectGroups(5);
            loader.LoadObjectGroups(4);

            //managers
            loader.rootObject = MyGame.main;
            loader.addColliders = false;
            
            loader.LoadObjectGroups(3);
            PortalUndoHandler = new PortalUndoHandler();
            AddChild(PortalUndoHandler);

            //level objects
            loader.rootObject = this;
            loader.addColliders = true;
            loader.LoadObjectGroups(2);

            //ui
            loader.addColliders = false;
            loader.rootObject = game;
            loader.LoadObjectGroups(1);

            //worldText
            loader.rootObject = this;
            loader.LoadObjectGroups(0);

            _player = FindObjectOfType<Player>();
        }
        public void Start()
        {
            
            LevelStarted?.Invoke(this);
            var UIElements = MyGame.main.GetChildren().OfType<IUserInterface>();

            foreach (var e in UIElements)
                e.Init();
            
        }

        void Update()
        {
            if (_player == null) return;
            Scrolling();
            
        }
        void Scrolling()
        {
            int boundary = 380;
            int rightBoundary = 380;


            if (_player.x + x < boundary)
            {
                x = boundary - _player.x;
            }
            if(_player.x + x > game.width - rightBoundary)
            {
                x = game.width - rightBoundary - _player.x;
            }
            boundary -= 100;
            rightBoundary -= 100;
            if (_player.y + y < boundary)
            {
                y = boundary - _player.y;
            }
            if(_player.y + y > game.height - rightBoundary)
            {
                y = game.height - rightBoundary - _player.y;
            }
        }
        #region custom loader
        //void SpawnTiles(Map levelData)
        //{
        //    if (levelData.Layers == null || levelData.Layers.Length == 0) return;

        //    Layer mainLayer = levelData.Layers[0];

        //    short[,] tileNumbers = mainLayer.GetTileArray();

        //    for (int row = 0; row < mainLayer.Height; row++)
        //        for (int col = 0; col < mainLayer.Width; col++)
        //        { 
        //            int tileNum = tileNumbers[col, row];

        //            TileSet tiles = levelData.GetTileSet(tileNum);

        //            if (tileNum > 0)
        //            {
        //                var tile = new CollisionTile(tiles.Image.FileName, tiles.Columns,tiles.Rows);//why not use normal sprite?
        //                tile.SetFrame(tileNum - tiles.FirstGId);
        //                tile.x = col * tile.width;
        //                tile.y = row * tile.height;
        //                AddChild(tile);
        //            }
        //        }
            
        //}

        //void SpawnObjects(Map levelData)
        //{ 
        //    if (levelData.ObjectGroups == null || levelData.ObjectGroups.Length == 0) return;

        //    var objectGroup = levelData.ObjectGroups[0];
            
        //    if (objectGroup.Objects == null || objectGroup.Objects.Length == 0) return;

        //    foreach (var obj in objectGroup.Objects)
        //    {
        //        Sprite newObj = null;
        //        switch (obj.Type)
        //        {
        //            case "Player":
        //                player = new Player();
        //                newObj = player;
        //            break;
                        
        //        }
        //        if (newObj != null)
        //        {
        //            newObj.x = obj.X + newObj.width / 2;
        //            newObj.y = obj.Y - newObj.height / 2;
        //            AddChild(newObj);
        //        }
        //    }
        //}
        #endregion
    }
}
