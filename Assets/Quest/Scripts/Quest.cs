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
    [SerializeField] bool isSavable;

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
    public virtual bool IsCancelable => isCancelable;
    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));
    public bool IsCancleable => IsCancelable && cancelCondition.All(x => x.IsPass(this));
    public virtual bool IsSavable => isSavable;

    public event TaskSuccessChangedHandler OnTaskSuccessChanged;
    public event CompleteHandler OnComplete;
    public event CancelHandler OnCanceled;
    public event NewTaskGroupHandler OnNewTaskGroup;


    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "Already registered quest");

        foreach(var taskGroup in taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.Tasks)
            {
                task.OnSuccessChanged += OnSuccessChanged;
                task.OnSaveQuest += () =>
                {
                    QuestManager.Instance.Save();
                };

            }
        }
        State = QuestState.InProgress;
        CurrentTaskGroup.Start();
    }
    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(IsRegistered, "Already registered quest");
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
        OnCanceled = null;
        OnNewTaskGroup = null;
    }
    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(!IsCancelable, "Not cancelable quest");

        State = QuestState.Cancel;
        OnCanceled?.Invoke(this);

    }

    public Quest Clone()
    {
        var clone = Instantiate(this);

        // 지금은 복사본을 만들때 Task만 복사
        // 나중에 Task처럼 실시간으로 값이 변경되는 기능이 있다면 TaskGroup(TaskGroup copyTarget)과 같이 복사해줘야함
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }

    private void OnSuccessChanged(Task task, int currentSuccess, int preSuccess) => OnTaskSuccessChanged?.Invoke(this, task, currentSuccess, preSuccess);

    #region Save & Load
    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData(CodeName, State, currentTaskGroupIndex, CurrentTaskGroup.Tasks.Select(x => x.CurrentSuccess).ToArray());
    }
    public void LoadFrom(QuestSaveData saveData)
    {
        State = saveData.state;
        currentTaskGroupIndex = saveData.taskGroupIndex;

        // 이전 taskGroup은 모두 완료처리
        // 저장할때 currentTaskGroupIndex를 저장했고 currentTaskGroupIndex이전 값들은 모두 완료된 의미
        for (int i = 0; i < currentTaskGroupIndex; ++i)
        {
            var taskGroup = taskGroups[i];
            taskGroup.Start();
            taskGroup.Complete();
        }

        // 현재 진행하고 있는 퀘스트의 진행 횟수 로드
        for(int i = 0; i < saveData.taskSuccessCounts.Length; ++i)
        {
            CurrentTaskGroup.Start();
            CurrentTaskGroup.Tasks[i].CurrentSuccess = saveData.taskSuccessCounts[i];
        }
    }
    #endregion

    [Conditional("UNITY_EDITOR")]
    public void CheckIsRunning()
    {
        Debug.Assert(IsRegistered, "Already registered quest");
        Debug.Assert(!IsCancel, "Already canceled quest");
        Debug.Assert(!IsComplete, "Already completed quest");
    }

}
