using Godot;
using System;

public partial class Projectile : Area2D
{
    [Export] public SpeedComponent SpeedComponent;

    public override void _Ready()
    {
        BodyEntered += OnPlayerHit;
        var screenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        screenNotifier.ScreenExited += QueueFree;
    }
    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2((float)(SpeedComponent.CurrentSpeed * delta), 0).Rotated(Rotation);
    }

    private void OnPlayerHit(Node2D body)
    {
        if (body is not Player) return;
        GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PlayerHit);
        QueueFree();
    }
}
