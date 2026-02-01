using Godot;
using System;

public partial class BombSwitch : Area2D
{
    [Export] public Sprite2D switchGreen;
    [Export] public Sprite2D switchRed;
    [Export] public UndergroundBomb bomb;
    bool onGreen = false;
    bool fullyDisabled = false;
    public override void _Ready()
    {
        base._Ready();
        AddToGroup("BombSwitches");
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
            await ToSignal(GetTree().CreateTimer(5.0), SceneTreeTimer.SignalName.Timeout);
            if (fullyDisabled) return;
            bomb.SetActive(true);
            switchGreen.Visible = false;
            switchRed.Visible = true;
            onGreen = false;
        }
    }
    public void FullyDisable()
    {
        fullyDisabled = true;
        onGreen = true;
        bomb.SetActive(false);
        switchGreen.Visible = true;
        switchRed.Visible = false;
    }
}
