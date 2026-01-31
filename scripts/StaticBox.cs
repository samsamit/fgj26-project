using Godot;
using System;

public partial class StaticBox : StaticBody2D
{
	private const string PressureObjects = "pressure_objects";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup(PressureObjects);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
