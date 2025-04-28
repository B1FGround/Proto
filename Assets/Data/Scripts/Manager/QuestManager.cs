using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    #region Save Path
    const string SaveRootPath = "questSystem";
    const string ActiveQuestsSavePath = "activeQuests";
    const string CompletedQuestsSavePath = "completedQuests";
    const string ActiveAchievementsSavePath = "activeAchievements";
    const string CompletedAchievementsSavePath = "completedAchievements";
    #endregion
    #region Events
    public delegate void QuestRegisteredHandler(Quest quest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);
    #endregion
    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();

    private List<Quest> activeAchievements = new List<Quest>();
    private List<Quest> completedAchievements = new List<Quest>();

    public event QuestRegisteredHandler QuestRegistered;
    public event QuestCompletedHandler QuestCompleted;
    public event QuestCanceledHandler QuestCanceled;

    public event QuestRegisteredHandler AchievementRegistered;
    public event QuestCompletedHandler AchievementCompleted;

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => completedQuests;
    public IReadOnlyList<Quest> ActiveAchievements => activeAchievements;
    public IReadOnlyList<Quest> CompletedAchievements => completedAchievements;

    QuestDatabase questDatabase;
    QuestDatabase achievementDatabase;

    protected override void Awake()
    {
        base.Awake();
        questDatabase = Resources.Load<QuestDatabase>("Data/QuestDatabase");
        achievementDatabase = Resources.Load<QuestDatabase>("Data/AchievementDatabase");

        if(!Load())
        {
            foreach(var achievement in achievementDatabase.Quests)
                Register(achievement);
        }
    }

    public Quest Register(Quest quest)
    {
        //var newQuest = Instantiate(quest);
        // quest를 인스턴스화 할때 안에있는 Task들은 원본과 같은 값을 바라봄
        // 만약 해당 Task를 가진 다른 퀘스트가 있고, 그 퀘스트 내에서 Task의 값을 바꾼다면 바라보는 모든 퀘스트의 Task값이 바뀌게 됨
        // 퀘스트 내부의 값 또한 복사(인스턴스화) 해줘야 하는데 Quest 클래스가 수정되어 모듈이 추가되면 QuestManager에서도 인스턴스화 해주는 코드가 추가되어야함

        // Quest class 내부에서 자신의 복사본을 만들어 내보내주는거로 해결 (Cloning)

        var newQuest = quest.Clone();
        if (newQuest is Achievement)
        {
            newQuest.OnComplete += OnAchievementCompleted;
            activeAchievements.Add(newQuest);
            newQuest.OnRegister();
            AchievementRegistered?.Invoke(newQuest);
        }
        else if (newQuest is Quest)
        {
            newQuest.OnComplete += OnQuestCompleted;
            newQuest.OnCanceled += OnQuestCanceled;

            activeQuests.Add(newQuest);
            newQuest.OnRegister();
            QuestRegistered?.Invoke(newQuest);
        }

        return newQuest;
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(activeQuests, category, target, successCount);
        ReceiveReport(activeAchievements, category, target, successCount);
    }
    public void ReceiveReport(Category category, TaskTarget target, int successCount)
     => ReceiveReport(category.CodeName, target.Value, successCount);
    void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    {
        // for문이 돌아가는 도중 Quest Complete로 인해 원본 목록이 바뀌어 버리면 Error가 생기기 때문에 ToArray로 함
        foreach (var quest in quests.ToArray())
            quest.ReceiveReport(category, target, successCount);
    }

    public bool ContainsInActiveQuests(Quest quest) => activeQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInCompletedQuests(Quest quest) => completedQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInActiveAchievements(Quest achievement) => activeAchievements.Any(x => x.CodeName == achievement.CodeName);
    public bool ContainsInCompletedAchievements(Quest achievement) => completedAchievements.Any(x => x.CodeName == achievement.CodeName);

    #region Save & Load
    public void Save()
    {
        var root = new JObject();
        root.Add(ActiveQuestsSavePath, CreateSaveDatas(activeQuests));
        root.Add(CompletedQuestsSavePath, CreateSaveDatas(completedQuests));
        root.Add(ActiveAchievementsSavePath, CreateSaveDatas(activeAchievements));
        root.Add(CompletedAchievementsSavePath, CreateSaveDatas(completedAchievements));

        PlayerPrefs.SetString(SaveRootPath, root.ToString());
        PlayerPrefs.Save();
    }
    bool Load()
    {
        if(PlayerPrefs.HasKey(SaveRootPath))
        {
            var root = JObject.Parse(PlayerPrefs.GetString(SaveRootPath));

            LoadSaveDatas(root[ActiveQuestsSavePath], questDatabase, LoadActiveQuest);
            LoadSaveDatas(root[CompletedQuestsSavePath], questDatabase, LoadCompletedQuest);

            LoadSaveDatas(root[ActiveAchievementsSavePath], achievementDatabase, LoadActiveQuest);
            LoadSaveDatas(root[CompletedAchievementsSavePath], achievementDatabase, LoadCompletedQuest);

            return true;
        }

        return false;
    }

    JArray CreateSaveDatas(IReadOnlyList<Quest> quests)
    {
        var saveDatas = new JArray();
        foreach (var quest in quests)
        {
            if(quest.IsSavable)
                saveDatas.Add(JObject.FromObject(quest.ToSaveData()));
        }

        return saveDatas;
    }

    // Save할때 만들어진 saveDatas(JArray)가 datasToken로 들어옴
    void LoadSaveDatas(JToken datasToken, QuestDatabase database, Action<QuestSaveData, Quest> onSuccess)
    {
        var datas = datasToken as JArray;
        foreach(var data in datas)
        {
            var saveData = data.ToObject<QuestSaveData>();
            var quest = database.FindQuestBy(saveData.codeName);
            onSuccess.Invoke(saveData, quest);
        }
    }
    void LoadActiveQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = Register(quest);
        newQuest.LoadFrom(saveData);
    }

    void LoadCompletedQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.LoadFrom(saveData);

        if (newQuest is Achievement)
            completedAchievements.Add(quest);
        else
            completedQuests.Add(quest);
    }


    #endregion

    #region Callback
    void OnQuestCompleted(Quest quest)
    {
        activeQuests.Remove(quest);
        completedQuests.Remove(quest);

        QuestCompleted?.Invoke(quest);
    }
    void OnQuestCanceled(Quest quest)
    {
        activeQuests.Remove(quest);

        QuestCanceled?.Invoke(quest);

        DestroyImmediate(quest);
    }

    void OnAchievementCompleted(Quest achievement)
    {
        activeAchievements.Remove(achievement);
        completedAchievements.Add(achievement);

        AchievementCompleted?.Invoke(achievement);
    }
    #endregion
}
