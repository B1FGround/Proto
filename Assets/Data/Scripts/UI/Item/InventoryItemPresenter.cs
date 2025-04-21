using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventoryItemPresenter : ItemPresenterParent, IClickableUIPresenter
{
    public InventoryItemPresenter() : base()
    {
    }

    public void OnClick()
    {
        view.SetInventoryViewitenData(model.ItemName);
    }
}