using UnityEngine;
using UnityEngine.UI;
using static CraftData;

public class CraftCategoryView : MonoBehaviour
{
    [SerializeField] private ItemCategory itemType = ItemCategory.None;
    [SerializeField] private Transform detailTypeTransform;
    [SerializeField] private Transform itemListTransform;
    [SerializeField] private CraftView craftUI;

    private Button categoryButton;
    private CraftCategoryPresenter presenter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        categoryButton = GetComponent<Button>();
        categoryButton.onClick.AddListener(OnClickTypeButton);
        presenter = new CraftCategoryPresenter(this, itemType, craftUI);
    }

    private void OnClickTypeButton()
    {
        presenter.OnClickCategory();
    }

    public void AddDetailTypes()
    {
        for (int i = 0; i < CraftData.detailType[(int)itemType].Length; i++)
        {
            GameObject detailCraftPrefab = Resources.Load<GameObject>("Prefabs/UI/CraftDetailType");
            var detailCraftObj = Instantiate(detailCraftPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            detailCraftObj.transform.SetParent(detailTypeTransform, false);
            detailCraftObj.GetComponent<CraftDetailType>().SetDetailTypeText(CraftData.detailType[(int)itemType][i]);
            detailCraftObj.GetComponent<CraftDetailType>().itemListTransform = itemListTransform;
            detailCraftObj.GetComponent<CraftDetailType>().itemCategory = itemType;
        }
    }

    public void ClearItemList()
    {
        for (int i = 0; i < itemListTransform.childCount; i++)
            Destroy(itemListTransform.GetChild(i).gameObject);
    }

    public void ClearDetailTypes()
    {
        foreach (Transform child in detailTypeTransform)
            Destroy(child.gameObject);
    }
}