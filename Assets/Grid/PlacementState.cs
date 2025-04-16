using System;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private int iD;
    private Grid grid;
    private PreviewSystem previewSystem;
    private ObjectDatabase database;
    private GridData floorData;
    private GridData buildData;
    private ObjectPlacer objectPlacer;
    private GameObject gridVisualization;
    private Vector3Int lastPreviewPos;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectDatabase database, GridData floorData, GridData buildData, ObjectPlacer objectPlacer, GameObject gridVisualization)
    {
        this.iD = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.buildData = buildData;
        this.objectPlacer = objectPlacer;
        this.gridVisualization = gridVisualization;

        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == iD);
        if (selectedObjectIndex > -1)
            previewSystem.StartShowingPlacementPreview(database.objectData[selectedObjectIndex].Prefab, database.objectData[selectedObjectIndex].Size);
        else
            throw new Exception($"No object with ID {iD}");
        this.gridVisualization = gridVisualization;
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    private bool CheckPlacement(Vector3Int gridPos, int selectedObjectIndex)
    {
        // 바닥 타일이 있는 경우
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : buildData;

        return selectedData.CanPlaceObjectAt(gridPos, database.objectData[selectedObjectIndex].Size);
    }

    public void OnAction(Vector3Int gridPos)
    {
        bool placeAvailable = CheckPlacement(gridPos, selectedObjectIndex);
        if (placeAvailable == false)
            return;

        int index = objectPlacer.PlaceObject(database.objectData[selectedObjectIndex].Prefab, grid.CellToWorld(lastPreviewPos));

        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : buildData;

        selectedData.AddObjectAt(lastPreviewPos, database.objectData[selectedObjectIndex].Size, database.objectData[selectedObjectIndex].ID, index);

        previewSystem.UpdatePosition(grid.CellToWorld(lastPreviewPos), false);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        // 그리드를 넘어서 그려지지 않도록 제한
        var gridMatScale = gridVisualization.GetComponent<Renderer>().material.GetVector("_Scale");
        Vector2Int placementPos = new Vector2Int(gridPos.x + database.objectData[selectedObjectIndex].Size.x, gridPos.y + database.objectData[selectedObjectIndex].Size.y);

        if (placementPos.x > gridVisualization.transform.position.x + (gridMatScale.x / 2) * gridVisualization.transform.localScale.x ||
            placementPos.y > gridVisualization.transform.position.z + (gridMatScale.y / 2) * gridVisualization.transform.localScale.y ||
            gridPos.x < gridVisualization.transform.position.x - (gridMatScale.x / 2) * gridVisualization.transform.localScale.x ||
            gridPos.y < gridVisualization.transform.position.z - (gridMatScale.y / 2) * gridVisualization.transform.localScale.y)
            return;
        else
            lastPreviewPos = gridPos;

        bool placeAvailable = CheckPlacement(gridPos, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), placeAvailable);
    }
}