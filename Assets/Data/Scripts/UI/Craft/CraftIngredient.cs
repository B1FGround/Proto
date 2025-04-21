using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftIngredient : MonoBehaviour
{
    [SerializeField] public TMP_Text itemName;
    [SerializeField] public TMP_Text count;
    [SerializeField] public Image itemImg;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void SetImage()
    {
        itemImg.sprite = Resources.Load<Sprite>("Prefabs/Items/" + itemName.text);
    }
}