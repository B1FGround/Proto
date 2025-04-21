using System.Collections.Generic;
using UnityEngine;
using static IEquipable;
using static TagHandler;

public class TagHandler : MonoBehaviour
{
    public enum TagCondition
    {
        OnEquip,
        OnAttack,
        OnSubAttack,
        OnHit,
        OnHeal,
        TagConditionEnd,
    }

    public enum TagType
    {
        Melee,
        Range,
        Fire,
        TagTypeEnd,
    }

    public Dictionary<TagCondition, Dictionary<TagType, List<TagEffect>>> tagEffects = new Dictionary<TagCondition, Dictionary<TagType, List<TagEffect>>>();

    public void OnAction(TagCondition tagCondition, TagType tagType, GameObject character = null, List<GameObject> targets = null)
    {
        if (tagEffects.ContainsKey(tagCondition) && tagEffects[tagCondition].ContainsKey(tagType))
        {
            foreach (var effect in tagEffects[tagCondition][tagType])
            {
                if (effect.ConditionCheck(character, targets))
                    effect.ApplyEffect(character, targets);
            }
        }
    }

    public void AddTagEffect(TagCondition condition, TagType tagType, TagEffect effect)
    {
        if (!tagEffects.ContainsKey(condition))
            tagEffects[condition] = new();

        if (!tagEffects[condition].ContainsKey(tagType))
            tagEffects[condition][tagType] = new();

        tagEffects[condition][tagType].Add(effect);
    }

    public void RemoveTagEffect(TagCondition condition, TagType tagType)
    {
        if (tagEffects.ContainsKey(condition) && tagEffects[condition].ContainsKey(tagType))
        {
            tagEffects[condition].Remove(tagType);

            if (tagEffects[condition].Count == 0)
                tagEffects.Remove(condition);
        }
    }

    public void RemoveTagEffectAllConditions(TagType tagType)
    {
        List<TagCondition> conditionsToRemove = new List<TagCondition>();

        foreach (var conditionPair in tagEffects)
        {
            if (conditionPair.Value.ContainsKey(tagType))
            {
                conditionPair.Value.Remove(tagType);

                if (conditionPair.Value.Count == 0)
                    conditionsToRemove.Add(conditionPair.Key);
            }
        }

        foreach (var condition in conditionsToRemove)
            tagEffects.Remove(condition);
    }
}