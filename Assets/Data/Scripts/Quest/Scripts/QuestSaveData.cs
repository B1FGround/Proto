public struct QuestSaveData
{
    public string codeName;
    public QuestState state;
    public int taskGroupIndex;
    public int[] taskSuccessCounts;

    public QuestSaveData(string codeName, QuestState state, int taskGroupIndex, int[] taskSuccessCounts)
    {
        this.codeName = codeName;
        this.state = state;
        this.taskGroupIndex = taskGroupIndex;
        this.taskSuccessCounts = taskSuccessCounts;
    }
}

