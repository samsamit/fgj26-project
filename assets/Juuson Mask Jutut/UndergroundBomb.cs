using Godot;
using System;

public partial class UndergroundBomb : Sprite2D
{
    [Export] public Sprite2D explosion;
    private bool active = true;
    private async void _on_area_2d_body_entered(Node2D body)
    {
        GD.Print("boom");
        explosion.Visible = true;
        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        explosion.Visible = false;
    }

    public void SetActive(bool state)
    {
        active = state;
    }
}
