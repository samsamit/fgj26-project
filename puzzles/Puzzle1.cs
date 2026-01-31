using Godot;
using System;

public partial class Puzzle1 : Puzzle
{
    public override string puzzleId => nameof(Puzzle1);


    public override void CompletePuzzle()
    {
        GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PuzzleCompleted, puzzleId);
    }

}
