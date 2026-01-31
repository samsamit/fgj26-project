using Godot;
using System;

public partial class MineMask : Sprite2D
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        GlobalPosition = GetGlobalMousePosition();
    }

}
