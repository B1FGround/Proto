using UnityEngine;

[CreateAssetMenu(fileName = "PositiveCount", menuName = "Quest/Task/Action/Positive Count")]
public class PositiveCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount > 0 ? currentSuccess + successCount : currentSuccess;
    }
}
