using Godot;
using System;
using System.Collections.Generic;

public partial class MainMenuScript : Control
{
	private TextureRect _staticTexture;
	private TextureRect _animatedTexture;
	private Timer _timerToStartFlicker;
	private Timer _timerToStopFlicker;
    private bool _creditsPlaying;
    private int _creditId;
    private BoxContainer _credits1;
    private BoxContainer _credits2;
    private BoxContainer _credits3;
    private BoxContainer _credits4;
    private BoxContainer _buttonContainer;
	[Export] public float AnimationCreditsSpeed = 10.0f;
	[Export] public float DefaultFlickerSpeed = 8.0f;
	[Export] public int MinTimeBetweenFlickers = 5;
	[Export] public int MaxTimeBetweenFlickers = 10;
    


	private PackedScene OptionsScene;

    private AudioStreamPlayer _AudioStreamPlayerLightFlicker;

	public override void _Ready()
	{
		_staticTexture = GetNode<TextureRect>("StaticTexture");
		_animatedTexture = GetNode<TextureRect>("AnimatedTexture");
		_timerToStartFlicker = GetNode<Timer>("FlickerStartTimer");
		_timerToStopFlicker = GetNode<Timer>("FlickerTimeout");
        _AudioStreamPlayerLightFlicker = GetNode<AudioStreamPlayer>("AudioStreamPlayerLightFlicker");
        _credits1 = GetNode<BoxContainer>("Credits1");
        _credits2 = GetNode<BoxContainer>("Credits2");
        _credits3 = GetNode<BoxContainer>("Credits3");
        _credits4 = GetNode<BoxContainer>("Credits4");
        _buttonContainer = GetNode<BoxContainer>("ButtonContainer");
		_timerToStartFlicker.Start();
		_animatedTexture.Visible = false;
		OptionsScene = ResourceLoader.Load<PackedScene>("res://scenes/OptionsMenu.tscn");
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
		var newNode = OptionsScene.Instantiate();
		AddChild(newNode);
	}

	public void OnButtonCreditsPressed()
	{
        _creditId = 1;
        _timerToStopFlicker.Stop();
        _timerToStartFlicker.Stop();
        _creditsPlaying = true;
        _buttonContainer.Visible = false;

        _timerToStartFlicker.WaitTime = 0;
        _timerToStartFlicker.Start();
		GD.Print("Credits pressed");
	}

	public void OnButtonExitPressed()
	{
		GetTree().Quit();
		//GD.Print("Exit pressed");
	}

	public void OnTimerTimeout()
	{
        if (_creditsPlaying)
        {
            _credits1.Visible = false;
            _credits2.Visible = false;
            _credits3.Visible = false;
            _credits4.Visible = false;
        }

		_timerToStartFlicker.Stop();
        _AudioStreamPlayerLightFlicker.Play();
		_animatedTexture.Visible = true;
		_staticTexture.Visible = false;
		_timerToStopFlicker.Start();
	}

	public void OnFlickerComplete()
	{
		_timerToStopFlicker.Stop();
		_animatedTexture.Visible = false;
		_staticTexture.Visible = true;
        if (_creditsPlaying)
        {
            switch (_creditId)
            {
                case 1:
                    _credits1.Visible = true;
                    _creditId++;
                    break;
                case 2:
                    _credits2.Visible = true;
                    _creditId++;
                    
                    break;
                case 3:
                    _credits3.Visible = true;
                    _creditId++;
                    break;
                case 4:
                    _credits4.Visible = true;
                    _creditId++;
                    break;
                case 5:
                    _creditsPlaying = false;
                    _buttonContainer.Visible = true;
                    _timerToStartFlicker.WaitTime = new Random().Next(MinTimeBetweenFlickers, MaxTimeBetweenFlickers);
                    GD.Print(_timerToStartFlicker.WaitTime);
                    _timerToStartFlicker.Start();

                    break;
            }



            _timerToStartFlicker.WaitTime = 2;
            _timerToStartFlicker.Start();
        }
        else
        {
		    _timerToStartFlicker.WaitTime = new Random().Next(MinTimeBetweenFlickers, MaxTimeBetweenFlickers);
		    GD.Print(_timerToStartFlicker.WaitTime);
		    _timerToStartFlicker.Start();
        }
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
