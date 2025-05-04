using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.UI.Extensions;

public class NodeLines : MonoBehaviour
{
    [SerializeField] LineCreator baseLine;
    LineInfo[] lines;
    void Start()
    {
        baseLine.SetBaseLine();
        lines = baseLine.transform.GetComponentsInChildren<LineInfo>();

    }
    public void OnClickNode(bool selected, List<Node> parentNodes, Node childNode)
    {
        foreach (var parentNode in parentNodes)
        {
            var lineInfo = lines.FirstOrDefault(line => line.parent == parentNode && line.child == childNode);
            if (lineInfo != null)
            {
                if (selected == true)
                    lineInfo.gameObject.GetComponent<UILineRenderer>().color = Color.red;
                else
                    lineInfo.gameObject.GetComponent<UILineRenderer>().color = Color.white;
            }
        }
    }

    public bool CheckChildNodeSelected(Node node)
    {
        return lines.Any(line => line.parent == node && line.child.Selected == true);
    }
}
