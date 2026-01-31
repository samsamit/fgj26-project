using Godot;
using System;

public partial class Goal : Area2D 
{
	private string puzzleId;
	public override void _Ready()
	{
		Node parent = GetParent();
		if (parent is not Puzzle)
		{
			GD.PrintErr("Goal must be a child of a Puzzle");
		}
		puzzleId = ((Puzzle)parent).puzzleId;
		BodyEntered += OnPlayerEntered;
	}

	public void OnPlayerEntered(Node body)
	{
		if (body is not Player) return;
		GD.Print("goal");
		GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PuzzleCompleted, puzzleId);
		BodyEntered -= OnPlayerEntered;
	}
}
