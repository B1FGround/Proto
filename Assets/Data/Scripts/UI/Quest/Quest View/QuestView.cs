using UnityEngine;
using UnityEngine.Rendering;

public class QuestView : MonoBehaviour
{
    [SerializeField] QuestListViewController questListViewController;
    [SerializeField] QuestDetailView questDetailView;

    private void Start()
    {
        var questSystem = QuestManager.Instance;

        foreach(var quest in questSystem.ActiveQuests)
            AddQuestToActiveListView(quest);

        foreach (var quest in questSystem.CompletedQuests)
            AddQuestToCompletedListView(quest);

        questSystem.QuestRegistered += AddQuestToActiveListView;
        questSystem.QuestCompleted += RemoveQuestFromActiveListView;
        questSystem.QuestCompleted += AddQuestToCompletedListView;
        questSystem.QuestCompleted += HideDetailIfQuestCanceled;
        questSystem.QuestCanceled += HideDetailIfQuestCanceled;
        questSystem.QuestCanceled += RemoveQuestFromActiveListView;

        foreach (var tab in questListViewController.Tabs)
            tab.onValueChanged.AddListener(HideDetail);

        //gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        var questSystem = QuestManager.Instance;
        if (questSystem)
        {
            questSystem.QuestRegistered -= AddQuestToActiveListView;
            questSystem.QuestCompleted -= RemoveQuestFromActiveListView;
            questSystem.QuestCompleted -= AddQuestToCompletedListView;
            questSystem.QuestCompleted -= HideDetailIfQuestCanceled;
            questSystem.QuestCanceled -= HideDetailIfQuestCanceled;
            questSystem.QuestCanceled -= RemoveQuestFromActiveListView;
        }
    }
    private void OnEnable()
    {
        if (questDetailView.Target != null)
            questDetailView.Show(questDetailView.Target);
    }
    private void ShowDetail(bool isOn, Quest quest)
    {
        if (isOn)
            questDetailView.Show(quest);
    }
    void HideDetail(bool isOn)
    {
        questDetailView.Hide();
    }
    void AddQuestToActiveListView(Quest quest)
        => questListViewController.AddQuestToActiveListView(quest, isOn => ShowDetail(isOn, quest));

    void AddQuestToCompletedListView(Quest quest)
        => questListViewController.AddQuestToCompletedView(quest, isOn => ShowDetail(isOn, quest));

    void HideDetailIfQuestCanceled(Quest quest)
    {
        if (questDetailView.Target == quest)
            questDetailView.Hide();
    }
    void RemoveQuestFromActiveListView(Quest quest)
    {
        questListViewController.RemoveQuestFromActiveListView(quest);
        if(questDetailView.Target == quest)
            questDetailView.Hide();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    gameObject.SetActive(!gameObject.activeSelf);
        //}
    }
}
