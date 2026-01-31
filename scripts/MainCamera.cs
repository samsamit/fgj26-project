using Godot;
using System;

public partial class MainCamera : Camera2D
{
	[Export]
	public Player MainPlayer;

	[Export]
	public Mask MaskPlayer;

	[Export]
	public float Speed = 100f;

	[Export]
	public float MinZoom = 0.5f;

	[Export]
	public float MaxZoom = 1;

	[Export]
	public float ZoomSpeed = 0.2f;

	[Export]
	public float BeginCameraMove = 50;

	[Export]
	public float TooFar = 310;

	[Export]
	public float Close = 250;

	public override void _PhysicsProcess(double delta)
	{
		var middlePoint = (MainPlayer.GlobalPosition + MaskPlayer.GlobalPosition) / 2;
		var middlePointDeviation = (GlobalPosition - middlePoint).Length();

		if (middlePointDeviation * Zoom.X > BeginCameraMove)
		{
			var newGlobalPosition = new Vector2(
				Mathf.MoveToward(GlobalPosition.X, middlePoint.X, Speed * (float)delta),
				Mathf.MoveToward(GlobalPosition.Y, middlePoint.Y, Speed * (float)delta)
			);
			GlobalPosition = newGlobalPosition;
		}

		if (IsClose(MainPlayer) && IsClose(MaskPlayer) && Zoom.X < MaxZoom)
		{
			float newZoom = Zoom.X + (float)delta * ZoomSpeed;
			Zoom = new Vector2(newZoom, newZoom);
		}
		if ((IsTooFarFromCamera(MainPlayer) || IsTooFarFromCamera(MaskPlayer)) && Zoom.X > MinZoom)
		{
			float newZoom = Zoom.X - (float)delta * ZoomSpeed;
			Zoom = new Vector2(newZoom, newZoom);
		}
	}

	private bool IsTooFarFromCamera(Node2D node)
	{
		return (GlobalPosition - node.GlobalPosition).Length() * Zoom.X > TooFar;
	}

	private bool IsClose(Node2D node)
	{
		return (GlobalPosition - node.GlobalPosition).Length() * Zoom.X < Close;
	}
}
