using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CraftData;

public class CraftDetailType : MonoBehaviour
{
    [SerializeField] private TMP_Text detailTypeText;
    public Transform itemListTransform;

    private Button detailTypeButton;
    public ItemCategory itemCategory;
    public ItemDetail itemType = ItemDetail.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        detailTypeButton = GetComponent<Button>();
        detailTypeButton.onClick.AddListener(OnClickDetailTypeButton);
    }

    public void SetDetailTypeText(string detailType)
    {
        detailTypeText.text = detailType;

        if (detailType.Equals("No item"))
            return;
        itemType = (ItemDetail)Enum.Parse(typeof(ItemDetail), detailType);
    }

    private void OnClickDetailTypeButton()
    {
        for (int i = 0; i < itemListTransform.childCount; i++)
            Destroy(itemListTransform.GetChild(i).gameObject);

        if (itemType == ItemDetail.None)
            return;

        for (int i = 0; i < ItemList[(int)itemType].Length; i++)
        {
            GameObject detailCraftPrefab = Resources.Load<GameObject>("Prefabs/UI/CraftItem");
            var detailCraftObj = Instantiate(detailCraftPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            detailCraftObj.transform.SetParent(itemListTransform);
            detailCraftObj.GetComponent<CraftableItemView>().SetPresenter(new CraftableItemPresenter(detailCraftObj.GetComponent<CraftableItemView>()));
            detailCraftObj.GetComponent<CraftableItemView>().SetData(itemCategory, itemType, ItemList[(int)itemType][i]);

            //detailCraftObj.GetComponent<CraftableItem>().itemName.text = CraftUI.ItemList[(int)itemType][i];
            //detailCraftObj.GetComponent<CraftableItem>().SetImage();
        }
    }
}