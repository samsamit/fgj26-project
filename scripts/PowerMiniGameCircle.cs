using Godot;
using System;

public partial class PowerMiniGameCircle : TextureButton
{
	private const float MaxTime = 5;
	private const float BeforeTimeMin = 1;
	private const float BeforeTimeMax = 3;

	private const float LateDeviationMin = 1f;
	private const float LateDeviationMax = 2f;

	private TextureRect GoodCircle;
	private TextureRect TooLateCircle;
	private TextureRect GameCircle;

	private float TooLateTime;

	private float AfterTime;
	private double CurrentTime;

	private static readonly Random Random = new();


	public override void _Ready()
	{
		GoodCircle = (TextureRect)GetNode("./GoodCircle");
		TooLateCircle = (TextureRect)GetNode("./TooLateCircle");
		GameCircle = (TextureRect)GetNode("./GameCircle");

		// Ensure child TextureRects don't intercept mouse events
		GoodCircle.MouseFilter = MouseFilterEnum.Ignore;
		TooLateCircle.MouseFilter = MouseFilterEnum.Ignore;
		GameCircle.MouseFilter = MouseFilterEnum.Ignore;

		TooLateTime = GetRandomNumber(BeforeTimeMin, BeforeTimeMax);
		AfterTime = GetRandomNumber(TooLateTime + LateDeviationMin, TooLateTime + LateDeviationMax);

		var goodCircleScale = AfterTime / MaxTime - 0.03f;
		GoodCircle.Scale = new(goodCircleScale, goodCircleScale);
		var tooLateCircleScale = TooLateTime / MaxTime + 0.03f;
		TooLateCircle.Scale = new(tooLateCircleScale, tooLateCircleScale);
	}

	public override void _PhysicsProcess(double delta)
	{
		CurrentTime += delta;
		var gameCircleScale = (float)(1 - CurrentTime / MaxTime);
		GameCircle.Scale = new(gameCircleScale, gameCircleScale);

		if (CurrentTime > MaxTime)
		{
			QueueFree();
		}
	}

	public float GetRandomNumber(float minimum, float maximum)
	{
		return (float)Random.NextDouble() * (maximum - minimum) + minimum;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && 
			mouseButton.ButtonIndex == MouseButton.Left && 
			mouseButton.Pressed)
		{
			// Check if click is within the GameCircle's bounds (the shrinking circle)
			var circleCenter = GlobalPosition + GameCircle.Position + new Vector2(32, 32);
			var circleRadius = 32 * GameCircle.Scale.X;
			var mousePos = GetGlobalMousePosition();
			var distance = circleCenter.DistanceTo(mousePos);
			
			if (distance <= circleRadius)
			{
				GetViewport().SetInputAsHandled(); // Prevent other nodes from receiving this event
				OnClick();
			}
		}
	}

	public void OnClick()
	{
		if (MaxTime - CurrentTime < AfterTime && MaxTime - CurrentTime > TooLateTime)
		{
			var maskPower = GlobalStateManager.Instance.MaskPower.Get();
			var newMaskPower = Math.Min(maskPower += 0.09f, 1);
			GD.Print(newMaskPower);
			GlobalStateManager.Instance.MaskPower.Set(newMaskPower);
		}
		QueueFree();
	}
}
