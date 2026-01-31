using Godot;

public partial class HiddenBridge : TriggerableArea
{
    public override void _Ready()
    {
        base._Ready();
        BodyEntered += DisableCollision;
        BodyExited += EnableCollision;
    }

    private void DisableCollision(Node body)
    {
        if (!IsTriggered)
            return;

        if (body is CharacterBody2D characterBody)
        {
            characterBody.SetCollisionMaskValue((int)CollisionLayers.Water, false);
        }
    }

    private void EnableCollision(Node body)
    {
        if (body is CharacterBody2D characterBody)
        {
            characterBody.SetCollisionMaskValue((int)CollisionLayers.Water, true);
        }
    }

    protected override void OnAreaEntered(Area2D area)
    {
        base.OnAreaEntered(area);

        foreach (var node in GetOverlappingBodies())
        {
            DisableCollision(node);
        }
    }

    protected override void OnAreaExited(Area2D area)
    {
        base.OnAreaExited(area);

        foreach (var node in GetOverlappingBodies())
        {
            EnableCollision(node);
        }
    }
}
