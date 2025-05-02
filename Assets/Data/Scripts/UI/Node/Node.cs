using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> parentNodes = new List<Node>();

    public IReadOnlyList<Node> ParentNodes { get => parentNodes; }
}
