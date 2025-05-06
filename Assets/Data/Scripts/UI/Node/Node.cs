using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Extensions.ContentScrollSnapHorizontal;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> parentNodes = new List<Node>();
    [SerializeField] NodeLines nodeLines;
    [SerializeField] StatUI statUI;

    bool selected = false;
    public bool Selected { get => selected; private set => selected = value; }

    public IReadOnlyList<Node> ParentNodes { get => parentNodes; }


    HashSet<string> descSet = new HashSet<string>() { "attack + 1", "attack + 3", "speed + 1", "speed + 3" };
    string desc;

    private void Start()
    {
        var button = GetComponent<Button>();
        desc = descSet.ElementAt(Random.Range(0, descSet.Count));

        button.onClick.AddListener(() =>
        {
            statUI.nodeInfo.SetActive(true);
            statUI.nodeInfo.transform.parent = this.transform;

            statUI.nodeInfo.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            statUI.nodeInfo.GetComponent<NodeInfo>().NodeDesc.text = desc;
            statUI.nodeInfo.GetComponent<NodeInfo>().ConfirmButton.onClick.RemoveAllListeners();
            statUI.nodeInfo.GetComponent<NodeInfo>().ConfirmButton.onClick.AddListener(() =>
            {
                if(selected == false)
                {
                    if (nodeLines.CheckParentNodeSelected(this) == false)
                        return;
                }
                if (nodeLines.CheckChildNodeSelected(this))
                    return;

                selected = !selected;

                List<Node> selectedParentNodes = new List<Node>();
                foreach (var parentNode in parentNodes)
                {
                    if (parentNode.selected)
                        selectedParentNodes.Add(parentNode);
                }
                nodeLines.OnClickNode(selected, selectedParentNodes, this);

                statUI.nodeInfo.SetActive(false);
            });
        });
    }
}
