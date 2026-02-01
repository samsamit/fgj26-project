using Godot;
using System;

public partial class MainMenuScript : Control
{
    private TextureRect _staticTexture;
    private TextureRect _animatedTexture;
    private Timer _timerToStartFlicker;
    private Timer _timerToStopFlicker;
    [Export] public float AnimationCreditsSpeed = 10.0f;
    [Export] public float DefaultFlickerSpeed = 8.0f;
    [Export] public int MinTimeBetweenFlickers = 5;
    [Export] public int MaxTimeBetweenFlickers = 10;

    public override void _Ready()
    {
        _staticTexture = GetNode<TextureRect>("StaticTexture");
        _animatedTexture = GetNode<TextureRect>("AnimatedTexture");
        _timerToStartFlicker = GetNode<Timer>("FlickerStartTimer");
        _timerToStopFlicker = GetNode<Timer>("FlickerTimeout");
        _timerToStartFlicker.Start();
        _animatedTexture.Visible = false;

    }

    public override void _Process(double delta)
    {


    }



    public void OnButtonStartPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/BaseScene.tscn");
        //GD.Print("Start pressed");
    }

    public void OnButtonOptionsPressed()
    {
        GD.Print("Options pressed");
    }

    public void OnButtonCreditsPressed()
    {

        GD.Print("Credits pressed");
    }

    public void OnButtonExitPressed()
    {
        GetTree().Quit();
        //GD.Print("Exit pressed");
    }

    public void OnTimerTimeout()
    {
        _timerToStartFlicker.Stop();
        _animatedTexture.Visible = true;
        _staticTexture.Visible = false;
        _timerToStopFlicker.Start();
    }

    public void OnFlickerComplete()
    {
        _timerToStopFlicker.Stop();
        _animatedTexture.Visible = false;
        _staticTexture.Visible = true;
        _timerToStartFlicker.WaitTime = new Random().Next(MinTimeBetweenFlickers, MaxTimeBetweenFlickers);
        GD.Print(_timerToStartFlicker.WaitTime);
        _timerToStartFlicker.Start();
    }

    public void OnCreditsEnded()
    {
        return;
    }
    public void OnCreditsChanged()
    {
        return;
    }
}
