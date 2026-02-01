using Godot;
using System;

public partial class SpeedComponent : Node
{
    [Export] public double MovementSpeed;
    public double CurrentSpeed => MovementSpeed * SlownessModifier;
    public double SlownessModifier = 1f;
}
