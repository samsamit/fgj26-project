using Godot;
using System;

public partial class BombSwitch : Area2D
{
    [Export] public Sprite2D switchGreen;
    [Export] public Sprite2D switchRed;
    [Export] public UndergroundBomb bomb;
    bool onGreen = false;
    public override void _Ready()
    {
        base._Ready();
        InputEvent += OnInputEvent;
    }

    private void OnInputEvent(
        Node viewport,
        InputEvent @event,
        long shapeIdx
    )
    {
        if (@event is InputEventMouseButton mouseEvent &&
            mouseEvent.Pressed &&
            mouseEvent.ButtonIndex == MouseButton.Left)
        {
            GD.Print("Object clicked!");
            EmitClicked();
        }
    }

    private async void EmitClicked()
    {
        if (!onGreen)
        {
            onGreen = true;
            bomb.SetActive(false);
            switchGreen.Visible = true;
            switchRed.Visible = false;
            await ToSignal(GetTree().CreateTimer(3.0), SceneTreeTimer.SignalName.Timeout);
            bomb.SetActive(false);
            switchGreen.Visible = false;
            switchRed.Visible = true;
        }
    }

}
