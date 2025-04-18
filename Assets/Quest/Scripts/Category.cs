using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Category_", menuName = "Quest/Category")]
public class Category : ScriptableObject, IEquatable<Category>
{
    [SerializeField] string codeName = "Default";
    [SerializeField] string displayName = "Default";

    public string CodeName => codeName;
    public string DisplayName => displayName;

    #region Operator
    public bool Equals(Category other)
    {
        if (other == null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return codeName == other.codeName;

    }

    public override int GetHashCode() => codeName.GetHashCode();

    public override bool Equals(object other) => Equals(other as Category);

    public static bool operator ==(Category left, string right)
    {
        if (left is null)
            return ReferenceEquals(right, null);
        return left.CodeName == right || left.DisplayName == right;
    }
    public static bool operator !=(Category left, string right) => !(left == right);
    #endregion
}
