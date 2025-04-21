using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeOut : ISceneControlType
{
    private MonoBehaviour coroutineRunner;
    private GameObject sceneChangeEffectPrefab;
    private float fadeDuration;

    public SceneFadeOut(GameObject sceneChangeEffectPrefab,
                       float fadeDuration,
                       MonoBehaviour coroutineRunner)
    {
        this.sceneChangeEffectPrefab = sceneChangeEffectPrefab;
        this.fadeDuration = fadeDuration;
        this.coroutineRunner = coroutineRunner;
    }

    public void OnAction(string sceneName, Vector3 playerPos, Image sceneChangeBG, List<GameObject> characters)
    {
        coroutineRunner.StartCoroutine(FadeOut(sceneName, characters, sceneChangeBG));
        var effect = Object.Instantiate(sceneChangeEffectPrefab, playerPos, Quaternion.identity);
        Object.Destroy(effect, fadeDuration);
    }

    private IEnumerator FadeOut(string sceneName, List<GameObject> characters, Image sceneChangeBG)
    {
        Color color = sceneChangeBG.color;
        sceneChangeBG.gameObject.SetActive(true);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            color.a = t / fadeDuration;

            if (t > fadeDuration / 2)
            {
                foreach (var character in characters)
                {
                    character.SetActive(false);
                }
            }
            sceneChangeBG.color = color;
            yield return null;
        }
        color.a = 1;
        sceneChangeBG.color = color;

        SceneManager.LoadScene(sceneName);
        GameManager.Instance.StartGameTimer();
    }
}