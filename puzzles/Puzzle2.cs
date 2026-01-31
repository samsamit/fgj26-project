using Godot;
using System;
using System.Collections.Generic;

public partial class Puzzle2 : Puzzle
{
    public override string puzzleId => nameof(Puzzle2);
    
    public override void CompletePuzzle()
    {
        GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PuzzleCompleted, puzzleId);
    }
}