using Godot;

public partial class SceneTransition : Area2D
{
    [Export(PropertyHint.File, "*.tscn")]
    public string TargetScene;

    [Export] 
    public Puzzle puzzleToCompleteToActivate;

    private bool isActive;

    public override void _Ready()
    {
        isActive = puzzleToCompleteToActivate == null || GlobalStateManager.Instance.CompletedPuzzles.Contains(puzzleToCompleteToActivate.puzzleId);
        if (puzzleToCompleteToActivate == null || isActive)
        {
            BodyEntered += OnBodyEntered;
        } else if (!isActive)
        {
            GlobalStateManager.Instance.PuzzleCompleted += OnPuzzleCompleted;
        }
    }

    public override void _ExitTree()
    {
        if (!isActive)
        {
            GlobalStateManager.Instance.PuzzleCompleted -= OnPuzzleCompleted;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player) return;
        
        if (string.IsNullOrEmpty(TargetScene))
        {
            GD.PrintErr("SceneTransition: TargetScene is not set!");
            return;
        }

        GD.Print($"Transitioning to scene: {TargetScene}");
        GetTree().CallDeferred("change_scene_to_file", TargetScene);
    }

    private void OnPuzzleCompleted(string puzzleId)
    {
        if (puzzleToCompleteToActivate != null && puzzleId == puzzleToCompleteToActivate.puzzleId)
        {
            isActive = true;
            GlobalStateManager.Instance.PuzzleCompleted -= OnPuzzleCompleted;
            BodyEntered += OnBodyEntered;
        }
    }
}
