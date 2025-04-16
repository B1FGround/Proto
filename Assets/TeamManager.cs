using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class TeamManager : Singleton<TeamManager>
{
    public enum CharacterType
    {
        Ranger,
        Warrior,
        Magician
    }

    private CharacterType leader;
    private List<CharacterType> currentTeam;

    protected override void Awake()
    {
        base.Awake();
        Instance.leader = CharacterType.Ranger;
        Instance.currentTeam = new List<CharacterType>() { leader };
    }

    public void SetLeader(CharacterType newLeader)
    {
        if (leader == newLeader)
            return;

        currentTeam.Remove(newLeader);
        currentTeam.Insert(0, newLeader);
        leader = newLeader;
    }

    public void AddTeam(CharacterType newCharacter)
    {
        if (currentTeam.Contains(newCharacter))
            return;
        currentTeam.Add(newCharacter);
    }

    public int GetTeamCount()
    {
        return currentTeam.Count;
    }

    public CharacterType GetCharacterType(int index)
    {
        return currentTeam[index];
    }
}