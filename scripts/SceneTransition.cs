using Godot;

public partial class SceneTransition : Area2D
{
    [Export(PropertyHint.File, "*.tscn")]
    public string TargetScene;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
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
}
