using System.Linq;
using UnityEngine;


public enum TaskState
{
    InActive,
    InProgress,
    Complete,
}
[CreateAssetMenu(fileName = "Task_", menuName = "Quest/Task/Task")]
public class Task : ScriptableObject
{
    #region Events
    public delegate void StateSChangedHandler(Task task, TaskState currentState, TaskState prevState);
    public delegate void SuccessChangedHandler(Task task, int currentSuccess, int prevSuccess);
    #endregion
    [Header("Category")]
    [SerializeField] Category category;

    [Header("Task Info")]
    [SerializeField] string codeName;
    [SerializeField] string description;

    [Header("Action")]
    [SerializeField] TaskAction action;

    [Header("Target")]
    [SerializeField] TaskTarget[] targets;

    [Header("Setting")]
    [SerializeField] InitialSuccessValue initialSuccessValue;
    [SerializeField] int neesSuccessToComplete;

    /// <summary>
    /// // 퀘스트가 완료되어도 보고를 받을 건지 (ex : 아이템 획득 12/10, 퀘스트 완료 전에 10개를 버린다면 실제 아이템은 없지만 조건 완료상태임)
    /// </summary>
    [SerializeField] bool canReceiveReportsDuringCompletion; 

    TaskState state;
    int currentSuccess;
    public event StateSChangedHandler OnStateChanged;
    public event SuccessChangedHandler OnSuccessChanged;

    public Category Category => category;
    public int CurrentSuccess
    {
        get => currentSuccess;
        set
        {
            var prevSuccess = currentSuccess;
            currentSuccess = Mathf.Clamp(value, 0, NeedToSuccessToComplete);

            if (currentSuccess != prevSuccess)
            {
                State = currentSuccess == NeedToSuccessToComplete ? TaskState.Complete : TaskState.InProgress;
                OnSuccessChanged?.Invoke(this, currentSuccess, prevSuccess);
            }
        }
    }
    public string CodeName => codeName;
    public string Description => description;
    public int NeedToSuccessToComplete => neesSuccessToComplete;
    public TaskState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            OnStateChanged?.Invoke(this, state, prevState);
        }
    }
    public bool IsComplete => State == TaskState.Complete;
    public Quest Owner { get; private set; }
    public void Setup(Quest owner)
    {
        Owner = owner;
    }
    public void Start()
    {
        state = TaskState.InProgress;
        if(initialSuccessValue != null)
            CurrentSuccess = initialSuccessValue.GetValue(this);
    }
    public void End()
    {
        state = TaskState.Complete;
        OnStateChanged = null;
        OnSuccessChanged = null;
    }
    public void Complete()
    {
        CurrentSuccess = NeedToSuccessToComplete;
    }

    public void ReceiveReport(int successCount)
    {
        // 모듈식으로 짜게 되면 실행하는 주최 (this)를 넣어주는게 여러모로 편리함
        CurrentSuccess = action.Run(this, CurrentSuccess, successCount);

    }

    public bool IsTarget(string category, object target) 
        => Category == category &&
        targets.Any(t => t.IsEqual(target)) &&
        (IsComplete == false || (IsComplete && canReceiveReportsDuringCompletion));


}
