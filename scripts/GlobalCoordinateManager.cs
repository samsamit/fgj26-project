using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalCoordinateManager : Node2D
{
	private List<Node2D> _spawnPoints = new List<Node2D>();

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
	
	public Vector2 GetRandomSpawnPoint()
	{
		if (_spawnPoints.Count == 0)
		{
			GD.PushError("No spawn points found!");
			return Vector2.Zero;
		}

	//	int randomIndex = (int)GD.Randi() % _spawnPoints.Count;
		return _spawnPoints[0].GlobalPosition;
	}
}
