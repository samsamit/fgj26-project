using Godot;
using System;

public partial class Goal : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnPlayerEntered;
	}

	public void OnPlayerEntered(Node body)
	{
		if (body is not Player) return;
		GD.Print("goal");
		EmitSignal(SignalName.PlayerEntered);
		BodyEntered -= OnPlayerEntered;
	}

	[Signal]
	public delegate void PlayerEnteredEventHandler();
}
