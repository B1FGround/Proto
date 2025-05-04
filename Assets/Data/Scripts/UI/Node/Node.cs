using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> parentNodes = new List<Node>();
    [SerializeField] NodeLines nodeLines;

    bool selected = false;
    public bool Selected { get => selected; private set => selected = value; }

    public IReadOnlyList<Node> ParentNodes { get => parentNodes; }

    private void Start()
    {
        var button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            if (nodeLines.CheckChildNodeSelected(this))
                return;

            selected = !selected;

            List<Node> selectedParentNodes = new List<Node>();
            foreach (var parentNode in parentNodes)
            {
                if(parentNode.selected)
                    selectedParentNodes.Add(parentNode);
            }
            nodeLines.OnClickNode(selected, selectedParentNodes, this);
        });
    }
}
