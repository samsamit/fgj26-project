using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalCoordinateManager : Node2D
{
	private List<Node2D> _spawnPoints = new List<Node2D>();
	private Player _player;
	private Mask _mask;
	
	public override void _Ready()
	{
		_spawnPoints.Clear();
		foreach (Node node in GetTree().GetNodesInGroup("spawn_points"))
		{
			if (node is Node2D node2D)
				_spawnPoints.Add(node2D);
		}

		GD.Print("Found spawn points: ", _spawnPoints.Count);
	}
	
	/// <summary>
	/// Selects a random spawn point out of all the defined ones
	/// </summary>
	/// <returns>Vector2: The spawn point</returns>
	public Vector2 GetRandomSpawnPoint()
	{
		GD.Print("RUNNING");
		if (_spawnPoints.Count == 0)
		{
			GD.PushError("No spawn points found!");
			return Vector2.Zero;
		}

		int randomIndex = (int)GD.Randi() % _spawnPoints.Count;
		return _spawnPoints[randomIndex].GlobalPosition;
	}

	/// <summary>
	/// Respawns both Player and Mask to the most suitable coordinate,
	/// e.g. latest checkpoint
	/// </summary>
	public void Respawn()
	{
		// TODO
		// Right now sets the player to a random coordinate. The proper
		// coordinates have to be specified later.
		Vector2 RespawnCoordinate = GetRandomSpawnPoint();
		_player.SpawnAt(RespawnCoordinate);
		_mask.SpawnAt(RespawnCoordinate);
	}
}
