using Godot;
using System;

public partial class PickableMask : Node2D
{
	[Export]
	public MaskEnum NewMask;

	public void OnAreaEntered(Node2D body)
	{
		if (body is Player)
		{
			QueueFree();
			GlobalStateManager.Instance.AddMask(NewMask);
		}
	}
}
