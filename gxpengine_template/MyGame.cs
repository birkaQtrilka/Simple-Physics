using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using GXPEngine;
using gxpengine_template;
using TiledMapParser;
using GXPEngine.Core;
using System.Reflection.Emit;
public interface IPrefab 
{
    GameObject Clone();
}
public class MyGame : Game 
{
    public event Action BeforeLevelReload;
    public Level CurrentLevel { get; private set; }
    public Dictionary<string, object> Data { get; private set; }
    public Dictionary<string, IPrefab> Prefabs { get; private set; }
    string _newLevelName;
    Vector2? _checkPoint;
    static void Main()
    {
        new MyGame().Start();
    }
    public MyGame() : base(800, 600, false, pPixelArt:true)     // Create a window that's 800x600 and NOT fullscreen
	{
        //MyGame.main.AddChild(new BeatManager("ubuga", 60, 0.2f));//put in data

        Prefabs = new Dictionary<string, IPrefab>();
        var loader = new TiledLoader("Prefabs.tmx", MyGame.main, false, autoInstance: true);
        loader.LoadObjectGroups();
        foreach (var obj in FindObjectsOfType<GameObject>())
            if(obj is IPrefab prefab)
            {
                RemoveChild(obj);
                Prefabs.Add(obj.name,prefab);
            }

        Data = new Dictionary<string, object>()
        {
            {"PlayerConfig1", new PlayerData(10, new SpellMaker(new List<Spell>())) },
            {"PortalSpell", new Spell_Portal(new int[] {1,1,1,1},new SpriteData("PortalSprite.png"), Prefabs["PortalRange_3"] as Portal)},
            {"UnlockSpell", new Spell_Unlock(new int[] {2,2,2,2}, new SpriteData("pixel-key.png"),new SpriteData("pixel-key.png",32,32,true), range: 32, 0.2f) }
        };
        LoadLevel("LVL3.tmx");
        //LoadMennuScene();
        

        OnAfterStep += LoadSceneIfNotNull;
	}
    void Update()
    {
        Console.WriteLine(currentFps);
        if (Input.GetKeyDown(Key.R))
        {
            LoadLevel(CurrentLevel.Name);
        }
    }
    //not public cuz haven't checked for bugs
    private void LoadMennuScene()
    {
        var menuLevel = new MenuLevel("StartMenu.tmx");
        CurrentLevel = menuLevel;
        menuLevel.Init();
        menuLevel.Start();
    }
    private void LoadSceneIfNotNull()
    {
        if (_newLevelName == null) return;
        DestroyAll();
        var level = new Level(_newLevelName);
        CurrentLevel = level;
        AddChild(level);
        level.Init();
        level.Start();

        if (_checkPoint.HasValue)
            level.Player.SetXY(_checkPoint.Value.x, _checkPoint.Value.y);
        _newLevelName = null;
    }

    public void SetCheckPoint(Vector2 pos)
    {
        _checkPoint = pos;
        CurrentLevel.Player.PlayerGameData.SpellMaker.CacheCurrentSpells();

    }

    public void LoadLevel(string levelName)
    {
        _newLevelName = levelName;
        _checkPoint = null;
    }

    public void ReloadLevel()
    {
        _newLevelName = CurrentLevel.Name;
        BeforeLevelReload?.Invoke();
    }

    void DestroyAll()
    {
        foreach (var child in GetChildren())
        {
            if(!(child is INonDestroyable))
            {
                child.Destroy();
            }
        }
    }

    protected override void OnDestroy()
    {
        OnAfterStep -= LoadSceneIfNotNull;

    }
}