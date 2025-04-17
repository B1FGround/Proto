using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public abstract class InitialSuccessValue : ScriptableObject
{
    public abstract int GetValue(Task task);
}
