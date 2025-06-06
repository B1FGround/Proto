using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new Dictionary<Vector3Int, PlacementData>();

    public void AddObjectAt(Vector3Int gridPos, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPos, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var position in positionToOccupy)
        {
            if (placedObjects.ContainsKey(position))
                throw new Exception($"Dictionary already contains this cell position {position}");

            placedObjects[position] = data;
        }

    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> value = new List<Vector3Int>();
        for(int x = 0; x < objectSize.x; ++x)
        {
            for (int y = 0; y < objectSize.y; ++y)
                value.Add(gridPosition + new Vector3Int(x, 0, y));
        }
        return value;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPos)
    {
        if (placedObjects.ContainsKey(gridPos) == false)
            return -1;

        return placedObjects[gridPos].PlacedObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPos)
    {
        foreach(var pos in placedObjects[gridPos].occupiedPositions)
            placedObjects.Remove(pos);
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
