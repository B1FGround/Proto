using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData buildData;
    ObjectPlacer objectPlacer;
    GameObject gridVisualization;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData floorData, GridData buildData, ObjectPlacer objectPlacer, GameObject gridVisualization)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.buildData = buildData;
        this.objectPlacer = objectPlacer;
        this.gridVisualization = gridVisualization;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos)
    {
        GridData selectedData = null;

        if (floorData.CanPlaceObjectAt(gridPos, Vector2Int.one) == false)
            selectedData = floorData;
        else if (buildData.CanPlaceObjectAt(gridPos, Vector2Int.one) == false)
            selectedData = buildData;

        if (selectedData == null)
        {

        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPos);
            if (gameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPos);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        Vector3 cellPosition = grid.CellToWorld(gridPos);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionisValid(gridPos));
    }

    private bool CheckIfSelectionisValid(Vector3Int gridPos) => !(buildData.CanPlaceObjectAt(gridPos, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPos, Vector2Int.one)); 

    public void UpdateState(Vector3Int gridPos)
    {
        // 그리드를 넘어서 그려지지 않도록 제한
        var gridMatScale = gridVisualization.GetComponent<Renderer>().material.GetVector("_Scale");
        Vector2Int placementPos = new Vector2Int(gridPos.x, gridPos.y);

        if (placementPos.x > gridVisualization.transform.position.x + (gridMatScale.x / 2) * gridVisualization.transform.localScale.x ||
            placementPos.y > gridVisualization.transform.position.z + (gridMatScale.y / 2) * gridVisualization.transform.localScale.y ||
            gridPos.x < gridVisualization.transform.position.x - (gridMatScale.x / 2) * gridVisualization.transform.localScale.x ||
            gridPos.y < gridVisualization.transform.position.z - (gridMatScale.y / 2) * gridVisualization.transform.localScale.y)

            return;
        bool validity = CheckIfSelectionisValid(gridPos);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), validity);
    }
}
