using Godot;
using System.Collections.Generic;
using System;
using System.Linq;

public partial class GlobalStateManager : Node
{
	public HashSet<string> CompletedPuzzles = [];

	private static MaskEnum[] DefaultAvailableMasks =
	{
		MaskEnum.Flashlite,
	};

	private static MaskEnum DefaultMask = MaskEnum.Flashlite;

	private static int DefaultHealth = 3;
	private static float DefaultMaskPower = 1f;

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
	public Observable<List<MaskEnum>> AvailableMasks = new(DefaultAvailableMasks.ToList());

	public MaskEnum PreviousMask = MaskEnum.Basic;
	public Observable<MaskEnum> CurrentMask = new(DefaultMask);
	public Observable<float> MaskPower = new(1f);
	public Observable<int> Health = new(3);

	public static GlobalStateManager Instance;

	public override void _EnterTree()
	{
		Instance = this;
		ResetState();
	}

	public override void _ExitTree()
	{
		Instance = null;
	}

	public void ResetState()
	{
		AvailableMasks.DeregisterAllObservers();
		CurrentMask.DeregisterAllObservers();
		MaskPower.DeregisterAllObservers();
		Health.DeregisterAllObservers();

		AvailableMasks.Set(DefaultAvailableMasks.ToList());
		CurrentMask.Set(DefaultMask);
		MaskPower.Set(DefaultMaskPower);
		Health.Set(DefaultHealth);
		CompletedPuzzles.Clear();
	}

	public void ResumeGame()
	{
		this.SetPhysicsProcess(true);
	}

	public void RestartGame()
	{
		ResetState();
		ResumeGame();
	}

	public void PauseGame()
	{
		this.SetPhysicsProcess(false);
	}

	public override void _Ready()
	{
		PuzzleCompleted += OnPuzzleCompleted;
		ResetState();
	}

	private void OnPuzzleCompleted(string puzzleName)
	{
		GD.Print("Puzzle completed: " + puzzleName);
		CompletedPuzzles.Add(puzzleName);
	}

	public override void _PhysicsProcess(double delta)
	{

		if (CurrentMask.Get() == MaskEnum.Strength)
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
