using UnityEngine;

[CreateAssetMenu(fileName = "Target_", menuName = "Quest/Task/Target/String")]
public class StringTarget : TaskTarget
{
    [SerializeField] private string value;

    public override object Value => value;

    public override bool IsEqual(object target)
    {
        string targetAsString = target as string;
        if(targetAsString == null)
            return false;

        return value == targetAsString;
    }
}
