using UnityEngine;

public class QuestSystemTest : MonoBehaviour
{
    [SerializeField] Quest quest;
    [SerializeField] Category category;
    [SerializeField] TaskTarget target;
    void Start()
    {
        var questManager = QuestManager.Instance;
        questManager.QuestRegistered += (quest) =>
        {
            print($"New Quest : {quest.CodeName} registered");
            print($"Active Quest Count : {questManager.ActiveQuests.Count}");
        };
        questManager.QuestCompleted += (quest) =>
        {
            print($"Quest Completed : {quest.CodeName} Completed");
            print($"Completed Quest Count : {questManager.CompletedQuests.Count}");
        };

        var newQuest = questManager.Register(quest);
        newQuest.OnTaskSuccessChanged += (args, task, currentSuccess, prevSuccess) =>
        {
            print($"Quest : {quest.CodeName}, Task : {task.Description}, CurrentSuccess : {task.CurrentSuccess}");
        };
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            QuestManager.Instance.ReceiveReport(category, target, 1);
        }
    }
}
