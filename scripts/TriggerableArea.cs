using Godot;

public abstract partial class TriggerableArea : Area2D
{
    public bool IsTriggered = false;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    protected virtual void OnAreaEntered(Area2D area)
    {
        if (area.Equals(GlobalStateManager.Instance.Mask?.ViewArea))
            return;

        IsTriggered = true;
    }

    protected virtual void OnAreaExited(Area2D area)
    {
        if (area.Equals(GlobalStateManager.Instance.Mask?.ViewArea))
            return;

        IsTriggered = false;
    }
}
