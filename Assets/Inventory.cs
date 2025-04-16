using System;
using System.Collections.Generic;
using UnityEngine;
using static CraftUI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private Dictionary<string, (ItemType, int)> inventory = new Dictionary<string, (ItemType, int)>();

    // key: 캐릭터 타입, value: 장착된 장비타입, 이름
    public Dictionary<TeamManager.CharacterType, Dictionary<DetailType, string>> equipped = new Dictionary<TeamManager.CharacterType, Dictionary<DetailType, string>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //for(int i = 0; i< CraftUI.detailType[0].Length; i++)
        //{
        //    inventory.Add(CraftUI.detailType[0][i], 0);
        //}
    }

    public void AddItem(string itemName, ItemType type, int count = 1)
    {
        if (inventory.ContainsKey(itemName))
        {
            var prevCount = inventory[itemName];
            prevCount.Item2 += count;

            inventory[itemName] = prevCount;
        }
        else
        {
            inventory.Add(itemName, (type, count));
        }
    }

    public bool CheckItemCount(string itemName, int count)
    {
        if (inventory.ContainsKey(itemName))
        {
            var value = inventory[itemName].Item2;
            return value >= count;
        }
        return false;
    }

    public int GetItemCount(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            var value = (int)inventory[itemName].Item2;
            return value;
        }
        return 0;
    }

    public (ItemType, string) GetItemTypeAndName(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            var value = inventory[itemName].Item1;
            return (value, itemName);
        }
        return (ItemType.None, "");
    }

    public void AddAllIngredient(int count)
    {
        for (int i = 0; i < CraftUI.detailType[0].Length; ++i)
            AddItem(CraftUI.detailType[0][i], ItemType.Common, count);
    }

    public List<(string, int)> GetItemsByType(CraftUI.ItemType type)
    {
        var list = new List<(string, int)>();

        if (type == ItemType.None)
        {
            foreach (var item in inventory)
            {
                if (item.Value.Item2 > 0)
                    list.Add((item.Key, item.Value.Item2));
            }
            return list;
        }

        foreach (var item in inventory)
        {
            if (item.Value.Item1 == type)
            {
                if (item.Value.Item2 > 0)
                    list.Add((item.Key, item.Value.Item2));
            }
        }
        return list;
    }

    public void Equip(TeamManager.CharacterType character, ItemModel item)
    {
        //if (equipped.ContainsKey(character))
        //{
        //    var itemTypeKeyValue = equippment;
        //
        //    if (equipped[character].ContainsKey(itemTypeKeyValue.Item1))
        //    {
        //        AddItem(equipped[character][itemTypeKeyValue.Item1], ItemType.Armor, 1);
        //        equipped[character][itemTypeKeyValue.Item1] = itemTypeKeyValue.Item2;
        //        AddItem(equipped[character][itemTypeKeyValue.Item1], ItemType.Armor, -1);
        //    }
        //    else
        //    {
        //        equipped[character].Add(itemTypeKeyValue.Item1, itemTypeKeyValue.Item2);
        //        AddItem(equippment.Item2, ItemType.Armor, -1);
        //    }
        //}
        //else
        //{
        //    var newEquipped = new Dictionary<DetailType, string>();
        //    newEquipped.Add(equippment.Item1, equippment.Item2);
        //    equipped.Add(character, newEquipped);
        //
        //    AddItem(equippment.Item2, ItemType.Armor, -1);
        //}
    }

    public DetailType GetDetailType(string itemName)
    {
        var name = itemName.Split(' ')[1];

        return (DetailType)Enum.Parse(typeof(DetailType), name);
    }
}