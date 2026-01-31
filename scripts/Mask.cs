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

	[Export]
	public Area2D ViewArea;

	[Export]
	public CanvasModulate darkness;

	private PointLight2D Light;

	// Tracks if the current mouse press started on a UI element
	private bool _clickStartedOnUi = false;
	private bool _wasMousePressed = false;

	public override void _Ready()
	{
		GD.Print("Mask script is active!");
		Light = (PointLight2D)GetNode("./Light");

		GlobalStateManager.Instance.CurrentMask.RegisterObserver(
			newMask => SetMask(newMask, 0.2f, new Color("white")));
	}

	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		bool mousePressed = Input.IsMouseButtonPressed(MouseButton.Left);

		// Detect mouse button press start
		if (mousePressed && !_wasMousePressed)
		{
			// Mouse just pressed - check if it's over UI
			_clickStartedOnUi = IsMouseOverGui();
		}

		// Detect mouse button release
		if (!mousePressed)
		{
			_clickStartedOnUi = false;
		}

		_wasMousePressed = mousePressed;

		// Only move mask if mouse is pressed and click didn't start on UI
		if (mousePressed && !_clickStartedOnUi)
		{
			GlobalPosition = GlobalPosition.MoveToward(mousePosition, FollowSpeed * (float)delta);
			GlobalStateManager.Instance.MaskPosition = GlobalPosition;
		}
	}

	/// <summary>
	/// Returns true if the mouse is currently hovering over a GUI control.
	/// </summary>
	private bool IsMouseOverGui()
	{
		return GetViewport().GuiGetHoveredControl() != null;
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
			MaskEnum.Flashlite => Round,
			MaskEnum.Basic => Square,
			MaskEnum.XRay => Star,
			MaskEnum.Strength => Triangle,
			_ => Round,
		};

		darkness.Visible = mask == MaskEnum.Flashlite;

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
	
	public void SpawnAt(Vector2 position)
	{
		GlobalPosition = position;
	}
}
