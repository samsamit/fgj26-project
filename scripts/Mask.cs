using Godot;
using System;

public partial class Mask : Node2D
{
	/// <summary>
	/// Speed at which the mask moves towards the mouse position (pixels per second).
	/// </summary>
	[Export]
	public float FollowSpeed { get; set; } = 100.0f;

	private GlobalStateManager _stateManager;
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
		GD.Print("Mask script is active!");
		Light = (PointLight2D)GetNode("./Light");

		GlobalStateManager.Instance.CurrentMask.RegisterObserver(
			newMask => SetMask(newMask, 0.5f, new Color("white")));
	}

	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();

		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{

			// Move towards the mouse position at the configured speed
			GlobalPosition = GlobalPosition.MoveToward(mousePosition, FollowSpeed * (float)delta);

			GlobalStateManager.Instance.MaskPosition = GlobalPosition;
		}
	}

	private void Area2DBodyEntered(Node body)
	{
		GD.Print("Body entered: " + body.Name);
	}

	private void Area2DBodyExited(Node body)
	{
		GD.Print("Body exited: " + body.Name);
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

		// Dynamic scaling for the collision shape, so that it matches the mask
		CollisionShape2D collisionShape = GetNode<CollisionShape2D>("./Area2D/CollisionShape2D");

		if (collisionShape?.Shape == null)
		{
			GD.PrintErr("CollisionShape or its Shape is null!");
			return;
		}

		switch (collisionShape.Shape)
		{
			case CircleShape2D circle:
				circle.Radius = maskSize / 2f;
				break;

			case RectangleShape2D rect:
				rect.Size = new Vector2(maskSize, maskSize);
				break;

			case ConvexPolygonShape2D polygon:
				Vector2[] points = polygon.Points;
				for (int i = 0; i < points.Length; i++)
				{
					points[i] = points[i].Normalized() * (maskSize / 2f);
				}
				polygon.Points = points;
				break;

			default:
				GD.Print("Unhandled shape type: " + collisionShape.Shape.GetType());
				break;
		}

	}
}
