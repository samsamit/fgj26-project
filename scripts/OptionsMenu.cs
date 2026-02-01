using Godot;
using System;

public partial class OptionsMenu : Control
{
	private VBoxContainer _mainMenuButtons;

	private Slider musicSlider, sfxSlider;

	public override void _Ready()
	{
		_mainMenuButtons = GetNode<VBoxContainer>("../ButtonContainer");
		_mainMenuButtons.Hide();

		musicSlider = GetNode<Slider>("BackgroundMusic");
		sfxSlider = GetNode<Slider>("SFX");

		musicSlider.SetValue(AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Music")));
		sfxSlider.SetValue(AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Effects")));
	}
	public void OnMusicVolumeChanged(float value)
	{
		var music = AudioServer.GetBusIndex("Music");
		AudioServer.SetBusVolumeDb(music, (float)Mathf.LinearToDb(value));
	}

	public void OnSFXVolumeChanged(float value)
	{
		var sfx = AudioServer.GetBusIndex("Effects");
		AudioServer.SetBusVolumeDb(sfx, (float)Mathf.LinearToDb(value));
	}

	public void OnBackButton()
	{
		QueueFree();
		_mainMenuButtons.Show();
	}
}
