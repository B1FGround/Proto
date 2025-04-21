using System;
using UnityEngine;

public abstract class Condition : ScriptableObject
{
    [SerializeField] string description;
    public abstract bool IsPass(Quest quest);
}
