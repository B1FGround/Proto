using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvenCategory : MonoBehaviour
{
    private Button categoryButton;
    [SerializeField] public CraftData.ItemCategory categoryType = CraftData.ItemCategory.None;
    [SerializeField] private Transform buttonsContainor;
    [SerializeField] private Transform itemsContainor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color originColor;

    private void Start()
    {
        categoryButton = GetComponent<Button>();
        categoryButton.onClick.AddListener(OnClickCategoryButton);
    }

    public void OnClickCategoryButton()
    {
        ChangeButtonColor();
        ClearItemsContainer();

        LoadItems(categoryType);
    }

    public void FirstOpen()
    {
        ChangeButtonColor();
        ClearItemsContainer();

        LoadItems(categoryType, true);
    }

    private void ChangeButtonColor()
    {
        for (int i = 0; i < buttonsContainor.childCount; ++i)
        {
            if (buttonsContainor.GetChild(i).gameObject == this.gameObject)
                buttonsContainor.GetChild(i).gameObject.GetComponent<Image>().color = selectedColor;
            else
                buttonsContainor.GetChild(i).gameObject.GetComponent<Image>().color = originColor;
        }
    }

    private void ClearItemsContainer()
    {
        for (int i = 0; i < itemsContainor.childCount; ++i)
            Destroy(itemsContainor.GetChild(i).gameObject);
    }

    public void LoadItems(CraftData.ItemCategory type = CraftData.ItemCategory.None, bool firstOpened = false)
    {
        var itemList = InventoryManager.Instance.GetItemsByCategory(type);
        var uiInven = GameObject.Find("InventoryUI").GetComponent<UIInventory>();

        // 빈 컨테이너 생성
        for (int i = 0; i < 32 - itemList.Count; ++i)
        {
            var invenItem = Instantiate(Resources.Load("UI/InvenItem")) as GameObject;
            invenItem.GetComponent<InventoryItem>().SetInfo("", 0);
            invenItem.transform.SetParent(itemsContainor);
        }

        for (int i = 0; i < itemList.Count; ++i)
        {
            string itemName = itemList[i].itemModel.ItemName;
            int itemCount = itemList[i].itemModel.ItemCount;

            var invenItem = Instantiate(Resources.Load("UI/InvenItem")) as GameObject;

            if (firstOpened == true)
            {
                if (i == 0)
                {
                    invenItem.GetComponent<InventoryItem>().SetInfo(itemName, itemCount, true);
                    invenItem.transform.SetParent(itemsContainor);
                    invenItem.transform.SetSiblingIndex(i);
                    if (invenItem.GetComponent<InventoryItem>().itemName.Equals(uiInven.selectedItemName))
                        invenItem.GetComponent<InventoryItem>().outLine.SetActive(true);
                    continue;
                }
            }
            invenItem.GetComponent<InventoryItem>().SetInfo(itemName, itemCount);
            invenItem.transform.SetParent(itemsContainor);
            invenItem.transform.SetSiblingIndex(i);
            if (invenItem.GetComponent<InventoryItem>().itemName.Equals(uiInven.selectedItemName))
                invenItem.GetComponent<InventoryItem>().outLine.SetActive(true);
        }
    }
}