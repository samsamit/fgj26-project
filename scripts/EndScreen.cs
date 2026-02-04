using Godot;
using System;

public partial class EndScreen : Control
{

    public override void _EnterTree()
    {
        base._EnterTree();
        GlobalStateManager.Instance.EndGame();
    }


    public void OnBackToMenuPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    
    }
}

