using Godot;
using System;

public partial class OptionsMenu : Control
{
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
	}
}
