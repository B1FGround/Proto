using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    [SerializeField] private Button[] buildButtons;
    [SerializeField] private Button removeButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var placementSystem = GameObject.Find("PlacementSystem");
        for (int i = 0; i < buildButtons.Length; i++)
        {
            int index = i;
            buildButtons[i].onClick.AddListener(() =>
            {
                placementSystem.GetComponent<PlacementSystem>().StartPlacement(index);
            });
        }
        removeButton.onClick.AddListener(() => placementSystem.GetComponent<PlacementSystem>().StartRemoving());
    }
}