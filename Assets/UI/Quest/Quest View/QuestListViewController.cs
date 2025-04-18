using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestListViewController : MonoBehaviour
{
    [SerializeField] Toggle[] tabs;

    [SerializeField] QuestListView activeQuestListView;
    [SerializeField] QuestListView completedQuestListView;


    public Toggle[] Tabs => tabs;
    public void AddQuestToActiveListView(Quest quest, UnityAction<bool> OnClicked)
        => activeQuestListView.AddElement(quest, OnClicked);
    public void RemoveQuestFromActiveListView(Quest quest)
        => activeQuestListView.RemoveElement(quest);
    public void AddQuestToCompletedView(Quest quest, UnityAction<bool> OnClicked)
        => completedQuestListView.AddElement(quest, OnClicked);


}
