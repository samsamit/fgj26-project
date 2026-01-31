using Godot;
using System;

public partial class MineMask : Sprite2D
{
    [Export] Node2D nodeToFollow;
    public override void _Process(double delta)
    {
        base._Process(delta);
        GlobalPosition = nodeToFollow.GlobalPosition;
        //GlobalPosition = GetGlobalMousePosition();
    }

}
