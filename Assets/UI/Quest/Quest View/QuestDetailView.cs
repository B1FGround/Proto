using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailView : MonoBehaviour
{
    [SerializeField] GameObject displayGroup;
    [SerializeField] Button cancelButton;

    [Header("Quest Description")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;

    [Header("Task Description")]
    [SerializeField] RectTransform taskDescriptionGroup;
    [SerializeField] TaskDescriptor taskDescriptorPrefab;
    [SerializeField] int taskDescriptorPoolCount;

    [Header("Reward Description")]
    [SerializeField] RectTransform rewardDescriptionGroup;
    [SerializeField] TextMeshProUGUI rewardDescriptionPrefab;
    [SerializeField] int rewardDescriptionPoolCount;

    List<TaskDescriptor> taskDescriptorPool;
    List<TextMeshProUGUI> rewardDescriptionPool;

    public Quest Target { get; private set; }

    private void Awake()
    {
        taskDescriptorPool = CreatePool(taskDescriptorPrefab, taskDescriptorPoolCount, taskDescriptionGroup);
        rewardDescriptionPool = CreatePool(rewardDescriptionPrefab, rewardDescriptionPoolCount, rewardDescriptionGroup);
        displayGroup.SetActive(false);

    }
    private void Start()
    {
        cancelButton.onClick.AddListener(CancelQuest);
    }
    private List<T> CreatePool<T>(T prefab, int count, RectTransform parent) where T : MonoBehaviour
    {
        var pool = new List<T>(count);
        for (int i = 0; i < count; ++i)
            pool.Add(Instantiate(prefab, parent));

        return pool;
    }
    public void Show(Quest quest)
    {
        displayGroup.SetActive(true);
        Target = quest;

        title.text = quest.DisplayName;
        description.text = quest.Description;

        int taskIndex = 0;
        foreach(var taskGroup in quest.TaskGroups)
        {
            foreach(var task in taskGroup.Tasks)
            {
                var poolObject = taskDescriptorPool[taskIndex++];
                poolObject.gameObject.SetActive(true);

                if (taskGroup.IsComplete)
                    poolObject.UpdateTextUsingStrikeThrough(task);
                else if (taskGroup == quest.CurrentTaskGroup)
                    poolObject.UpdateText(task);
                else // 아직 진행하지 못하는 퀘스트
                    poolObject.UpdateText("● ???????????");
            }
        }

        // 사용하지 않는 pool 객체 끄기
        for(int i = taskIndex; i < taskDescriptorPoolCount; ++i)
            taskDescriptorPool[i].gameObject.SetActive(false);

        var rewards = quest.Rewards;
        var rewardsCount = rewards.Count;
        for(int i = 0; i < rewardsCount; ++i)
        {
            var poolObject = rewardDescriptionPool[i];
            if (i < rewardsCount)
            {
                var reward = rewards[i];
                poolObject.text = $"● {reward.Description} +{reward.Quantity}";
            }
            else
                poolObject.gameObject.SetActive(false);
        }

        cancelButton.gameObject.SetActive(quest.IsCancelable && !quest.IsComplete);
    }
    public void Hide()
    {
        Target = null;
        displayGroup.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }
    void CancelQuest()
    {
        if (Target.IsCancelable)
            Target.Cancel();
    }
}
