using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    public enum CardType
    {
        Damage,
        AttackSpeed,
        AdditionalAttack
    }

    [SerializeField] private CardSelectUI cardSelectUI;

    private Button cardButton;
    public GameObject selected;
    public CardType cardType;

    private void Start()
    {
        cardButton = GetComponent<Button>();
        cardButton.onClick.AddListener(OnClickCard);
    }

    private void OnClickCard()
    {
        foreach (var card in cardSelectUI.cards)
            card.transform.Find("Selected").gameObject.SetActive(false);

        selected.SetActive(true);
        cardSelectUI.selectedCard = this.gameObject;
    }
}