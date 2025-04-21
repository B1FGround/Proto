using UnityEngine;
using static CraftData;
using static ItemModel;

public static class ItemFactory
{
    public static Item CreateItem(ItemDetail itemType, string itemName, int itemCount)
    {
        switch (itemType)
        {
            case ItemDetail.Helmet:
                return new Helmet(itemName, itemCount);

            case ItemDetail.Armor:
                return new Chest(itemName, itemCount);

            case ItemDetail.Bottom:
                return new Bottom(itemName, itemCount);

            case ItemDetail.Drum:
            case ItemDetail.Stone:
            case ItemDetail.Cube:
                return new Item(itemName, itemCount);

            default:
                return null;
        }
    }
}