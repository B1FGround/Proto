using UnityEngine;

public interface IEquipable
{
    public enum EquipSocket
    {
        Helmet,
        Armor,
        Bottom,
        Leg,
    }

    public EquipSocket Socket { get; set; }

    public void OnEquip();

    public void OnUnEquip();
}