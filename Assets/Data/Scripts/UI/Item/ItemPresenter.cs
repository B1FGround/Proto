using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemPresenter : ItemPresenterParent, IClickableUIPresenter
{
    public ItemPresenter() : base()
    {
    }

    public void OnClick()
    {
        view.SetTeamInfoViewItemData(model.ItemName);
    }
}