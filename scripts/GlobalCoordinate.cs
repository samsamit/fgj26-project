using Godot;
using System;

public partial class GlobalCoordinate : Node2D
{
	[Export]
	public string NameLabel { get; set; } = "SpawnPoint";

	public override void _Ready()
	{
		// Debug print global position
		GD.Print("SpawnPoint ", NameLabel, " at ", GlobalTransform.Origin);
	}
}
