using Godot;

public partial class AreaReachedCondition : CompletionCondition
{
	[Export] private Goal goalArea;

	public override void _Ready()
	{
		goalArea.PlayerEntered += OnGoalReached;
	}

	private void OnGoalReached()
	{
		MarkCompleted();
	}
}
