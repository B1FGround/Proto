using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
///  Quest 정보 출력, TaskDescriptor의 제어 담당
/// </summary>
public class QuestTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questTitleText;
    [SerializeField] TaskDescriptor taskDescriptorPrefab;

    Dictionary<Task, TaskDescriptor> taskDescriptorsByTask = new Dictionary<Task, TaskDescriptor>();
    Quest targetQuest;

    public void Setup(Quest targetQuest, Color titleColor)
    {
        this.targetQuest = targetQuest;
        questTitleText.text = targetQuest.Category == null ? targetQuest.DisplayName : $"[{targetQuest.Category.DisplayName}] {targetQuest.DisplayName}";

        questTitleText.color = titleColor;

        targetQuest.OnNewTaskGroup += UpdateTaskDescriptors;
        targetQuest.OnComplete += DestroySelf;

        var taskGroups = targetQuest.TaskGroups;
        UpdateTaskDescriptors(targetQuest, taskGroups[0]);

        // 로드했을 때 완료된 태스크들 처리
        if (taskGroups[0] != targetQuest.CurrentTaskGroup)
        {
            for(int i = 1; i < taskGroups.Count; ++i)
            {
                var taskGroup = taskGroups[i];
                UpdateTaskDescriptors(targetQuest, taskGroup, taskGroups[i - 1]);
                if (taskGroup == targetQuest.CurrentTaskGroup)
                    break;
            }
        }
    }
    private void OnDestroy()
    {
        if(targetQuest != null)
        {
            targetQuest.OnNewTaskGroup -= UpdateTaskDescriptors;
            targetQuest.OnComplete -= DestroySelf;
        }
        foreach(var tuple in taskDescriptorsByTask)
        {
            var task = tuple.Key;
            task.OnSuccessChanged -= UpdateText;
        }
    }
    void UpdateTaskDescriptors(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup = null)
    {
        foreach (var task in currentTaskGroup.Tasks)
        {
            var taskDescriptor = Instantiate(taskDescriptorPrefab, transform);
            taskDescriptor.UpdateText(task);
            task.OnSuccessChanged += UpdateText;

            taskDescriptorsByTask.Add(task, taskDescriptor);
        }
        if (prevTaskGroup != null)
        {
            foreach(var task in prevTaskGroup.Tasks)
            {
                var taskDescriptor = taskDescriptorsByTask[task];
                taskDescriptor.UpdateTextUsingStrikeThrough(task);
            }
        }
    }
    void UpdateText(Task task, int currentSuccess, int prevSuccess)
    {
        taskDescriptorsByTask[task].UpdateText(task);
    }
    void DestroySelf(Quest quest)
    {
        Destroy(quest);
    }    
}
