using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CraftData;

public class CraftableItemView : MonoBehaviour
{
    [SerializeField] public TMP_Text itemName;
    [SerializeField] public Image itemImg;

    private Button itemButton;
    private CraftView craftUI = null;

    private CraftableItemPresenter presenter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        craftUI = GameObject.Find("CraftUI").GetComponent<CraftView>();
        itemButton = GetComponent<Button>();
        itemButton.onClick.AddListener(OnClickItemButton);
    }

    public void SetPresenter(CraftableItemPresenter presenter)
    {
        this.presenter = presenter;
    }

    public void SetData(ItemCategory category, ItemDetail type, string name)
    {
        presenter.SetData(category, type, name);
    }

    public void SetData(string name)
    {
        itemName.text = name;
        SetImage();
    }

    private void OnClickItemButton()
    {
        craftUI.ShowCraftInfo(presenter.ItemModel);
    }

    public void SetImage()
    {
        itemImg.sprite = Resources.Load<Sprite>("UI/" + itemName.text.Split(' ')[1]);
    }
}