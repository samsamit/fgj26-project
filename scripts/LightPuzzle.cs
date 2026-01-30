using Godot;
using System;

public partial class LightPuzzle : Puzzle
{
	public override void CompletePuzzle()
	{
		//GlobalSettings.levelsBeaten.Add(GetType().Name);
	}
	public override void _Ready()
	{
		base._Ready();
		//GlobalSettings.onLevelCompleted += OnSomeLevelCompleted;
	}
	void OnSomeLevelCompleted(string levelId)
	{
		GD.Print(levelId);
	}


}
