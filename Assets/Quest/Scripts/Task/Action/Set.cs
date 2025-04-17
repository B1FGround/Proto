using UnityEngine;

[CreateAssetMenu(fileName = "Set", menuName = "Quest/Task/Action/Set")]
public class SetModule : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount;
    }
}
