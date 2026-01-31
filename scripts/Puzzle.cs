using Godot;

public abstract partial class Puzzle : Node
{
	public abstract string puzzleId { get; }
	public bool isActive;
	public abstract void CompletePuzzle();

	public override void _Ready()
	{
		InitializeConditionSignals();
	}

	private void InitializeConditionSignals()
	{
		foreach (var condition in Conditions)
		{
			condition.ConditionCompleted += OnConditionCompleted;
		}
	}

	private void OnConditionCompleted()
	{
		GD.Print("Checking puzzle completion conditions");
		foreach (var condition in Conditions)
		{
            if (condition == null) continue;
			if (!condition.IsCompleted) return;
		}
		GD.Print($"puzzle {puzzleId} all conditions completed");
		GD.Print($"puzzle {puzzleId} completed");
        GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PuzzleCompleted, puzzleId);
	}

    [Export] public CompletionCondition[] Conditions;



}
