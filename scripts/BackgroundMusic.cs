using Godot;
using System;
using System.Collections.Generic;

public partial class BackgroundMusic : AudioStreamPlayer
{
	[Export]
	public AudioStream MainTheme;

	[Export]
	public AudioStream Drums;

	[Export]
	public AudioStream WithoutDrums;

	public override void _Ready()
	{
		GlobalStateManager.Instance.AvailableMasks.RegisterObserver(UpdateMusic);
	}

	private void UpdateMusic(List<MaskEnum> masks)
	{
		if (masks.Contains(MaskEnum.XRay))
		{
			Stream = MainTheme;
		}
		else if (masks.Contains(MaskEnum.Strength))
		{
			Stream = Drums;
		}
		else
		{
			Stream = WithoutDrums;
		}
		Play();
	}

	public void OnFinished()
	{
		Play();
	}
}
