using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftableItem : MonoBehaviour
{
    [SerializeField] public TMP_Text itemName;
    [SerializeField] public Image itemImg;

    private Button itemButton;
    private CraftUI craftUI = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        craftUI = GameObject.Find("CraftUI").GetComponent<CraftUI>();
        itemButton = GetComponent<Button>();
        itemButton.onClick.AddListener(OnClickItemButton);
    }

    private void OnClickItemButton()
    {
        craftUI.ShowCraft(itemName.text);
        craftUI.SetImage();
    }

    public void SetImage()
    {
        itemImg.sprite = Resources.Load<Sprite>("UI/" + itemName.text.Split(' ')[1]);
    }
}