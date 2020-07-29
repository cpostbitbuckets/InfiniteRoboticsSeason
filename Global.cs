using Godot;
using System;

/// <summary>
/// The Global script manages switching scenes. 
/// It connects to signals to start new matches or what have you
/// </summary>
public class Global : Node
{
    private Node currentScene = null;
    private PackedScene gameMenuScene;
    private GameMenu inGameMenu;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PauseMode = PauseModeEnum.Process;
        // TODO: fix in game menu to work
        gameMenuScene = (PackedScene)ResourceLoader.Load("res://menus/GameMenu.tscn");
        inGameMenu = (GameMenu)gameMenuScene.Instance();

        var root = GetTree().Root;
        currentScene = root.GetChild(root.GetChildCount() - 1);

        Signals.QuitEvent += OnQuit;
        Signals.NewMatchEvent += OnNewMatch;
        Signals.GoToMainMenuEvent += OnMainMenu;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("pause"))
        {
            currentScene.GetTree().Paused = !currentScene.GetTree().Paused;
        }
    }

    private void OnMainMenu()
    {
        CallDeferred(nameof(DeferredGotoScene), "res://Main.tscn");
    }

    private void OnNewMatch()
    {
        CallDeferred(nameof(DeferredGotoScene), "res://matches/match.tscn");
    }

    private void OnQuit()
    {
        GetTree().Quit();
    }

    public void OnGotoScene(string path)
    {
        CallDeferred(nameof(DeferredGotoScene), path);
    }

    /// <summary>
    /// Deferred
    /// </summary>
    /// <param name="path"></param>
    private void DeferredGotoScene(string path)
    {

        // we are deferred so we can free the current scene
        currentScene.Free();

        // load the next scene
        PackedScene nextScene = (PackedScene)ResourceLoader.Load(path);

        // setup the next scene as a child of the root node
        currentScene = nextScene.Instance();
        GetTree().Root.AddChild(currentScene);
        // This is for compatibility with the SceneTree.change_scene() API.
        GetTree().CurrentScene = currentScene;
    }

}
