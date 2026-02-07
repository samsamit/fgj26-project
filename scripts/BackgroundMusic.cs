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
		if (masks == null || masks.Count == 0)
		{
			return;
		}
		MaskEnum lastAddedMask = masks[masks.Count - 1];

		switch (lastAddedMask)
		{
			case MaskEnum.XRay:
				Stream = WithoutDrums;
				break;
			case MaskEnum.Strength:
				Stream = Drums;
				break;
			case MaskEnum.Slow:
				Stream = Drums;
				break;
			default:
				Stream = WithoutDrums;
				break;
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
