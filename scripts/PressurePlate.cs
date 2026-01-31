using Godot;

public partial class PressurePlate : Area2D
{
	[Export] public int RequiredWeight = 1;

	private int _currentWeight = 0;
	private bool _isPressed = false;

	private Texture2D  _spriteUnpressed = GD.Load<Texture2D>("res://assets/painelaattavapaa.png");
	private Texture2D _spritePressed = GD.Load<Texture2D>("res://assets/painelaattapainettu.png");
	private Sprite2D _sprite; 
		
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_sprite.Texture = _spriteUnpressed;
		
	}

	private void OnBodyEntered(Node2D body)
	{
		if (!body.IsInGroup("pressure_objects"))
		{
			return;	
		}

		_currentWeight++;
		_sprite.Texture = _spritePressed;

		UpdateState();
	}

	private void OnBodyExited(Node2D body)
	{
		if (!body.IsInGroup("pressure_objects"))
		{
			return;	
		}

		_currentWeight--;
		_sprite.Texture = _spriteUnpressed;
		
		UpdateState();
	}

	private void UpdateState()
	{
		bool shouldBePressed = _currentWeight >= RequiredWeight;

		if (shouldBePressed == _isPressed)
		{
			return;	
		}

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
