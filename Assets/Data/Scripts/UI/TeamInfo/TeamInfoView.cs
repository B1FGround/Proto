using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamInfoView : UIView
{
    [SerializeField] private TMP_Text viewName;
    [SerializeField] private Button closeButton;

    [SerializeField] private GameObject[] characterButtons;
    [SerializeField] private Button changeLeaderButton;
    [SerializeField] private Button addTeamButton;

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;

    [SerializeField] private Transform itemsContainer;
    [SerializeField] private Button equipButton;

    private TeamInfoPresenter presenter;
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
    }

    public override void Open()
    {
    }

    public void SetPresenter(TeamInfoPresenter presenter)
    {
        if (this.presenter == null)
        {
            this.presenter = presenter;
            this.presenter.view = this;
        }
    }

    public void OnClickQuitButton() => this.gameObject.SetActive(false);

    private void OnClickWarriorButton()
    {
        presenter.SelectCharacter(TeamManager.CharacterType.Warrior);
    }

    public void OnClickRangerButton()
    {
        presenter.SelectCharacter(TeamManager.CharacterType.Ranger);
    }

    private void OnClickMagicianButton()
    {
        presenter.SelectCharacter(TeamManager.CharacterType.Magician);
    }

    public void HighlightCharacter(TeamManager.CharacterType charcterType)
    {
        selectedCharacter = charcterType;

        characterButtons[(int)charcterType].transform.Find("Selected").gameObject.SetActive(true);
        presenter.GetArmors();
    }

    public void ClearSelectedCharacter()
    {
        for (int i = 0; i < characterButtons.Length; ++i)
        {
            characterButtons[i].transform.Find("Selected").gameObject.SetActive(false);
        }
    }

    private void ClearItemContainer()
    {
        for (int i = 0; i < itemsContainer.childCount; ++i)
            Destroy(itemsContainer.GetChild(i).gameObject);
    }

    public void ArmorsInitialize(List<GameObject> armors)
    {
        ClearItemContainer();
        for (int i = 0; i < armors.Count; ++i)
        {
            armors[i].transform.SetParent(itemsContainer);
            armors[i].transform.SetSiblingIndex(i);
            armors[i].GetComponent<ItemView>().outLine.SetActive(false);
        }
    }

    private void OnClickChangeLeaderButton()
    {
        presenter.ChangeLeader(selectedCharacter);
    }

    private void OnClickAddTeamButton()
    {
        presenter.AddTeam(selectedCharacter);
    }

    private void OnClickEquipButton()
    {
        if (string.IsNullOrEmpty(selectedItemName))
            return;

        presenter.Equip(selectedItemName, selectedCharacter);
    }

    public void OnClickItem(string name)
    {
        for (int i = 0; i < itemsContainer.childCount; ++i)
            itemsContainer.GetChild(i).gameObject.GetComponent<ItemView>().outLine.SetActive(false);

        itemName.text = name;
        if (name.Contains(' '))
            itemImage.sprite = Resources.Load<Sprite>("Prefabs/Items/" + name.Split(' ')[1]);
        else
            itemImage.sprite = Resources.Load<Sprite>("Prefabs/Items/" + name);

        selectedItemName = name;
    }
}