using Godot;
using System;

public partial class DarknessManager : Node
{
	[Export] CanvasModulate darknessOverlay;

	public override void _Ready()
	{
		base._Ready();
		darknessOverlay.Visible = true;
	}

}
