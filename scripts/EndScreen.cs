using Godot;
using System;

public partial class EndScreen : Control
{
    public void OnBackToMenuPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    
    }
}

