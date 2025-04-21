using UnityEngine;

[CreateAssetMenu(fileName = "Achievement_", menuName = "Quest/Achievement")]
public class Achievement : Quest
{
    public override bool IsCancelable => false;
    public override bool IsSavable => false;

    public override void Cancel()
    {
        Debug.LogAssertion("Achievement can't be canceled");
    }
}
