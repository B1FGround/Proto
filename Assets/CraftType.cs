using UnityEngine;
using UnityEngine.UI;

public class CraftType : MonoBehaviour
{
    [SerializeField] private CraftUI.ItemType itemType = CraftUI.ItemType.None;
    [SerializeField] private Transform detailTypeTransform;
    [SerializeField] private Transform itemListTransform;
    [SerializeField] private CraftUI craftUI;

    private Button typeButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        typeButton = GetComponent<Button>();
        typeButton.onClick.AddListener(OnClickTypeButton);
        craftUI = GameObject.Find("CraftUI").GetComponent<CraftUI>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnClickTypeButton()
    {
        for (int i = 0; i < detailTypeTransform.childCount; i++)
            Destroy(detailTypeTransform.GetChild(i).gameObject);

        for (int i = 0; i < CraftData.detailType[(int)itemType].Length; i++)
        {
            GameObject detailCraftPrefab = Resources.Load<GameObject>("UI/CraftDetailType");
            var detailCraftObj = Instantiate(detailCraftPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            detailCraftObj.transform.SetParent(detailTypeTransform);
            detailCraftObj.GetComponent<CraftDetailType>().SetDetailTypeText(CraftData.detailType[(int)itemType][i]);
            detailCraftObj.GetComponent<CraftDetailType>().itemListTransform = itemListTransform;
        }
        ClearItemList();
        craftUI.selectedType = itemType;
    }

    private void ClearItemList()
    {
        for (int i = 0; i < itemListTransform.childCount; i++)
            Destroy(itemListTransform.GetChild(i).gameObject);
    }
}