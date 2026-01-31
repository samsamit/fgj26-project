using Godot;
using System;

public partial class Mask : CharacterBody2D
{
	public const float Speed = 200.0f;

	private const string MoveRight = "arrow_right";
	private const string MoveLeft = "arrow_left";
	private const string MoveBack = "arrow_back";
	private const string MoveForward = "arrow_forward";
	
	public override void _Ready()
	{
		GD.Print("Player script is active!");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Zero;
		direction.X = Input.GetActionStrength(MoveRight)
					  - Input.GetActionStrength(MoveLeft);
		
		direction.Y = Input.GetActionStrength(MoveBack)
					  - Input.GetActionStrength(MoveForward);
		
		Velocity = direction.Normalized() * Speed;
		MoveAndSlide();
	}
}
