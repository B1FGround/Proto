using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] Quest[] quests;

    private void Start()
    {
        foreach (var quest in quests)
        {
            if (quest.IsAcceptable && QuestManager.Instance.ContainsInCompletedQuests(quest) == false)
                QuestManager.Instance.Register(quest);
        }
    }
}
