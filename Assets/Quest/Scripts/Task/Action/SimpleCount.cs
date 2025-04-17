using UnityEngine;

[CreateAssetMenu(fileName = "SimpleCount", menuName = "Quest/Task/Action/Simple Count")]
public class SimpleCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return currentSuccess + successCount;
    }
}
