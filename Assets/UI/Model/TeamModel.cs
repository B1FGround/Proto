using UnityEngine;

public class TeamModel
{
    public void AddTeam(TeamManager.CharacterType type)
    {
        TeamManager.Instance.AddTeam(type);
        GameObject.FindWithTag("Player").GetComponent<TeamContainer>().SetTeam(TeamManager.Instance.GetTeamCount());
    }

    public void ChangeLeader(TeamManager.CharacterType type)
    {
        TeamManager.Instance.SetLeader(type);
        GameObject.FindWithTag("Player").GetComponent<TeamContainer>().SetTeam(TeamManager.Instance.GetTeamCount());
    }
}