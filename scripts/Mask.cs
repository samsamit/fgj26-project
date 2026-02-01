using Godot;
using System;

public partial class Mask : Node2D
{
	/// <summary>
	/// Speed at which the mask moves towards the mouse position (pixels per second).
	/// </summary>
	[Export]
	public float FollowSpeed { get; set; } = 125.0f;

	private GlobalStateManager _stateManager;

	// Sprites
	[Export] public Texture2D Round;
	[Export] public Texture2D Square;
	[Export] public Texture2D Star;
	[Export] public Texture2D Triangle;

	bool fullySnapToMouse;
	[Export] public Area2D ViewArea;


	private PointLight2D Light;
	private CanvasModulate Background;
	[Export] public Node2D xRayMaskSprite;

	// Tracks if the current mouse press started on a UI element
	private bool _clickStartedOnUi = false;
	private bool _wasMousePressed = false;
	private AudioStreamPlayer2D _MaskMovingPlayer, _MaskSwitchPlayer;

	private bool AKUNPURKKA = false;

	public override void _Ready()
	{
		GD.Print("Mask script is active!");
		Light = (PointLight2D)GetNode("./Light");
		Background = (CanvasModulate)GetNode("./MaskBackground");

		_MaskMovingPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D_MaskMoving");
		_MaskSwitchPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D_MaskSwitch");
		GlobalStateManager.Instance.CurrentMask.RegisterObserver(SetMask);
	}

	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		bool mousePressed = Input.IsMouseButtonPressed(MouseButton.Left);
		var currentMask = GlobalStateManager.Instance.CurrentMask.Get();

		// Detect mouse button press start
		if (mousePressed && !_wasMousePressed)
		{
			GD.Print("mousePressed && !_wasMousePressed");
			if (!_MaskMovingPlayer.Playing && currentMask != MaskEnum.Strength)
			{
				GD.Print("!_MaskMovingPlayer.Playing");

				float audioLength = (float)_MaskMovingPlayer.Stream.GetLength();
				// Arvotaan aloituskohta. 
				// Varmuuden vuoksi vähennetään pieni siivu (esim 0.1s) lopusta,
				// ettei se aloita aivan lopusta ja lopeta heti.
				float randomStartTime = (float)GD.RandRange(0.0, Math.Max(0, audioLength - 0.1));
				_MaskMovingPlayer.Play(randomStartTime);
				GD.Print("_MaskMovingPlayer.Playing: ", _MaskMovingPlayer.Playing);
			}
			// Mouse just pressed - check if it's over UI
			_clickStartedOnUi = IsMouseOverGui();
		}

		// Detect mouse button release
		if (!mousePressed)
		{
			if (_MaskMovingPlayer.Playing)
			{
				_MaskMovingPlayer.Stop();
			}
			_clickStartedOnUi = false;
		}

		_wasMousePressed = mousePressed;

		// Only move mask if mouse is pressed and click didn't start on UI
		if (mousePressed && !_clickStartedOnUi && currentMask != MaskEnum.Strength)
		{
			GlobalPosition = GlobalPosition.MoveToward(mousePosition, FollowSpeed * (float)delta);
			GlobalStateManager.Instance.MaskPosition = GlobalPosition;
		}
		else if (currentMask == MaskEnum.Strength)
		{
			GlobalPosition = GlobalStateManager.Instance.PlayerPosition;
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

	public void SetMask(MaskEnum mask)
	{
		//Ekalla kiekalla ei toisteta ääniefektiä.
		if (!AKUNPURKKA) AKUNPURKKA = true; else _MaskSwitchPlayer.Play();

		Light.Visible = true;
		switch (mask)
		{
			case MaskEnum.Flashlite:
				Light.Texture = Round;
				Light.Color = new Color("white");
				Light.TextureScale = 0.4f;
				break;
			case MaskEnum.Basic:
				Light.Texture = Square;
				Light.Color = new Color("white");
				Light.TextureScale = 0.5f;
				break;
			case MaskEnum.XRay:
				Light.Visible = false;
				break;
			case MaskEnum.Strength:
				Light.Visible = false;
				break;
			case MaskEnum.Slow:
				Light.Texture = Round;
				Light.Color = new Color("white");
				Light.TextureScale = 0.5f;
				break;
			default:
				Light.Texture = Round;
				Light.Color = new Color("white");
				Light.TextureScale = 0.5f;
				break;
		}

		Background.Visible = mask == MaskEnum.Flashlite;
		xRayMaskSprite.Visible = mask == MaskEnum.XRay;
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
				circle.Radius = Light.Texture.GetSize().X * Light.TextureScale / 2f;
				break;

			case RectangleShape2D rect:
				rect.Size = Light.Texture.GetSize() * Light.TextureScale;
				break;

			case ConvexPolygonShape2D polygon:
				Vector2[] points = polygon.Points;
				for (int i = 0; i < points.Length; i++)
				{
					points[i] = points[i].Normalized() * (Light.Texture.GetSize().X * Light.TextureScale / 2f);
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
