using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;

public partial class HouseArea : Area2D
{

    [Export]
    public Player Player;

    [Export]
    public TileMapLayer BuildingsLayer;

    [Export] 
    public CollisionPolygon2D CollisionPolygon;

    private List<Vector2I> _tilesInsideHouseArea = new List<Vector2I>();

    public override void _Ready()
    {
        base._Ready();
        this.BodyEntered += OnBodyEntered;
        this.BodyExited += OnBodyExited;
        _tilesInsideHouseArea = GetTilesInsideHouseArea();
        GD.Print($"House area {this.ToString()} has {_tilesInsideHouseArea.Count} tiles inside it.");
    }

    private void OnBodyExited(Node2D body)
    {
        if (body == Player)
        {
            GD.Print($"Player exited the house area {this.ToString()}!");
            // Restore tiles to full opacity
            foreach (var cell in _tilesInsideHouseArea)
            {
                int sourceId = BuildingsLayer.GetCellSourceId(cell);
                Vector2I atlasCoords = BuildingsLayer.GetCellAtlasCoords(cell);
                GD.Print($"Restoring tile at {cell} with source id {sourceId} and atlas coordinates {atlasCoords}.");
                BuildingsLayer.SetCell(cell, sourceId, atlasCoords: atlasCoords);
            }
        }
    }


    private void OnBodyEntered(Node2D body)
    {
        if (body == Player)
        {
            GD.Print($"Player entered the house area {this.ToString()}!");
        }
        // Hide only the tiles that are inside the house area with modulation:
        foreach (var cell in _tilesInsideHouseArea)
        {
            int sourceId = BuildingsLayer.GetCellSourceId(cell);
            Vector2I atlasCoords = BuildingsLayer.GetCellAtlasCoords(cell);
            const int AlternativeTile = 1; // Vittu tätä alternative tile systeemiä
            GD.Print($"Hiding tile at {cell} with source id {sourceId} and atlas coordinates {atlasCoords} by setting it to alternative tile {1}.");
            BuildingsLayer.SetCell(cell, sourceId, atlasCoords: atlasCoords, alternativeTile: AlternativeTile);
        }

    }

    private List<Vector2I> GetTilesInsideHouseArea()
    {
        var tilesInside = new List<Vector2I>();
        
        if (BuildingsLayer == null || CollisionPolygon == null)
            return tilesInside;

        // Get the polygon points in global coordinates
        var polygonPoints = CollisionPolygon.Polygon;
        var globalPolygon = new Vector2[polygonPoints.Length];
        for (int i = 0; i < polygonPoints.Length; i++)
        {
            globalPolygon[i] = CollisionPolygon.ToGlobal(polygonPoints[i]);
        }

        // Get all used cells in the tilemap layer
        var usedCells = BuildingsLayer.GetUsedCells();
        
        foreach (var cell in usedCells)
        {
            // Convert tile cell position to world position (center of the tile)
            var tileWorldPos = BuildingsLayer.ToGlobal(BuildingsLayer.MapToLocal(cell));
            
            // Check if the tile center is inside the polygon
            if (Geometry2D.IsPointInPolygon(tileWorldPos, globalPolygon))
            {
                tilesInside.Add(cell);
            }
        }

        return tilesInside;
    }

}
