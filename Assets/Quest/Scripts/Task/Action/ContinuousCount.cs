using UnityEngine;

[CreateAssetMenu(fileName = "ContinuousCount", menuName = "Quest/Task/Action/Continuous Count")]
public class ContinuousCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount > 0 ? currentSuccess + successCount : 0;
    }
}
