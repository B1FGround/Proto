using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] public Image itemImage;
    [SerializeField] public TMP_Text itemCount;
    [SerializeField] private Button itemDetailButton;
    [SerializeField] public GameObject outLine;
    public string itemName;

    public void SetInfo(string name, int count, bool firstOpened = false, bool isTeamUI = false)
    {
        if (name.Equals(""))
        {
            outLine.SetActive(false);
            itemDetailButton.enabled = false;
            itemImage.gameObject.SetActive(false);
            itemCount.gameObject.SetActive(false);
            return;
        }
        itemDetailButton.enabled = true;
        itemName = name;
        itemCount.text = count.ToString();

        if (itemName.Contains(' '))
            itemImage.sprite = Resources.Load<Sprite>("UI/" + itemName.Split(' ')[1]);
        else
            itemImage.sprite = Resources.Load<Sprite>("UI/" + itemName);
        outLine.SetActive(false);
        if (count == 1)
            itemCount.gameObject.SetActive(false);

        if (isTeamUI == false)
            itemDetailButton.onClick.AddListener(OnClickItem);
        else
            itemDetailButton.onClick.AddListener(OnClickItemTeamUI);
        if (firstOpened)
            OnClickItem();
    }

    private void OnClickItem()
    {
        var invenUI = GameObject.Find("InventoryUI");
        var itemContainer = invenUI.GetComponent<UIInventory>().itemsContainor;

        for (int i = 0; i < itemContainer.childCount; ++i)
            itemContainer.GetChild(i).gameObject.GetComponent<InventoryItem>().outLine.SetActive(false);

        outLine.SetActive(true);

        invenUI.GetComponent<UIInventory>().itemDetailName.text = itemName;
        invenUI.GetComponent<UIInventory>().itemDetailDesc.text = "This is " + itemName;
        if (itemName.Contains(' '))
            invenUI.GetComponent<UIInventory>().itemDetailImage.sprite = Resources.Load<Sprite>("UI/" + itemName.Split(' ')[1]);
        else
            invenUI.GetComponent<UIInventory>().itemDetailImage.sprite = Resources.Load<Sprite>("UI/" + itemName);

        invenUI.GetComponent<UIInventory>().selectedItemName = itemName;
    }

    private void OnClickItemTeamUI()
    {
        var invenUI = GameObject.Find("TeamInfoUI");
        var itemContainer = invenUI.GetComponent<TeamInfoUI>().itemsContainor;

        for (int i = 0; i < itemContainer.childCount; ++i)
            itemContainer.GetChild(i).gameObject.GetComponent<InventoryItem>().outLine.SetActive(false);

        outLine.SetActive(true);

        invenUI.GetComponent<TeamInfoUI>().itemName.text = itemName;
        if (itemName.Contains(' '))
            invenUI.GetComponent<TeamInfoUI>().itemImage.sprite = Resources.Load<Sprite>("UI/" + itemName.Split(' ')[1]);
        else
            invenUI.GetComponent<TeamInfoUI>().itemImage.sprite = Resources.Load<Sprite>("UI/" + itemName);

        invenUI.GetComponent<TeamInfoUI>().selectedItemName = itemName;
    }
}