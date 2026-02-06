using Godot;
using System;

public partial class Projectile : Area2D
{
    [Export] public SpeedComponent SpeedComponent;
    [Export] public AnimatableBody2D Body;

    private VisibleOnScreenNotifier2D _screenNotifier;

    [Export]
    public float MaxTimeOutOfScreenSeconds = 2f;
    private float _timeOutOfScreen = 0f;

    public override void _Ready()
    {
        BodyEntered += OnPlayerHit;
        _screenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        _screenNotifier.ScreenExited += QueueFree;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnPlayerHit;
        _screenNotifier.ScreenExited -= QueueFree;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_screenNotifier.IsOnScreen())
        {
            _timeOutOfScreen = 0f;
        }
        else
        {
            _timeOutOfScreen += (float)delta;
            if (_timeOutOfScreen > MaxTimeOutOfScreenSeconds)
            {
                QueueFree();
                return;
            }
        }

        var movementVector = new Vector2((float)(SpeedComponent.CurrentSpeed * delta), 0).Rotated(Rotation);

        var collision = Body.MoveAndCollide(movementVector);
        // Sync parent Area2D position to where the Body moved, then reset Body to center
        GlobalPosition = Body.GlobalPosition;
        Body.Position = Vector2.Zero;
        
        if (collision is not null)
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
