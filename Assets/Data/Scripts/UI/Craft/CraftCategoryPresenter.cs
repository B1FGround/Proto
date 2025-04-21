using static CraftData;

public class CraftCategoryPresenter
{
    private CraftCategoryView view;
    private ItemCategory category;
    private CraftView craftUI;

    public CraftCategoryPresenter(CraftCategoryView view, ItemCategory category, CraftView craftUI)
    {
        this.view = view;
        this.category = category;
        this.craftUI = craftUI;
    }

    public void OnClickCategory()
    {
        view.ClearDetailTypes();
        view.AddDetailTypes();
        view.ClearItemList();
        craftUI.selectedType = category;
    }
}