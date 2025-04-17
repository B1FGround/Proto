using UnityEngine;

public class QuestSystemSaveTest : MonoBehaviour
{
    [SerializeField] Quest quest;
    [SerializeField] Category category;
    [SerializeField] TaskTarget target;
    void Start()
    {
        var questManager = QuestManager.Instance;
        if(questManager.ActiveQuests.Count == 0 )
        {
            Debug.Log("Register");
            var newQuest = questManager.Register(quest);
        }
        else
        {
            questManager.QuestCompleted += (quest) =>
            {
                Debug.Log("Completed");
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            };
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            QuestManager.Instance.ReceiveReport(category, target, 1);
        }
    }
}
