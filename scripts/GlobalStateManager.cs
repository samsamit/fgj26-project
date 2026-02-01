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

	private double NextPowerDecreaseTime = 0;
	private double CurrentTime = 0;
	private const double DecreaseSpeed = 0.5f;
	private const float DecreaseAmount = 0.02f;

	public Vector2 PlayerPosition = Vector2.Zero;
	public Vector2 MaskPosition = Vector2.Zero;
	public Observable<List<MaskEnum>> AvailableMasks = new([MaskEnum.Flashlite, MaskEnum.Slow]);
	public Observable<MaskEnum> CurrentMask = new(MaskEnum.Flashlite);
	public Observable<float> MaskPower = new(1f);
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

		PuzzleCompleted += OnPuzzleCompleted;
		Instance = this;
		CurrentMask.Set(MaskEnum.Flashlite);
	}

	private void OnPuzzleCompleted(string puzzleName)
	{
		GD.Print("Puzzle completed: " + puzzleName);
		CompletedPuzzle.Add(puzzleName);
	}

	public override void _PhysicsProcess(double delta)
	{
		CurrentTime += delta;
		if (NextPowerDecreaseTime == 0)
		{
			NextPowerDecreaseTime = delta + DecreaseSpeed;
		}

		if (CurrentTime > NextPowerDecreaseTime)
		{
			NextPowerDecreaseTime = CurrentTime + DecreaseSpeed;
			var maskPower = MaskPower.Get();
			if (CurrentMask.Get() == MaskEnum.Strength)
			{
				var newMaskPower = Math.Max(maskPower -= DecreaseAmount, 0);
				MaskPower.Set(newMaskPower);
			}
			else
			{
				var newMaskPower = Math.Min(maskPower += DecreaseAmount, 1);
				MaskPower.Set(newMaskPower);
			}
		}
	}

	[Signal]
	public delegate void PuzzleCompletedEventHandler(string puzzleName);

	[Signal]
	public delegate void PlayerHitEventHandler();

	public void AddMask(MaskEnum maskEnum)
	{
		var currentAvailableMasks = AvailableMasks.Get();
		currentAvailableMasks.Add(maskEnum);
		AvailableMasks.Set(currentAvailableMasks);
		CurrentMask.Set(maskEnum);
	}
}
