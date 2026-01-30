using Godot;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;

	private const string MoveRight = "move_right";
	private const string MoveLeft = "move_left";
	private const string MoveBack = "move_back";
	private const string MoveForward = "move_forward";
	
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
		
		if (direction != Vector2.Zero)
		GD.Print("Player moving: ", direction);

		Velocity = direction.Normalized() * Speed;
		MoveAndSlide();
	}
}
