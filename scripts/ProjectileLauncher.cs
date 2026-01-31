using Godot;
using System;

public partial class ProjectileLauncher : Node2D
{
    [Export(PropertyHint.Range, "0,360,0.1,radians_as_degrees")]
    public float Angle;

    [Export] public Node2D Turret;
    [Export] public PackedScene ProjectileBase;
    [Export] public double ProjectileSpeed;
    [Export] public double ProjectileLifeTime;
    [Export] public double FireRate;

    private double _nextFire;

    public override void _Ready()
    {
        Turret.Rotation = Angle;
        _nextFire = new Random().NextDouble() * FireRate;
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
        var projectile = (Projectile)ProjectileBase.Instantiate();
        projectile.Rotation = Angle;
        projectile.Position += new Vector2(16, 0).Rotated(Angle);
        projectile.Speed = ProjectileSpeed;
        projectile.LifeTime = ProjectileLifeTime;
        AddChild(projectile);
        _nextFire += FireRate;
    }
}