using Godot;
using System;

public partial class Mask : Node2D
{
	/// <summary>
	/// Speed at which the mask moves towards the mouse position (pixels per second).
	/// </summary>
	[Export]
	public float FollowSpeed { get; set; } = 100.0f;

	private GlobalStateManager _stateManager;

	public override void _Ready()
	{
		GD.Print("Mask script is active!");
		_stateManager = GetNode<GlobalStateManager>("/root/World");
	}

	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			
		// Move towards the mouse position at the configured speed
		GlobalPosition = GlobalPosition.MoveToward(mousePosition, FollowSpeed * (float)delta);
		
		_stateManager.MaskPosition = GlobalPosition;
		}
	}
	
	private void Area2DBodyEntered(Node body)
	{
		GD.Print("Body entered: " + body.Name);
	}

	private void Area2DBodyExited(Node body)
	{
		GD.Print("Body exited: " + body.Name);
	}
}
