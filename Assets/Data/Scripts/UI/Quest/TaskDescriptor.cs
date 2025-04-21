using TMPro;
using UnityEngine;

/// <summary>
/// Task 정보 출력
/// </summary>
public class TaskDescriptor : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Color normalColor;
    [SerializeField] Color taskCompletionColor;
    [SerializeField] Color taskSuccessCountColor;
    [SerializeField] Color strikeThroughColor;

    public void UpdateText(string text)
    {
        this.text.text = text;
    }
    public void UpdateText(Task task)
    {
        text.fontStyle = FontStyles.Normal;
        if(task.IsComplete)
        {
            var colorCode = ColorUtility.ToHtmlStringRGB(taskCompletionColor);
            text.text = BuildText(task, colorCode, colorCode);
        }
        else
        {
            text.text = BuildText(task, ColorUtility.ToHtmlStringRGB(normalColor), ColorUtility.ToHtmlStringRGB(taskSuccessCountColor));
        }
    }
    public void UpdateTextUsingStrikeThrough(Task task)
    {
        var colorCode = ColorUtility.ToHtmlStringRGB(strikeThroughColor);
        text.fontStyle = FontStyles.Strikethrough;
        text.text = BuildText(task, colorCode, colorCode);
    }

    string BuildText(Task task, string textColorCode, string successCountColorCode)
    {
        return $"<color=#{textColorCode}>● {task.Description} <color=#{successCountColorCode}>{task.CurrentSuccess}</color>/{task.NeedToSuccessToComplete}</color>";
    }
}
