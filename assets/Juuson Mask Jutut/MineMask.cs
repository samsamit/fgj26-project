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

		if (GlobalStateManager.Instance.CurrentMask.Get() == MaskEnum.XRay)
		{
			Modulate = new Color("#ad030038");
		}
		else
		{
			Modulate = new Color("#00000000");
		}
	}

}
