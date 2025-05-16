using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CraftData;

public class CraftView : UIView
{
    public ItemCategory selectedType = ItemCategory.None;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;

    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;

    [SerializeField] private TMP_Text craftableCount;

    [SerializeField] private Button craftButton;
    [SerializeField] private Button addItemButton;
    [SerializeField] private Transform itemListTransform;
    private CraftPresenter craftPresenter;

    private Dictionary<string, (int, int, ItemCategory)> craftableItem = new Dictionary<string, (int, int, ItemCategory)>();

    private void Start()
    {
        quitButton.onClick.AddListener(Close);
        minusButton.onClick.AddListener(OnClickMinusButton);
        plusButton.onClick.AddListener(OnClickPlusButton);
        craftButton.onClick.AddListener(OnClickCraftButton);
        addItemButton.onClick.AddListener(OnClickAddItemsButton);
        craftPresenter = new CraftPresenter(this);
    }

    public override void Open()
    {
    }

    public void ShowCraftInfo(ItemModel itemModel)
    {
        craftPresenter.ShowCraftInfo(itemModel.ItemName, itemModel.ItemCount, itemModel.Category, itemModel.Detail);

        for (int i = 0; i < itemListTransform.childCount; i++)
            Destroy(itemListTransform.GetChild(i).gameObject);

        craftableItem.Clear();

        foreach (var item in CraftData.craft)
        {
            if (item.ItemName.Equals(craftPresenter.GetItemData().ItemName))
            {
                this.itemName.text = item.ItemName;
                for (int i = 0; i < item.Ingredients.Length; i++)
                {
                    GameObject detailCraftPrefab = Resources.Load<GameObject>("Prefabs/UI/CraftIngredient");
                    var detailCraftObj = Instantiate(detailCraftPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    detailCraftObj.transform.SetParent(itemListTransform);
                    detailCraftObj.GetComponent<CraftIngredient>().itemName.text = item.Ingredients[i].Item1;
                    detailCraftObj.GetComponent<CraftIngredient>().count.text = InventoryManager.Instance.GetItemCount(item.Ingredients[i].Item1).ToString() + "/" + item.Ingredients[i].Item2.ToString();
                    detailCraftObj.GetComponent<CraftIngredient>().SetImage();

                    craftableItem.Add(item.Ingredients[i].Item1, (item.Ingredients[i].Item2, item.Ingredients[i].Item2, item.type));
                }

                bool value = true;
                foreach (var key in craftableItem.Keys)
                    value &= InventoryManager.Instance.CheckItemCount(key, craftableItem[key].Item1);

                if (value == false)
                {
                    craftableCount.text = "0";
                    craftButton.enabled = false;
                }
                else
                {
                    craftableCount.text = "1";
                    craftButton.enabled = true;
                }

                return;
            }
        }
    }

    private void OnClickMinusButton()
    {
        if (craftableCount.text.Equals("0"))
            return;
        craftableCount.text = (int.Parse(craftableCount.text) - 1).ToString();

        var keys = craftableItem.Keys.ToList();
        foreach (var key in keys)
        {
            var value = craftableItem[key];
            value.Item1 -= value.Item2;
            craftableItem[key] = value;
        }
        UpdateIngredientCount();
    }

    private void OnClickPlusButton()
    {
        var keys = craftableItem.Keys.ToList();
        foreach (var key in keys)
        {
            var value = craftableItem[key];
            var newValue = value.Item1 += value.Item2;

            if (!InventoryManager.Instance.CheckItemCount(key, newValue))
                return;

            value.Item1 = newValue;
            craftableItem[key] = value;
        }

        craftableCount.text = (int.Parse(craftableCount.text) + 1).ToString();

        UpdateIngredientCount();
    }

    public void UpdateIngredientCount()
    {
        var keys = craftableItem.Keys.ToList();
        foreach (var key in keys)
        {
            var value = craftableItem[key];

            value.Item1 = int.Parse(craftableCount.text) * value.Item2;
            craftableItem[key] = value;
        }
        for (int i = 0; i < itemListTransform.childCount; ++i)
        {
            foreach (var key in craftableItem.Keys)
            {
                if (itemListTransform.GetChild(i).GetComponent<CraftIngredient>() != null && itemListTransform.GetChild(i).GetComponent<CraftIngredient>().itemName.text.Equals(key))
                {
                    itemListTransform.GetChild(i).GetComponent<CraftIngredient>().count.text = InventoryManager.Instance.GetItemCount(key).ToString() + "/" + craftableItem[key].Item1.ToString();
                    break;
                }
            }
        }
    }

    private void OnClickCraftButton()
    {
        if (craftableCount.text.Equals("0"))
            return;

        var keys = craftableItem.Keys.ToList();
        foreach (var key in keys)
        {
            var value = craftableItem[key];
            InventoryManager.Instance.AddItem(key, -value.Item1);
        }

        var itemData = craftPresenter.GetItemData();
        InventoryManager.Instance.AddItem(itemData.ItemName, int.Parse(craftableCount.text), itemData.Detail);

        bool check = true;
        foreach (var key in craftableItem.Keys)
            check &= InventoryManager.Instance.CheckItemCount(key, craftableItem[key].Item1);
        if (check == false)
            craftableCount.text = "0";

        UpdateIngredientCount();
    }

    private void OnClickAddItemsButton()
    {
        InventoryManager.Instance.AddAllIngredient(100);
        craftableCount.text = "1";
        craftButton.enabled = true;

        UpdateIngredientCount();
    }

    public void SetData(string name, int count)
    {
        itemName.text = name;
        itemImage.sprite = Resources.Load<Sprite>("Prefabs/Items/" + name.Split(' ')[1]);
        craftableCount.text = count.ToString();
    }
}