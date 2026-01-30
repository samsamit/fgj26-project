using Godot;
using System;

public partial class Icon : Node2D
{
	[Export]
	public float Speed = 200.0f;

	private Vector2 direction = Vector2.Right;
	private float mouthOpen = 0.5f;
	private const float radius = 20.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Change direction based on input
		if (Input.IsActionJustPressed("ui_right"))
			direction = Vector2.Right;
		if (Input.IsActionJustPressed("ui_left"))
			direction = Vector2.Left;
		if (Input.IsActionJustPressed("ui_down"))
			direction = Vector2.Down;
		if (Input.IsActionJustPressed("ui_up"))
			direction = Vector2.Up;

		// Move continuously in current direction
		Position += direction * Speed * (float)delta;

		// Animate mouth
		mouthOpen = (Mathf.Sin(Time.GetTicksMsec() * 0.005f) + 1) * 0.5f; // 0 to 1

		// Set rotation to face direction
		Rotation = direction.Angle();

		// Redraw
		QueueRedraw();
	}

	public override void _Draw()
	{
		// Draw the Pac-Man body (yellow circle)
		DrawCircle(Vector2.Zero, radius, Colors.Yellow);

		// Draw the mouth (black triangle)
		float mouthWidth = radius * 0.6f * mouthOpen;
		Vector2 mouth1 = new Vector2(radius, -mouthWidth);
		Vector2 mouth2 = new Vector2(radius, mouthWidth);
		Vector2[] mouthPoints = { Vector2.Zero, mouth1, mouth2 };
		Color[] mouthColors = { Colors.Black, Colors.Black, Colors.Black };
		DrawPolygon(mouthPoints, mouthColors);
	}
}
