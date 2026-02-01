using Godot;
using System;

public partial class ProjectileLauncher : Node2D
{
    [Export(PropertyHint.Range, "0,360,0.1,radians_as_degrees")]
    public float Angle;

    [Export] public Node2D Turret;
    [Export] public PackedScene ProjectileBase;
    [Export] public double ProjectileSpeed;
    [Export] public double FireRate = 1;

    private double _nextFire;
    private AudioStreamPlayer2D _AudioStreamPlayerFire;

    public override void _Ready()
    {
        _AudioStreamPlayerFire =  GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2DFire");
        Turret.Rotation = Angle;
        _nextFire = new Random().NextDouble() * 1 / FireRate;
    }

    public override void _PhysicsProcess(double delta)
    {
        _nextFire -= delta;

        if (_nextFire <= 0)
        {
            Fire();
        }

    }

    private void Fire()
    {
        _AudioStreamPlayerFire.Play();
        var projectile = (Projectile)ProjectileBase.Instantiate();
        projectile.Rotation = Angle;
        projectile.Position += new Vector2(20, 0).Rotated(Angle);
        projectile.SpeedComponent.MovementSpeed = ProjectileSpeed;
        AddChild(projectile);
        _nextFire += 1 / FireRate;
    }
}