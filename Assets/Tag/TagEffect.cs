using System.Collections.Generic;
using UnityEngine;

public abstract class TagEffect
{
    public int priority = 0;

    public abstract bool ConditionCheck(GameObject character = null, List<GameObject> targets = null);

    public abstract void ApplyEffect(GameObject character = null, List<GameObject> targets = null);
}