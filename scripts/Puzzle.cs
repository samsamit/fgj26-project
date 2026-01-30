using Godot;
using System;

public abstract partial class Puzzle : Node
{
    [Export] public string puzzleId;
    [Export] public bool isActive;
    public abstract void CompletePuzzle();
}
