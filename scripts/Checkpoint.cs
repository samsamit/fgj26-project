using Godot;
using System;

public partial class Checkpoint : Area2D
{
	[Export]
	public string NameLabel { get; set; } = "Checkpoint";

	[Signal]
	public delegate void CheckpointReachedEventHandler(Vector2 position);

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		// Debug print global position
		GD.Print("Checkpoint ", NameLabel, " at ", GlobalTransform.Origin);
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			GD.Print("Player reached checkpoint: ", NameLabel);
			EmitSignal(SignalName.CheckpointReached, GlobalPosition);
		}
	}
}
