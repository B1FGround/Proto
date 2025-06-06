using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LineCreator : MonoBehaviour
{
    [SerializeField] List<GameObject> nodeObjects;

    public void SetBaseLine()
    {
        Canvas.ForceUpdateCanvases();
        float rootXPos = this.transform.parent.GetComponent<RectTransform>().anchoredPosition.x * -1f;

        foreach (var nodeObject in nodeObjects)
        {
            var node = nodeObject.GetComponent<Node>();

            GameObject lineObject = null;
            List<Vector2> linePoints = new List<Vector2>();

            foreach (var parentNode in node.ParentNodes)
            {
                lineObject = new GameObject("Line");
                lineObject.transform.SetParent(this.transform);
                lineObject.AddComponent<UILineRenderer>();
                lineObject.AddComponent<LineInfo>();
                lineObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(rootXPos, 0f);
                linePoints.Add(new Vector2(parentNode.gameObject.GetComponent<RectTransform>().anchoredPosition.x, parentNode.transform.parent.GetComponent<RectTransform>().anchoredPosition.y));
                linePoints.Add(new Vector2(nodeObject.GetComponent<RectTransform>().anchoredPosition.x, nodeObject.transform.parent.GetComponent<RectTransform>().anchoredPosition.y));

                lineObject.GetComponent<LineInfo>().parent = parentNode;
                lineObject.GetComponent<LineInfo>().child = node;
            }
            if (lineObject != null)
            {
                var lineRenderer = lineObject.GetComponent<UILineRenderer>();
                lineRenderer.Points = linePoints.ToArray();
                lineRenderer.LineThickness = 10f;
                lineRenderer.SetAllDirty();
            }
        }
    }
}
