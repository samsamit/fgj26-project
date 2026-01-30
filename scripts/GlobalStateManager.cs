using Godot;
using System.Collections.Generic;

public partial class GlobalStateManager : Node
{
    public static HashSet<string> CompletedPuzzle = [];

    public static Vector2 PlayerPosition = Vector2.Zero;
    public static Vector2 MaskPosition = Vector2.Zero;

    public override void _Ready()
    {
        PuzzleCompleted += puzzleName => CompletedPuzzle.Add(puzzleName);
    }

    [Signal]
    public delegate void PuzzleCompletedEventHandler(string puzzleName);
}
