using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class SceneController : Singleton<SceneController>
{
    private GameObject sceneChangeEffectPrefab;
    private readonly float fadeDuration = 1.5f;
    private ISceneControlType fadeIn = null;
    private ISceneControlType fadeOut = null;

    protected override void Awake()
    {
        base.Awake();
        Instance.sceneChangeEffectPrefab = Resources.Load<GameObject>("Prefabs/Effect/SceneChange");
        Instance.fadeIn = new SceneFadeIn(sceneChangeEffectPrefab, fadeDuration, this);
        Instance.fadeOut = new SceneFadeOut(sceneChangeEffectPrefab, fadeDuration, this);
    }

    public void SceneChange(string sceneName, Vector3 playerPos, List<GameObject> characters, Image sceneChangeBG)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        fadeOut.OnAction(sceneName, playerPos, sceneChangeBG, characters);
        var pos = playerPos;
        pos.y += 0.75f;
        Instantiate(Resources.Load<GameObject>("Prefabs/Effect/SpawnEffect"), pos, Quaternion.identity);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        CameraManager.Instance.InitCameraSetting();
        var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        var teamContainer = GameObject.FindWithTag("Player").GetComponent<TeamContainer>();
        teamContainer.Initialized();
        teamContainer.SetTeam(TeamManager.Instance.GetTeamCount());

        var pos = playerController.transform.position;
        pos.y += 0.75f;
        Instantiate(Resources.Load<GameObject>("Prefabs/Effect/SpawnEffect"), pos, Quaternion.identity);

        fadeIn.OnAction(scene.name, playerController.gameObject.transform.position, playerController.sceneChangeBg, teamContainer.GetCharacters());
    }
}