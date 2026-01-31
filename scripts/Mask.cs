using Godot;
using System;

public partial class Mask : CharacterBody2D
{
	public const float Speed = 200.0f;
	
	public override void _Ready()
	{
		GD.Print("Player script is active!");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		return;
		Vector2 velocity = Vector2.Zero;
		
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 direction = (mousePosition - GlobalPosition).Normalized();
			velocity = direction * Speed;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		GlobalPosition = GetGlobalMousePosition();
    }

}
