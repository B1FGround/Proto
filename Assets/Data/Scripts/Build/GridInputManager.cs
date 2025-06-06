using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInputManager : MonoBehaviour
{
    [SerializeField]
    LayerMask placementLayermask;

    private Vector3 lastPosition;

    public Action OnClicked;
    public Action OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayermask))
            lastPosition = hit.point;
        return lastPosition;
    }
}
