using Godot;
using System;

public partial class UndergroundBomb : Sprite2D
{
    [Export] public Sprite2D explosion;
    private bool active = true;
    private bool playerIsInside = false;
    private async void _on_area_2d_body_entered(Node2D body)
    {
            playerIsInside = true;
    }

    private void _on_area_2d_body_exited(Node2D body)
    {
        playerIsInside = false;
    }

    public void SetActive(bool state)
    {
        active = state;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (active && playerIsInside)
        {
            playerIsInside = false;
            Boom();
        }
    }
    private async void Boom()
    {
        GD.Print("boom");
        explosion.Visible = true;
        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        explosion.Visible = false;
    }

}
