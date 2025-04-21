using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GridInputManager gridInputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectDatabase database;
    [SerializeField] private GameObject gridVisualization;

    private GridData floorData;
    private GridData buildData;

    [SerializeField] private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPositions = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;

    private IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        buildData = new GridData();
        floorData = new GridData();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(id, grid, previewSystem, database, floorData, buildData, objectPlacer, gridVisualization);

        gridInputManager.OnClicked += PlaceStructure;
        gridInputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, previewSystem, floorData, buildData, objectPlacer, gridVisualization);
        gridInputManager.OnClicked += PlaceStructure;
        gridInputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (gridInputManager.IsPointerOverUI())
            return;

        var mousePos = gridInputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        buildingState.OnAction(gridPos);
    }

    //private bool CheckPlacement(Vector3Int gridPos, int selectedObjectIndex)
    //{
    //    // 바닥 타일이 있는 경우
    //    GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : buildData;

    //    return selectedData.CanPlaceObjectAt(gridPos, database.objectData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (buildingState == null)
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();

        gridInputManager.OnClicked -= PlaceStructure;
        gridInputManager.OnExit -= StopPlacement;
        lastDetectedPositions = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        var mousePos = gridInputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        if (lastDetectedPositions != gridPos)
        {
            buildingState.UpdateState(gridPos);
            lastDetectedPositions = gridPos;
        }
    }
}