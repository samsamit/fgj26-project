using Godot;

namespace FGJ26LPR.scenes.CompletionConditions;

public partial class PressurePlateUpCondition : CompletionCondition
{
	[Export] private PressurePlate _pressurePlate;

	private bool _isPressed = false;
	
	public override void _Ready()
	{
		_pressurePlate.Pressed += OnPressed;
		_pressurePlate.Released += OnReleased;
	}

	private void OnPressed()
	{
		_isPressed = true;
		MarkUncompleted();
	}
	
	private void OnReleased()
	{
		_isPressed = false;
		MarkCompleted();
	}
}
