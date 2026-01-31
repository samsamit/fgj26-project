using Godot;
using System.Collections.Generic;
using System;

public partial class GlobalStateManager : Node
{
	public HashSet<string> CompletedPuzzle = [];

	public Vector2 PlayerPosition = Vector2.Zero;
	public Vector2 MaskPosition = Vector2.Zero;
	public Observable<MaskEnum> CurrentMask = new(MaskEnum.Round);

	public static GlobalStateManager Instance;

	public override void _Ready()
	{
		PuzzleCompleted += puzzleName =>
		{
			GD.Print("hey");
			CompletedPuzzle.Add(puzzleName);
		};
		Instance = this;
	}

	[Signal]
	public delegate void PuzzleCompletedEventHandler(string puzzleName);

	public static Action<string> onPuzzleCompleted;
	public static void OnPuzzleCompleted()
	{

	}

	public void ChangeMask(MaskEnum maskEnum)
	{
		CurrentMask.Set(maskEnum);
	}
}
