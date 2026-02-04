using Godot;
using System;

public partial class EndScreen : Control
{
    private AudioStreamPlayer _audioPlayer;

    public override void _EnterTree()
    {
        base._EnterTree();
        GlobalStateManager.Instance.EndGame();
    }

    public override void _Ready()
    {
        _audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _audioPlayer.Play(111.0f);
    }

    public void OnBackToMenuPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    
    }
}

