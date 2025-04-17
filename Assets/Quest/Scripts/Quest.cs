using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

using Debug = UnityEngine.Debug;
using System.Linq;
/*
 *  퀘스트 경우의 수
 *  1. 태스크 한개
 *  2. 태스크 여러개
 *  3. 하나의 태스크 완료 후 다른 태스크 시작
 */

public enum QuestState
{
    InActive,
    InProgress,
    Complete,
    Cancel,
    WaitingForComplition,
}
[CreateAssetMenu(fileName = "Quest_", menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    #region Events
    public delegate void TaskSuccessChangedHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompleteHandler(Quest quest);
    public delegate void CancelHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup);
    #endregion
    [SerializeField] Category category;
    [SerializeField] Sprite icon;

    [Header("Quest Info")]
    [SerializeField] string codeName;
    [SerializeField] string displayName;
    [SerializeField, TextArea] string description;


    [Header("Tasks")]
    [SerializeField] TaskGroup[] taskGroups;

    [Header("Rewards")]
    [SerializeField] Reward[] rewards;

    [Header("Options")]
    [SerializeField] bool useAutoComplete;
    [SerializeField] bool isCancelable;

    [Header("Contidions")]
    [SerializeField] Condition[] acceptionConditions;
    [SerializeField] Condition[] cancelCondition;

    private int currentTaskGroupIndex;

    public Category Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    public QuestState State { get; private set; }
    public TaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> TaskGroups => taskGroups;
    public bool IsRegistered => State != QuestState.InActive;
    public bool IsCompleteable => State == QuestState.WaitingForComplition;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public IReadOnlyList<Reward> Rewards => rewards;
    public bool IsCancelable => isCancelable;
    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));
    public bool IsCancleable => IsCancelable && cancelCondition.All(x => x.IsPass(this));

    public event TaskSuccessChangedHandler OnTaskSuccessChanged;
    public event CompleteHandler OnComplete;
    public event CancelHandler OnCancel;
    public event NewTaskGroupHandler OnNewTaskGroup;


    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "Already registered quest");

        foreach(var taskGroup in taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.Tasks)
                task.OnSuccessChanged += OnSuccessChanged;
        }
        State = QuestState.InProgress;
        CurrentTaskGroup.Start();
    }
    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(!IsRegistered, "Already registered quest");
        Debug.Assert(!IsCancel, "Already canceled quest");

        if (IsComplete)
            return;

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if (currentTaskGroupIndex + 1 == taskGroups.Length)
            {
                State = QuestState.WaitingForComplition;
                if (useAutoComplete)
                    Complete();
            }
            else
            {
                var prevTaskGroup = taskGroups[currentTaskGroupIndex++];
                prevTaskGroup.End();
                CurrentTaskGroup.Start();
                OnNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTaskGroup);
            }
        }
        else
            State = QuestState.InProgress; // Task의 canReceiveReportsDuringCompletion 옵션 때문에 넣어줌 


    }
    public void Complete()
    {
        CheckIsRunning();

        foreach (var taskGroup in taskGroups)
            taskGroup.Complete();

        State = QuestState.Complete;

        foreach (var reward in rewards)
            reward.Give(this);

        OnComplete?.Invoke(this);

        OnTaskSuccessChanged = null;
        OnComplete = null;
        OnCancel = null;
        OnNewTaskGroup = null;
    }
    public void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(!IsCancelable, "Not cancelable quest");

        State = QuestState.Cancel;
        OnCancel?.Invoke(this);

    }

    private void OnSuccessChanged(Task task, int currentSuccess, int preSuccess) => OnTaskSuccessChanged?.Invoke(this, task, currentSuccess, preSuccess);

    [Conditional("UNITY_EDITOR")]
    public void CheckIsRunning()
    {
        Debug.Assert(!IsRegistered, "Already registered quest");
        Debug.Assert(!IsCancel, "Already canceled quest");
        Debug.Assert(!IsComplete, "Already completed quest");
    }

}
