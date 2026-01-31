using Godot;
using System.Collections.Generic;
using System;

public partial class GlobalStateManager : Node
{
	public HashSet<string> CompletedPuzzle = [];

	[Export]
	public Player Player;

	[Export]
	public Mask Mask;


	public Vector2 PlayerPosition = Vector2.Zero;
	public Vector2 MaskPosition = Vector2.Zero;
	public Observable<List<MaskEnum>> AvailableMasks = new([MaskEnum.Flashlite, MaskEnum.Basic, MaskEnum.XRay]);
	public Observable<MaskEnum> CurrentMask = new(MaskEnum.Flashlite);
	public Observable<int> Health = new(3);

	public static GlobalStateManager Instance;

	public override void _EnterTree()
	{
		base._EnterTree();
		Instance = this;
	}

	public override void _Ready()
	{
		base._Ready();

		PuzzleCompleted += puzzleName =>
		{
			GD.Print("hey");
			CompletedPuzzle.Add(puzzleName);
		};
	}

	[Signal]
	public delegate void PuzzleCompletedEventHandler(string puzzleName);

	public static Action<string> onPuzzleCompleted;
	public static void OnPuzzleCompleted()
	{

	}

	public void AddMask(MaskEnum maskEnum)
	{
		var currentAvailableMasks = AvailableMasks.Get();
		currentAvailableMasks.Add(maskEnum);
		AvailableMasks.Set(currentAvailableMasks);
		CurrentMask.Set(maskEnum);
	}
}
