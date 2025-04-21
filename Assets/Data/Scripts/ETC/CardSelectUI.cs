using UnityEngine;
using UnityEngine.UI;
using static CardInfo;

public class CardSelectUI : UIView
{
    [SerializeField] private Button confirmBtn;
    [SerializeField] public GameObject[] cards;

    public GameObject selectedCard = null;
    private TeamContainer teamContainer;

    private void OnEnable()
    {
        Time.timeScale = 0f;
        GameManager.Instance.StopGameTimer();
    }

    public override void Open()
    {
    }

    private void Start()
    {
        for (int i = 0; i < cards.Length; i++)
            cards[i].GetComponent<CardInfo>().cardType = (CardInfo.CardType)i;

        confirmBtn.onClick.AddListener(onClickConfirm);
        teamContainer = GameObject.FindWithTag("Player").GetComponent<TeamContainer>();
    }

    private void onClickConfirm()
    {
        if (selectedCard == null || teamContainer == null)
            return;

        foreach (var card in cards)
            card.transform.Find("Selected").gameObject.SetActive(false);

        foreach (var character in teamContainer.GetCharacters())
        {
            character.GetComponent<CharacterAttack>().AddPower(selectedCard.GetComponent<CardInfo>().cardType);
        }
        selectedCard = null;

        Time.timeScale = 1f;

        this.gameObject.SetActive(false);
        GameManager.Instance.StartGameTimer();
    }
}