using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TeamInfoPresenter
{
    private InventoryModel inventoryModel;
    private TeamModel teamModel;
    public TeamInfoView view;

    public TeamInfoPresenter()
    {
        inventoryModel = new InventoryModel();
        teamModel = new TeamModel();
    }

    public void GetArmors()
    {
        view.ArmorsInitialize(inventoryModel.GetArmors());
    }

    public void ChangeLeader(TeamManager.CharacterType characterType)
    {
        teamModel.ChangeLeader(characterType);
    }

    public void AddTeam(TeamManager.CharacterType characterType)
    {
        teamModel.AddTeam(characterType);
    }

    public void Equip(string selectedItem, TeamManager.CharacterType selectedCharacter)
    {
        inventoryModel.Equip(selectedItem, selectedCharacter);
        GetArmors();
    }

    public void SelectCharacter(TeamManager.CharacterType character)
    {
        view.ClearSelectedCharacter();
        view.HighlightCharacter(character);
    }
}