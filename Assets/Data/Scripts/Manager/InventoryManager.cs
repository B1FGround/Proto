using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static CraftData;
using static IEquipable;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<Item> Items { get; set; } = new List<Item>();
    public Dictionary<TeamManager.CharacterType, Dictionary<EquipSocket, Item>> equipped = new Dictionary<TeamManager.CharacterType, Dictionary<EquipSocket, Item>>();

    public void AddItem(string itemName, int count, ItemDetail itemType = ItemDetail.DetailEnd)
    {
        var item = FindItem(itemName);
        if (item != null)
        {
            item.itemModel.SetInfo(item.itemModel.ItemName, item.itemModel.ItemCount + count);
            if (item.itemModel.ItemCount <= 0)
            {
                Items.Remove(item);
            }
        }
        else
        {
            item = ItemFactory.CreateItem(itemType, itemName, count);
            if(item != null)
                Items.Add(item);
        }
    }

    public Item FindItem(string itemName)
    {
        if (Items.Count == 0)
            return null;
        return Items.Find(x => x.itemModel.ItemName == itemName);
    }

    public void RemoveItem(string itemName)
    {
        Item item = FindItem(itemName);
        if (item is null)
            return;

        if (item.itemModel.ItemCount > 1)
        {
            item.itemModel.SetInfo(item.itemModel.ItemName, item.itemModel.ItemCount - 1);
        }
        else
        {
            Items.Remove(item);
        }
    }

    public int GetItemCount(string itemName)
    {
        Item item = FindItem(itemName);
        if (item is null)
            return 0;
        return item.itemModel.ItemCount;
    }

    public bool CheckItemCount(string itemName, int count)
    {
        Item item = FindItem(itemName);
        if (item is null)
            return false;
        return item.itemModel.ItemCount >= count;
    }

    public void AddAllIngredient(int count)
    {
        for (int i = 0; i < CraftData.detailType[0].Length; ++i)
            AddItem(CraftData.detailType[0][i], count, (ItemDetail)i);
    }

    public List<Item> GetItemsByCategory(CraftData.ItemCategory category)
    {
        List<Item> items = new List<Item>();
        if (category == CraftData.ItemCategory.None)
            return Items;

        foreach (var item in Items)
        {
            if (item.itemModel.Category == category)
            {
                items.Add(item);
            }
        }
        return items;
    }

    public CraftData.ItemDetail GetDetailType(string itemName)
    {
        Item item = FindItem(itemName);
        if (item is null)
            return CraftData.ItemDetail.DetailEnd;
        return item.itemModel.Detail;
    }

    public void Equip(TeamManager.CharacterType character, Item item)
    {
        if (item is IEquipable equipable)
        {
            if (equipped.ContainsKey(character))
            {
                if (equipped[character].ContainsKey(equipable.Socket) == true)
                {
                    AddItem(equipped[character][equipable.Socket].itemModel.ItemName, 1);
                    equipped[character][equipable.Socket] = item;
                    AddItem(item.itemModel.ItemName, -1);
                }
                else
                {
                    equipped[character].Add(equipable.Socket, item);
                    AddItem(item.itemModel.ItemName, -1);
                }
            }
            else
            {
                var newEquipped = new Dictionary<EquipSocket, Item>();
                newEquipped.Add(equipable.Socket, item);
                equipped.Add(character, newEquipped);

                AddItem(item.itemModel.ItemName, -1);
            }
        }
    }
}