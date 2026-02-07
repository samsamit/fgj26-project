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
		GlobalStateManager.Instance.AvailableMasks.RegisterAfterChangeObserver(UpdateMusicAfterAddingNewMask);
		GlobalStateManager.Instance.PuzzleCompleted += ChangeMusicOnPuzzleCompletion;
	}

	public override void _ExitTree()
	{
		GlobalStateManager.Instance.PuzzleCompleted -= ChangeMusicOnPuzzleCompletion;
	}

	private void UpdateMusicAfterAddingNewMask(List<MaskEnum> masks)
	{
		if (masks.Contains(MaskEnum.XRay))
		{
			Stream = WithoutDrums;
		}
		else if (masks.Contains(MaskEnum.Strength))
		{
			Stream = Drums;
		}
		else if (masks.Contains(MaskEnum.Slow))
		{
			Stream = Drums;
		}
		else
		{
			Stream = WithoutDrums;
		}

		FadeOutInMusic();
	}

	private void ChangeMusicOnPuzzleCompletion(string puzzleId)
	{
		if (puzzleId == nameof(Puzzle5))
		{
			Stream = MainTheme;
			FadeOutInMusic();
		}
		else if (puzzleId == nameof(Puzzle4))
		{
			Stream = WithoutDrums;
			FadeOutInMusic();
		}
	}

	private void FadeOutInMusic()
	{
		// Fade out current music using tween and fade in the new one: short 1000ms crossfade;
		var tween = CreateTween();
		tween.TweenProperty(this, "volume_db", -80.0f, 1.0f)
			 .SetTrans(Tween.TransitionType.Linear)
			 .SetEase(Tween.EaseType.InOut);
		tween.TweenCallback(Callable.From(() => Play()));
		tween.TweenProperty(this, "volume_db", 0.0f, 1.0f)
			 .SetTrans(Tween.TransitionType.Linear)
			 .SetEase(Tween.EaseType.InOut);
	}

	public void OnFinished()
	{
		Play();
	}
}
