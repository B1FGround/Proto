using UnityEngine;
using static CraftData;

public class Bottom : Item, IEquipable
{
    public IEquipable.EquipSocket Socket { get; set; }

    public Bottom(string itemName, int itemCount) : base(itemName, itemCount)
    {
        itemModel.Category = ItemCategory.Armor;
        Socket = IEquipable.EquipSocket.Bottom;
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