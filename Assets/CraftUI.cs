using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    public enum ItemType
    {
        Common,
        Armor,
        None,
    }

    public enum DetailType
    {
        Drum,
        Stone,
        Cube,
        Helmet,
        Armor,
        Sword,
        Glove,
        Shoulder,
        Bottom,
        Leg,
        None,
    }

    public ItemType selectedType = ItemType.None;

    public struct Ingredient
    {
        public ItemType type;
        public string ItemName;
        public (string, int)[] Ingredients;
    }

    public static string[][] ItemList = new string[][]
    {
        new string[] { "Red Drum", "Green Drum", "Blue Drum" },
        new string[] { "Red Stone", "Green Stone", "Blue Stone" },
        new string[] { "Red Cube", "Green Cube", "Blue Cube", "Black Cube" },
        new string[] { "Red Helmet", "Green Helmet", "Blue Helmet" },
        new string[] { "Red Armor", "Green Armor", "Blue Armor" },
        new string[] { "Red Sword", "Green Sword", "Blue Sword", "Black Sword", "White Sword" },
        new string[] { "Red Glove", "Green Glove", "Blue Glove" },
        new string[] { "Red Shoulder", "Green Shoulder", "Blue Shoulder" },
        new string[] { "Red Bottom", "Green Bottom", "Blue Bottom" },
        new string[] { "Red Leg", "Green Leg", "Blue Leg" },

        new string[] { "None", "None", "None" },
    };

    public static string[][] detailType = new string[][]
    {
        new string[] { "Drum", "Stone", "Cube" },
        new string[] { "Helmet", "Armor", "Bottom", "Leg" },
        new string[] { "No item", "No item", "No item" },
    };

    public static Ingredient[] craft = new Ingredient[]
    {
        new Ingredient { type = ItemType.Common, ItemName = "Red Drum", Ingredients = new (string, int)[] { ("Stone", 2), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Green Drum", Ingredients = new (string, int)[] { ("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Blue Drum", Ingredients = new (string, int)[] { ("Stone", 5), ("Cube", 1) } },

        new Ingredient { type = ItemType.Common, ItemName = "Red Stone", Ingredients = new (string, int)[] {("Stone", 7), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Green Stone", Ingredients = new (string, int)[] { ("Drum", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Blue Stone", Ingredients = new (string, int)[] {("Stone", 2), ("Cube", 1) } },

        new Ingredient { type = ItemType.Common, ItemName = "Red Cube", Ingredients = new (string, int)[] { ("Drum", 5), ("Stone", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Green Cube", Ingredients = new (string, int)[] {("Stone", 6), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Blue Cube", Ingredients = new (string, int)[] {("Stone", 2), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Black Cube", Ingredients = new (string, int)[] { ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Helmet", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 3) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Helmet", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 6) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Helmet", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 2) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Armor", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Armor", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 2) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Armor", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 4) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Black Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "White Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Glove", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Glove", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Glove", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Shoulder", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Shoulder", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Shoulder", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Bottom", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Bottom", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Bottom", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Leg", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Leg", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Leg", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
    };

    [SerializeField] private TMP_Text itemName;
    [SerializeField] public Image itemImg;
    [SerializeField] private TMP_Text craftableCount;
    [SerializeField] private Transform itemListTransform;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button craftButton;
    [SerializeField] private Button addItemsButton;
    [SerializeField] private Button quitButton;

    // key: item name, value: (current count, origin count)
    private Dictionary<string, (int, int, ItemType)> craftableItem = new Dictionary<string, (int, int, ItemType)>();

    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.Instance;
        minusButton.onClick.AddListener(OnClickMinusButton);
        plusButton.onClick.AddListener(OnClickPlusButton);
        craftButton.onClick.AddListener(OnClickCraftButton);
        addItemsButton.onClick.AddListener(OnClickAddItemsButton);
        quitButton.onClick.AddListener(() => gameObject.SetActive(false));
        craftableCount.text = "0";
    }

    public void ShowCraft(string itemName)
    {
        for (int i = 0; i < itemListTransform.childCount; i++)
            Destroy(itemListTransform.GetChild(i).gameObject);

        craftableItem.Clear();

        foreach (var item in craft)
        {
            if (item.ItemName.Equals(itemName))
            {
                this.itemName.text = item.ItemName;
                for (int i = 0; i < item.Ingredients.Length; i++)
                {
                    GameObject detailCraftPrefab = Resources.Load<GameObject>("UI/CraftIngredient");
                    var detailCraftObj = Instantiate(detailCraftPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    detailCraftObj.transform.SetParent(itemListTransform);
                    detailCraftObj.GetComponent<CraftIngredient>().itemName.text = item.Ingredients[i].Item1;
                    detailCraftObj.GetComponent<CraftIngredient>().count.text = inventory.GetItemCount(item.Ingredients[i].Item1).ToString() + "/" + item.Ingredients[i].Item2.ToString();
                    detailCraftObj.GetComponent<CraftIngredient>().SetImage();

                    craftableItem.Add(item.Ingredients[i].Item1, (item.Ingredients[i].Item2, item.Ingredients[i].Item2, item.type));
                }

                bool value = true;
                foreach (var key in craftableItem.Keys)
                    value &= inventory.CheckItemCount(key, craftableItem[key].Item1);

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

            if (!inventory.CheckItemCount(key, newValue))
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
                    itemListTransform.GetChild(i).GetComponent<CraftIngredient>().count.text = inventory.GetItemCount(key).ToString() + "/" + craftableItem[key].Item1.ToString();
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
            Inventory.Instance.AddItem(key, value.Item3, -value.Item1);
        }

        inventory.AddItem(itemName.text, selectedType, int.Parse(craftableCount.text));

        bool check = true;
        foreach (var key in craftableItem.Keys)
            check &= inventory.CheckItemCount(key, craftableItem[key].Item1);
        if (check == false)
            craftableCount.text = "0";

        UpdateIngredientCount();
    }

    public void SetImage()
    {
        itemImg.sprite = Resources.Load<Sprite>("UI/" + itemName.text.Split(' ')[1]);
    }

    private void OnClickAddItemsButton()
    {
        inventory.AddAllIngredient(100);
        craftableCount.text = "1";
        craftButton.enabled = true;

        UpdateIngredientCount();
    }
}