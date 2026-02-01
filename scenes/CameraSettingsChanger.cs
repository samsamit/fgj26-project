using Godot;
using System;

public partial class CameraSettingsChanger : Area2D
{
    [Export] MainCamera mainCamera;
    [Export] float zoomSpeed;
    [Export] float zoom;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player) return;
        mainCamera.ZoomSpeed = zoomSpeed;
        if (zoomSpeed == 0)
        {
            mainCamera.Zoom = zoom * Vector2.One;
        }
    }
}
