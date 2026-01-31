using Godot;
using System.Collections.Generic;
using System;

public partial class GlobalStateManager : Node
{
	public HashSet<string> CompletedPuzzle = [];

	public Vector2 PlayerPosition = Vector2.Zero;
	public Vector2 MaskPosition = Vector2.Zero;

	public override void _Ready()
	{
		PuzzleCompleted += puzzleName => {
			GD.Print("hey");
			CompletedPuzzle.Add(puzzleName);
			};
	}

	[Signal]
	public delegate void PuzzleCompletedEventHandler(string puzzleName);
}
