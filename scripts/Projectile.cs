using Godot;
using System;

public partial class Projectile : Area2D
{
    public double Speed;
    public double LifeTime;

    public override void _Ready()
    {
        BodyEntered += OnPlayerHit;
    }
    public override void _PhysicsProcess(double delta)
    {
        LifeTime -= delta;
        if (LifeTime <= 0) QueueFree();

        Position += new Vector2((float)(Speed * delta), 0).Rotated(Rotation);
    }

    private void OnPlayerHit(Node2D body)
    {
        if (body is not Player) return;
        GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PlayerHit);
        QueueFree();
    }
}
