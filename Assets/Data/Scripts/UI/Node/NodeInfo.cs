using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nodeDesc;
    [SerializeField] Button confirmButton;

    public Button ConfirmButton { get => confirmButton; }
    public TextMeshProUGUI NodeDesc { get => nodeDesc; set => nodeDesc.text = value.ToString(); }


}
