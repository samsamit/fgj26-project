using Godot;
using System;

public partial class PowerMiniGame : Control
{
	private PackedScene PowerMiniGameCircle;
	private double NextSpawnTime = 0;
	private double CurrentTime = 0;
	private const double SpawnSpeed = 0.75;
	private const int MaxSpawnX = 200;
	private const int MaxSpawnY = 256;
	private const int MaxSpawn = 10;
	private Godot.Vector2[] PreviousSpawns = new Godot.Vector2[MaxSpawn];
	private int LastSpawn = 0;
	private static readonly Random Random = new();

	public override void _Ready()
	{
		PowerMiniGameCircle = ResourceLoader.Load<PackedScene>("res://scenes/PowerMiniGameCircle.tscn");
		MouseFilter = MouseFilterEnum.Stop;
	}

	public override void _PhysicsProcess(double delta)
	{
		CurrentTime += delta;
		if (NextSpawnTime == 0)
		{
			NextSpawnTime = delta + SpawnSpeed;
		}

		if (CurrentTime > NextSpawnTime)
		{
			NextSpawnTime = CurrentTime + SpawnSpeed;
			LastSpawn++;
			LastSpawn = LastSpawn == MaxSpawn ? LastSpawn = 0 : LastSpawn++;
			var newSpawn = GetNewFreeCoordinate();
			PreviousSpawns[LastSpawn] = newSpawn;
			var newCircle = (PowerMiniGameCircle)PowerMiniGameCircle.Instantiate();
			AddChild(newCircle);
			newCircle.Position = new Vector2(newSpawn.X, -newSpawn.Y);
		}
	}

	private Vector2 GetNewFreeCoordinate()
	{
		var newPosition = new Vector2(GetRandomNumber(0, MaxSpawnX), GetRandomNumber(0, MaxSpawnY));
		for (var i = 0; i > 100; i++)
		{
			var newPositionProposalX = GetRandomNumber(0, MaxSpawnX);
			var newPositionProposalY = GetRandomNumber(0, MaxSpawnY);
			var failed = false;
			foreach (var previousSpawn in PreviousSpawns)
			{
				if (Math.Abs(previousSpawn.X - newPositionProposalX) < 32)
				{
					failed = true;
					break;
				}
				if (Math.Abs(previousSpawn.Y - newPositionProposalY) < 32)
				{
					failed = true;
					break;
				}
			}
			if (failed)
			{
				continue;
			}
			newPosition.X = newPositionProposalX;
			newPosition.Y = newPositionProposalY;
			break;
		}
		return newPosition;
	}

	private float GetRandomNumber(float minimum, float maximum)
	{
		return (float)Random.NextDouble() * (maximum - minimum) + minimum;
	}
}
