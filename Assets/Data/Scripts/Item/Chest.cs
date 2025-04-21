using UnityEngine;
using static CraftData;

public class Chest : Item, IEquipable
{
    public IEquipable.EquipSocket Socket { get; set; }

    public Chest(string name, int count) : base(name, count)
    {
        itemModel.Category = ItemCategory.Armor;
        Socket = IEquipable.EquipSocket.Armor;
    }

    public void OnEquip()
    {
    }

    public void OnUnEquip()
    {
    }

    public void SetInfo(string itemName, int itemCount)
    {
        itemModel.SetInfo(itemName, itemCount);
    }
}