using System;
using System.Linq;
using UnityEngine;



/// <summary>
/// QuestTracker 제어 담당
/// </summary>
public class QuestTrackerView : MonoBehaviour
{
    [Serializable]
    struct CategoryColor
    {
        public Category category;
        public Color color;
    }

    [SerializeField] QuestTracker questTrackerPrefab;
    [SerializeField] CategoryColor[] categoryColors;

    private void Start()
    {
        QuestManager.Instance.QuestRegistered += CreateQuestTracker;
        
        foreach(var quest in QuestManager.Instance.ActiveQuests)
            CreateQuestTracker(quest);
    }
    private void OnDestroy()
    {
        if(QuestManager.Instance != null)
            QuestManager.Instance.QuestRegistered -= CreateQuestTracker;
    }
    void CreateQuestTracker(Quest quest)
    {
        var categoryColor = categoryColors.FirstOrDefault(x => x.category == quest.Category);
        var color = categoryColor.category == null ? Color.white : categoryColor.color;
        Instantiate(questTrackerPrefab, transform).Setup(quest, color);
    }

}