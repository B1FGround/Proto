using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectNodeType : MonoBehaviour
{
    enum NodeType
    {
        Common,
        Melee,
        Range,
        Magic,
        NodeTypeEnd,
    }
    [SerializeField] private Toggle[] toggles;

    [SerializeField] private GameObject[] nodes;
    private void Start()
    {
        foreach(var node in nodes)
        {
            node.SetActive(false);
        }
        nodes[0].SetActive(true);

        for (int i = 0; i < (int)NodeType.NodeTypeEnd; ++i)
        {
            int index = i;
            toggles[index].onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                {
                    foreach(var node in nodes)
                    {
                        node.SetActive(false);
                    }
                    nodes[index].SetActive(true);
                    nodes[index].gameObject.GetComponent<ScrollRect>().verticalScrollbar.value = 1f;
                }
            });
        }
    }
}
