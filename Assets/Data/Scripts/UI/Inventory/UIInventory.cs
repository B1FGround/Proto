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
    public void OnClickItem(string name)
    {
        for (int i = 0; i < itemsContainor.childCount; ++i)
            itemsContainor.GetChild(i).gameObject.GetComponent<ItemView>().outLine.SetActive(false);

        itemDetailName.text = name;
        if (name.Contains(' '))
            itemDetailImage.sprite = Resources.Load<Sprite>("Prefabs/Items/" + name.Split(' ')[1]);
        else
            itemDetailImage.sprite = Resources.Load<Sprite>("Prefabs/Items/" + name);

        selectedItemName = name;
        itemDetailDesc.text = "This is " + name;
    }
}