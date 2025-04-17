using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum TaskGroupState
{
    InActive,
    InProgress,
    Complete,
}
[Serializable]
public class TaskGroup
{
    [SerializeField] Task[] tasks;

    public IReadOnlyList<Task> Tasks => tasks;
    public Quest Owner { get; private set; }
    public bool IsAllTaskComplete => tasks.All(t => t.IsComplete);
    public bool IsComplete => State == TaskGroupState.Complete;
    public TaskGroupState State { get; private set; }
    public void Setup(Quest owner)
    {
        Owner = owner;
        foreach (var task in tasks)
            task.Setup(owner);
    }
    public void Start()
    {
        State = TaskGroupState.InProgress;
        foreach (var task in tasks)
            task.Start();
    }
    public void End()
    {
        State = TaskGroupState.Complete;
        foreach (var task in tasks)
            task.End();
    }
    public void ReceiveReport(string category, object target, int successCount)
    {
        foreach (var task in tasks)
        {
            if(task.IsTarget(category, target))
                task.ReceiveReport(successCount);
        }
    }
    public void Complete()
    {
        if (IsComplete)
            return;
        foreach (var task in tasks)
            task.Complete();
    }
}
