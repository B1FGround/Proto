using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Quest/Quest Database")]
public class QuestDatabase : ScriptableObject
{
    [SerializeField] List<Quest> quests;

    public IReadOnlyList<Quest> Quests => quests;

    public Quest FindQuestBy(string codeName) => Quests.FirstOrDefault(x => x.CodeName == codeName);

#if UNITY_EDITOR
    [ContextMenu("Find Quests")]
    private void FindQuests()
    {
        FindQuestsBy<Quest>();
    }
    [ContextMenu("Find Achievement")]
    private void FindAchievements()
    {
        FindQuestsBy<Achievement>();
    }

    void FindQuestsBy<T>() where T : Quest
    {
        quests = new List<Quest>();

        // Asset 폴더에서 필터에 맞는 asset의 guid를 가져옴
        // guid : 유니티가 asset을 관리하기 위해 내부적으로 사용하는 ID
        // FindAssets : T타입, 그 하위 타입까지 모두 찾음
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var quest = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            // GetType()은 런타임에 실제 객체의 타입을 리턴
            if (quest.GetType() == typeof(T))
                quests.Add(quest);

            //QuestDatabase의 Serialize 변수의 변화가 생겼으니 반영함
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
