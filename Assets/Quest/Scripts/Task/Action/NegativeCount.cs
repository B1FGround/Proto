using UnityEngine;

[CreateAssetMenu(fileName = "NegativeCount", menuName = "Quest/Task/Action/Negative Count")]
public class NegativeCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount < 0 ? currentSuccess - 1 : currentSuccess;
    }
}
