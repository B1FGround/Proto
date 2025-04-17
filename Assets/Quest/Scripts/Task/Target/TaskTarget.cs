using UnityEngine;

public abstract class TaskTarget : ScriptableObject
{
    // 실제 타겟의 타입은 자식클래스에서 정의
    public abstract object Value { get; }
    public abstract bool IsEqual(object target);
}
