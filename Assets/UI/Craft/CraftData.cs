using UnityEngine;
using static CraftUI;

public static class CraftData
{
    public enum ItemCategory
    {
        Common,
        Armor,
        None,
    }

    public enum ItemDetail
    {
        Drum,
        Stone,
        Cube,
        Helmet,
        Armor,
        Sword,
        Glove,
        Shoulder,
        Bottom,
        Leg,
        None,
        DetailEnd,
    }

    public struct Ingredient
    {
        public ItemType type;
        public string ItemName;
        public (string, int)[] Ingredients;
    }

    public static string[][] ItemList = new string[][]
    {
        new string[] { "Red Drum", "Green Drum", "Blue Drum" },
        new string[] { "Red Stone", "Green Stone", "Blue Stone" },
        new string[] { "Red Cube", "Green Cube", "Blue Cube", "Black Cube" },
        new string[] { "Red Helmet", "Green Helmet", "Blue Helmet" },
        new string[] { "Red Armor", "Green Armor", "Blue Armor" },
        new string[] { "Red Sword", "Green Sword", "Blue Sword", "Black Sword", "White Sword" },
        new string[] { "Red Glove", "Green Glove", "Blue Glove" },
        new string[] { "Red Shoulder", "Green Shoulder", "Blue Shoulder" },
        new string[] { "Red Bottom", "Green Bottom", "Blue Bottom" },
        new string[] { "Red Leg", "Green Leg", "Blue Leg" },

        new string[] { "None", "None", "None" },
    };

    public static string[][] detailType = new string[][]
    {
        new string[] { "Drum", "Stone", "Cube" },
        new string[] { "Helmet", "Armor", "Bottom", "Leg" },
        new string[] { "No item", "No item", "No item" },
    };

    public static Ingredient[] craft = new Ingredient[]
    {
        new Ingredient { type = ItemType.Common, ItemName = "Red Drum", Ingredients = new (string, int)[] { ("Stone", 2), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Green Drum", Ingredients = new (string, int)[] { ("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Blue Drum", Ingredients = new (string, int)[] { ("Stone", 5), ("Cube", 1) } },

        new Ingredient { type = ItemType.Common, ItemName = "Red Stone", Ingredients = new (string, int)[] {("Stone", 7), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Green Stone", Ingredients = new (string, int)[] { ("Drum", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Blue Stone", Ingredients = new (string, int)[] {("Stone", 2), ("Cube", 1) } },

        new Ingredient { type = ItemType.Common, ItemName = "Red Cube", Ingredients = new (string, int)[] { ("Drum", 5), ("Stone", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Green Cube", Ingredients = new (string, int)[] {("Stone", 6), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Blue Cube", Ingredients = new (string, int)[] {("Stone", 2), ("Cube", 1) } },
        new Ingredient { type = ItemType.Common, ItemName = "Black Cube", Ingredients = new (string, int)[] { ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Helmet", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 3) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Helmet", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 6) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Helmet", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 2) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Armor", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Armor", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 2) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Armor", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 4) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1), ("Drum", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Black Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "White Sword", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Glove", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Glove", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Glove", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Shoulder", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Shoulder", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Shoulder", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Bottom", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Bottom", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Bottom", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },

        new Ingredient { type = ItemType.Armor, ItemName = "Red Leg", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Green Leg", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
        new Ingredient { type = ItemType.Armor, ItemName = "Blue Leg", Ingredients = new (string, int)[] {("Stone", 1), ("Cube", 1) } },
    };
}