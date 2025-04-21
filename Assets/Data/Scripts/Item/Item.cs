using UnityEngine;

public class Item
{
    public ItemModel itemModel;

    public Item(string itemName, int itemCount)
    {
        itemModel = new ItemModel();
        itemModel.SetInfo(itemName, itemCount);
    }
}