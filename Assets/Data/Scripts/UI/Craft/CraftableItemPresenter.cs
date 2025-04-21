using UnityEngine;
using static CraftData;

public class CraftableItemPresenter
{
    public CraftableItemView View { get; private set; }
    private ItemModel itemModel;

    public ItemModel ItemModel
    { get { return itemModel; } }

    public CraftableItemPresenter(CraftableItemView view)
    {
        View = view;
        itemModel = new ItemModel();
    }

    public void SetData(ItemCategory category, ItemDetail type, string name)
    {
        itemModel.Category = category;
        itemModel.Detail = type;
        itemModel.SetInfo(name, 1);
        View.SetData(name);
    }
}