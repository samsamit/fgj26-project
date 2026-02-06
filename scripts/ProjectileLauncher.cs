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

    [Export] public CompletionCondition[] ActivationConditions;

    private static Random _random = new Random();

    private double _nextFire;
    private AudioStreamPlayer2D _AudioStreamPlayerFire;

    private bool isActive = true;

    public override void _EnterTree()
    {
        if (ActivationConditions != null)
        {
            bool allCompleted = true;
            foreach (var condition in ActivationConditions)
            {
                condition.ConditionChanged += handleActivationConditionChanged;
                allCompleted &= condition.IsCompleted;
            }
            isActive = allCompleted;
        }
        if (!isActive)
        {
            GD.Print($"Projectile launcher {Name} inactive");
            Visible = false;
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (ActivationConditions != null)
        {
            foreach (var condition in ActivationConditions)
            {
                condition.ConditionChanged -= handleActivationConditionChanged;
            }
        }
    }

    private void handleActivationConditionChanged(bool isCompleted)
    {
        if (!isCompleted) return;
        foreach (var condition in ActivationConditions)
        {
            if (!condition.IsCompleted) return;
        }
        isActive = true;
        if (isActive)
        {
            Visible = true;
        }
    }

    public override void _Ready()
    {
        _AudioStreamPlayerFire =  GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2DFire");
        Turret.Rotation = Angle;
        _nextFire = _random.NextDouble() * 1 / FireRate;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!isActive) return;
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
        projectile.Position += new Vector2(24, 0).Rotated(Angle);
        projectile.SpeedComponent.MovementSpeed = ProjectileSpeed;
        AddChild(projectile);
        _nextFire += _random.NextDouble() * 1 / FireRate;
    }
}