using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestListView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI elementTextPrefab;
    Dictionary<Quest, GameObject> elementsByQuests = new Dictionary<Quest, GameObject>();

    [SerializeField] ToggleGroup toggleGroup;

    private void Awake()
    {
    }

    public void AddElement(Quest quest, UnityAction<bool> OnClicked)
    {
        var element = Instantiate(elementTextPrefab, transform);
        element.text = quest.DisplayName;

        var toggle = element.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(OnClicked);

        elementsByQuests.Add(quest, element.gameObject);
    }

    public void RemoveElement(Quest quest)
    {
        Destroy(elementsByQuests[quest]);
        elementsByQuests.Remove(quest);
    }
}
