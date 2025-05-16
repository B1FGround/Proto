using UnityEngine;
using static CraftData;

public class Leg : Item, IEquipable
{
    public IEquipable.EquipSocket Socket { get; set; }

    public Leg(string itemName, int itemCount) : base(itemName, itemCount)
    {
        itemModel.Category = ItemCategory.Armor;
        itemModel.Detail = ItemDetail.Leg;
        Socket = IEquipable.EquipSocket.Leg;
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