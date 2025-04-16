using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject[] characterButtons;

    [SerializeField] private Button changeLeaderButton;
    [SerializeField] private Button addTeamButton;
    [SerializeField] private Button closeButton;

    [SerializeField] public Image itemImage;
    [SerializeField] public TMP_Text itemName;

    [SerializeField] public Transform itemsContainor;
    [SerializeField] public Button equipButton;

    public string selectedItemName = "";
    private TeamManager.CharacterType selectedCharacter;

    private void Start()
    {
        characterButtons[(int)TeamManager.CharacterType.Warrior].GetComponent<Button>().onClick.AddListener(OnClickWarriorButton);
        characterButtons[(int)TeamManager.CharacterType.Ranger].GetComponent<Button>().onClick.AddListener(OnClickRangerButton);
        characterButtons[(int)TeamManager.CharacterType.Magician].GetComponent<Button>().onClick.AddListener(OnClickMagicianButton);
        changeLeaderButton.onClick.AddListener(OnClickChangeLeaderButton);
        addTeamButton.onClick.AddListener(OnClickAddTeamButton);
        closeButton.onClick.AddListener(OnClickQuitButton);
        equipButton.onClick.AddListener(OnClickEquipButton);

        OnClickRangerButton();
    }

    private void OnClickWarriorButton()
    {
        ClearSelectedCharacter();
        characterButtons[(int)TeamManager.CharacterType.Warrior].transform.Find("Selected").gameObject.SetActive(true);
        GetArmors();
        selectedCharacter = TeamManager.CharacterType.Warrior;
    }

    public void OnClickRangerButton()
    {
        ClearSelectedCharacter();
        characterButtons[(int)TeamManager.CharacterType.Ranger].transform.Find("Selected").gameObject.SetActive(true);
        GetArmors();
        selectedCharacter = TeamManager.CharacterType.Ranger;
    }

    private void OnClickMagicianButton()
    {
        ClearSelectedCharacter();
        characterButtons[(int)TeamManager.CharacterType.Magician].transform.Find("Selected").gameObject.SetActive(true);
        GetArmors();
        selectedCharacter = TeamManager.CharacterType.Magician;
    }

    private void OnClickChangeLeaderButton()
    {
        TeamManager.Instance.SetLeader(selectedCharacter);
        GameObject.FindWithTag("Player").GetComponent<TeamContainer>().SetTeam(TeamManager.Instance.GetTeamCount());
    }

    private void OnClickAddTeamButton()
    {
        TeamManager.Instance.AddTeam(selectedCharacter);
        GameObject.FindWithTag("Player").GetComponent<TeamContainer>().SetTeam(TeamManager.Instance.GetTeamCount());
    }

    public void OnClickQuitButton()
    {
        this.gameObject.SetActive(false);
    }

    private void ClearSelectedCharacter()
    {
        for (int i = 0; i < characterButtons.Length; ++i)
        {
            characterButtons[i].transform.Find("Selected").gameObject.SetActive(false);
        }
    }

    private void GetArmors()
    {
        ClearItemContainer();
        selectedItemName = "";
        var itemList = Inventory.Instance.GetItemsByType(CraftUI.ItemType.Armor);

        // 빈 컨테이너 생성
        for (int i = 0; i < 15 - itemList.Count; ++i)
        {
            var invenItem = Instantiate(Resources.Load("UI/InvenItem")) as GameObject;
            invenItem.GetComponent<InventoryItem>().SetInfo("", 0);
            invenItem.transform.SetParent(itemsContainor);
        }

        for (int i = 0; i < itemList.Count; ++i)
        {
            string itemName = itemList[i].Item1;
            int itemCount = itemList[i].Item2;

            var invenItem = Instantiate(Resources.Load("UI/InvenItem")) as GameObject;
            invenItem.GetComponent<InventoryItem>().SetInfo(itemName, itemCount, false, true);
            invenItem.transform.SetParent(itemsContainor);
            invenItem.transform.SetSiblingIndex(i);
            invenItem.GetComponent<InventoryItem>().outLine.SetActive(false);
        }
    }

    private void ClearItemContainer()
    {
        for (int i = 0; i < itemsContainor.childCount; ++i)
            Destroy(itemsContainor.GetChild(i).gameObject);
    }

    private void OnClickEquipButton()
    {
        if (string.IsNullOrEmpty(selectedItemName))
            return;

        //var itemTypeAndName = Inventory.Instance.GetDetailType(selectedItemName);
        //Inventory.Instance.Equip(selectedCharacter, (itemTypeAndName, selectedItemName));
        //var characters = GameObject.FindWithTag("Player").GetComponent<TeamContainer>().GetCharacters();
        //foreach (var character in characters)
        //{
        //    character.GetComponent<CharacterAttack>().Equip(selectedCharacter);
        //}
        //GetArmors();
    }
}