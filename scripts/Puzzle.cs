using Godot;
using System;

public abstract partial class Puzzle : Node
{
	public abstract string puzzleId { get; }
	public bool isActive;
	public abstract void CompletePuzzle();
}
