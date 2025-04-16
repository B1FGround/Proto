using UnityEngine;
using static CraftData;

public class ItemModel
{
    public ItemCategory Category { get; set; }
    public ItemDetail Detail { get; set; }

    private string itemName;
    private int itemCount;

    public string ItemName
    { get { return itemName; } }

    public int ItemCount
    { get { return itemCount; } }

    public (string, string) SetInfo(string itemName, int itemCount)
    {
        this.itemName = itemName;
        this.itemCount = itemCount;

        return (itemName, itemCount.ToString());
    }
}