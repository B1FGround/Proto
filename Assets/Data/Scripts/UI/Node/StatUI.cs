using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    [SerializeField] public GameObject nodeInfo;
    [SerializeField] Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        quitButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
