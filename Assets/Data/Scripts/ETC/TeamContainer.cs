using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class TeamContainer : MonoBehaviour
{
    private GameObject Team_1;
    private GameObject Team_2;
    private GameObject Team_3;
    private GameObject Team_4;

    private GameObject currentTeam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Dungeon")
        {
            return;
        }
        Team_1 = this.gameObject.transform.Find("Team_1").gameObject;
        Team_2 = this.gameObject.transform.Find("Team_2").gameObject;
        Team_3 = this.gameObject.transform.Find("Team_3").gameObject;
        Team_4 = this.gameObject.transform.Find("Team_4").gameObject;
        currentTeam = Team_1;

        SetTeam(1);
    }

    public void Initialized()
    {
        Team_1 = this.gameObject.transform.Find("Team_1").gameObject;
        Team_2 = this.gameObject.transform.Find("Team_2").gameObject;
        Team_3 = this.gameObject.transform.Find("Team_3").gameObject;
        Team_4 = this.gameObject.transform.Find("Team_4").gameObject;
        currentTeam = Team_1;

        //SetTeam(1);
    }

    public void SetTeam(int team)
    {
        ClearCharacter();
        switch (team)
        {
            case 1:
                Team_1.SetActive(true);
                Team_2.SetActive(false);
                Team_3.SetActive(false);
                Team_4.SetActive(false);
                currentTeam = Team_1;
                break;

            case 2:
                Team_1.SetActive(false);
                Team_2.SetActive(true);
                Team_3.SetActive(false);
                Team_4.SetActive(false);
                currentTeam = Team_2;
                break;

            case 3:
                Team_1.SetActive(false);
                Team_2.SetActive(false);
                Team_3.SetActive(true);
                Team_4.SetActive(false);
                currentTeam = Team_3;
                break;

            case 4:
                Team_1.SetActive(false);
                Team_2.SetActive(false);
                Team_3.SetActive(false);
                Team_4.SetActive(true);
                currentTeam = Team_4;
                break;
        }
        AddCharacter();
    }

    public List<GameObject> GetCharacters()
    {
        List<GameObject> characters = new List<GameObject>();
        for (int i = 0; i < currentTeam.transform.childCount; i++)
        {
            var slot = currentTeam.transform.GetChild(i);
            var character = slot.transform.GetChild(0).gameObject;

            characters.Add(character);
        }
        return characters;
    }

    private void ClearCharacter()
    {
        if (currentTeam == null)
            currentTeam = Team_1;
        for (int i = 0; i < currentTeam.transform.childCount; i++)
        {
            var slot = currentTeam.transform.GetChild(i);
            var character = slot.transform.GetChild(0).gameObject;

            Destroy(character);
        }
    }

    private void AddCharacter()
    {
        var characterPrefab = Resources.Load("Prefabs/Object/Character");
        for (int i = 0; i < currentTeam.transform.childCount; i++)
        {
            var slot = currentTeam.transform.GetChild(i);

            var character = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity, slot) as GameObject;
            character.transform.localPosition = Vector3.zero;
            character.GetComponent<CharacterAttack>().Initialized((TeamManager.Instance.GetCharacterType(i)));
        }
    }
}