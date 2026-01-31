using Godot;

public partial class PressurePlate : Area2D
{
	[Export] public int RequiredWeight = 1;

	private int _currentWeight = 0;
	private bool _isPressed = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (!body.IsInGroup("pressure_objects"))
			return;

		_currentWeight++;

		UpdateState();
	}

	private void OnBodyExited(Node2D body)
	{
		if (!body.IsInGroup("pressure_objects"))
			return;

		_currentWeight--;

		UpdateState();
	}

	private void UpdateState()
	{
		bool shouldBePressed = _currentWeight >= RequiredWeight;

		if (shouldBePressed == _isPressed)
			return;

		_isPressed = shouldBePressed;
		
		if (_isPressed)
		{
			Pressed();
		}
		else
		{
			Released();
		}
	}

	private void Pressed()
	{
		GD.Print("Plate pressed");
		// play animation, emit signal, open door, etc
	}

	private void Released()
	{
		GD.Print("Plate released");
		// reverse animation, close door, etc
	}
}
