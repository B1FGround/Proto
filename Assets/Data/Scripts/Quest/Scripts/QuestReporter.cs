using System.Linq;
using UnityEngine;

public class QuestReporter : MonoBehaviour
{
    [SerializeField] Category category;
    [SerializeField] TaskTarget target;
    [SerializeField] int successCount;
    [SerializeField] string[] colliderTags;

    private void OnTriggerEnter(Collider other)
    {
        ReportIfPassCondition(other);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ReportIfPassCondition(collision);
    }
    public void Report()
    {
        QuestManager.Instance.ReceiveReport(category, target, successCount);
    }
    void ReportIfPassCondition(Component other)
    {
        if (colliderTags.Any(x => other.CompareTag(x)))
            Report();
    }
}
