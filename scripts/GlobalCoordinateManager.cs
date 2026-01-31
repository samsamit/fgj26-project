using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalCoordinateManager : Node2D
{
	private List<Node> spawnPoints = new List<Node>();

	public override void _Ready()
	{
		// Automatically gather spawn points from children
		spawnPoints.Clear();
		foreach (Node node in GetTree().GetNodesInGroup("spawn_points"))
		{
			spawnPoints.Add(node);
		}

		GD.Print("Found spawn points: ", spawnPoints.Count);
	}

	// Get a random spawn point's global position
	public Vector3 GetRandomSpawnPoint()
	{
		if (spawnPoints.Count == 0)
		{
			GD.PushError("No spawn points found!");
			return Vector3.Zero;
		}

		var randomIndex = (int)GD.Randi() % spawnPoints.Count;
		var spawnNode = spawnPoints[randomIndex] as Node3D;
		if (spawnNode == null)
		{
			GD.PushError("Spawn point is not a Node3D!");
			return Vector3.Zero;
		}

		return spawnNode.GlobalTransform.Origin;
	}
}
