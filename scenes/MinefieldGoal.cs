using Godot;
using System;

public partial class MinefieldGoal : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player) return;
        foreach (Node node in GetTree().GetNodesInGroup("BombSwitches"))
        {
            if (node is BombSwitch script)
            {
                script.FullyDisable();
            }
        }
    }
}
