using Godot;
using System;
using System.Collections.Generic;

public class GlobalStateManager
{
    public static HashSet<string> CompletedPuzzle = [];

    public static Vector2 PlayerPosition = Vector2.Zero;
    public static Vector2 MaskPosition = Vector2.Zero;

    [Signal]
    public delegate void LevelCompletedEventHandler();
}
