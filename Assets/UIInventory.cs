using JH.DataBinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : UIView
{
    [SerializeField] private Button quitButton;
    [SerializeField] public Image itemDetailImage;
    [SerializeField] public TMP_Text itemDetailName;
    [SerializeField] public TMP_Text itemDetailDesc;
    [SerializeField] public Transform itemsContainor;
    [SerializeField] private GameObject[] typeButtons;

    public string selectedItemName;

    private void Start()
    {
        quitButton.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }

    public override void Open()
    {
    }

    private void OnEnable()
    {
        typeButtons[0].GetComponent<InvenCategory>().FirstOpen();
    }
}