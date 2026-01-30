using Godot;
using System;

public partial class Goal : Area2D 
{
    [Export] GlobalStateManager stateManager;
    [Export] string puzzleId;
    public override void _Ready()
    {
        stateManager.PuzzleCompleted += SomethingHappened;
        base._Ready();
        BodyEntered += OnBodyEntered;
    }
    void SomethingHappened(string value)
    {
        GD.Print(value);
    }
    public void OnBodyEntered(Node body)
    {
        GD.Print("goal");
        stateManager.EmitSignal(GlobalStateManager.SignalName.PuzzleCompleted, puzzleId);
        BodyEntered -= OnBodyEntered;
    }
}
