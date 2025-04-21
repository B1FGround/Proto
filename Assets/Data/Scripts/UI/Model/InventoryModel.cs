using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    public List<GameObject> GetArmors()
    {
        var inventoryItems = InventoryManager.Instance.GetItemsByCategory(CraftData.ItemCategory.Armor);

        var items = new List<GameObject>();

        for (int i = 0; i < inventoryItems.Count; ++i)
        {
            string itemName = inventoryItems[i].itemModel.ItemName;
            int itemCount = inventoryItems[i].itemModel.ItemCount;

            var invenItem = Object.Instantiate(Resources.Load("Prefabs/UI/Item")) as GameObject;
            invenItem.GetComponent<ItemView>().SetInfo(itemName, itemCount, new ItemPresenter());
            items.Add(invenItem);
            invenItem.GetComponent<ItemView>().outLine.SetActive(false);
        }

        return items;
    }

    public void Equip(string selectedItemName, TeamManager.CharacterType selectedCharacter)
    {
        var item = InventoryManager.Instance.FindItem(selectedItemName);
        InventoryManager.Instance.Equip(selectedCharacter, item);
        var characters = GameObject.FindWithTag("Player").GetComponent<TeamContainer>().GetCharacters();
        foreach (var character in characters)
            character.GetComponent<CharacterAttack>().Equip(selectedCharacter);
    }
}