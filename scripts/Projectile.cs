using Godot;
using System;

public partial class Projectile : Area2D
{
    [Export] public SpeedComponent SpeedComponent;
    [Export] public AnimatableBody2D Body;

    private VisibleOnScreenNotifier2D _screenNotifier;

    public override void _Ready()
    {
        BodyEntered += OnPlayerHit;
        _screenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        _screenNotifier.ScreenExited += QueueFree;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!_screenNotifier.IsOnScreen()) QueueFree();
        Position += new Vector2((float)(SpeedComponent.CurrentSpeed * delta), 0).Rotated(Rotation);

        if (Body.MoveAndCollide(Vector2.Zero) is not null)
        {
            QueueFree();
        }
    }

    private void OnPlayerHit(Node2D body)
    {
        if (body is not Player) return;
        GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PlayerHit);
        QueueFree();
    }
}
