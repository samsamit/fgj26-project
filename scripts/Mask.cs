using Godot;
using System;

public partial class Mask : Node2D
{
	public const float Speed = 200.0f;

	[Export]
	public Texture2D Round;

	[Export]
	public Texture2D Square;

	[Export]
	public Texture2D Star;

	[Export]
	public Texture2D Triangle;

	private PointLight2D Light;

	public override void _Ready()
	{
		GD.Print("Player script is active!");
		Light = (PointLight2D)GetNode("./Light");
	}

	public override void _PhysicsProcess(double delta)
	{
		return;
		Vector2 velocity = Vector2.Zero;

		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{

			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 direction = (mousePosition - GlobalPosition).Normalized();
			velocity = direction * Speed;
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		GlobalPosition = GetGlobalMousePosition();
		//SetMask(MaskEnum.Square, 1, new Color("red"));
	}

	public void SetMask(MaskEnum mask, float maskSize, Color maskColor)
	{
		Light.TextureScale = maskSize;
		Light.Color = maskColor;
		Light.Texture = mask switch
		{
			MaskEnum.Round => Round,
			MaskEnum.Square => Square,
			MaskEnum.Star => Star,
			MaskEnum.Triangle => Triangle,
			_ => Round,
		};
	}
}
