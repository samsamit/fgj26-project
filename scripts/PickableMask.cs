using Godot;
using System;

public partial class PickableMask : Node2D
{
	[Export]
	public MaskEnum NewMask;

	[Export]
	private Texture2D BasicMask;
	[Export]
	private Texture2D FlashliteMask;
	[Export]
	private Texture2D StrengthMask;
	[Export]
	private Texture2D XRayMask;

	[Export]
	private Puzzle puzzleToUnlock;

	public override void _Ready()
	{
		var sprite = (Sprite2D)GetNode("./Sprite2D");
		switch (NewMask)
		{
			case MaskEnum.Basic:
				sprite.Texture = BasicMask;
				break;
			case MaskEnum.Flashlite:
				sprite.Texture = FlashliteMask;
				break;
			case MaskEnum.Strength:
				sprite.Texture = StrengthMask;
				break;
			case MaskEnum.XRay:
				sprite.Texture = XRayMask;
				break;
		}
		Visible = false;

		GlobalStateManager.Instance.PuzzleCompleted += OnPuzzleCompleted;
	}

    private void OnPuzzleCompleted(string puzzleId)
    {
        if (puzzleToUnlock != null && puzzleId == puzzleToUnlock.puzzleId)
		{
			Visible = true;
		}
    }

    public void OnAreaEntered(Node2D body)
	{
		if (body is Player)
		{
			QueueFree();
			GlobalStateManager.Instance.AddMask(NewMask);
		}
	}
}
