using UnityEngine;

public class Lightning : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public GameObject start;
    public GameObject end;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (start != null && end != null)
        {
            lineRenderer.SetPosition(0, start.transform.position);
            lineRenderer.SetPosition(1, end.transform.position);
        }
    }
}