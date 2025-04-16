using UnityEngine;

public class CraftPresenter
{
    public CraftView View { get; private set; }
    private InventoryModel inventoryModel;
    private ItemModel selectedItem;

    public CraftPresenter(CraftView view)
    {
        View = view;
        inventoryModel = new InventoryModel();
        selectedItem = new ItemModel();
    }

    public void ShowCraftInfo(string name, int count, CraftData.ItemCategory category, CraftData.ItemDetail itemDetail)
    {
        selectedItem.SetInfo(name, count);
        selectedItem.Category = category;
        selectedItem.Detail = itemDetail;
        View.SetData(selectedItem.ItemName, selectedItem.ItemCount);
    }

    public void SetData(int count)
    {
        selectedItem.SetInfo(selectedItem.ItemName, count);
    }

    public ItemModel GetItemData()
    {
        return selectedItem;
    }
}