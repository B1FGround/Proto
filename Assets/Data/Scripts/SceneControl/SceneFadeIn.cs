using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : ISceneControlType
{
    private MonoBehaviour coroutineRunner;
    private GameObject sceneChangeEffectPrefab;
    private float fadeDuration;

    public SceneFadeIn(GameObject sceneChangeEffectPrefab,
                       float fadeDuration,
                       MonoBehaviour coroutineRunner)
    {
        this.sceneChangeEffectPrefab = sceneChangeEffectPrefab;
        this.fadeDuration = fadeDuration;
        this.coroutineRunner = coroutineRunner;
    }

    public void OnAction(string sceneName, Vector3 playerPos, Image sceneChangeBG, List<GameObject> characters)
    {
        coroutineRunner.StartCoroutine(FadeIn(playerPos, sceneChangeBG));
    }

    private IEnumerator FadeIn(Vector3 playerPos, Image sceneChangeBG)
    {
        Color color = sceneChangeBG.color;
        sceneChangeBG.gameObject.SetActive(true);
        var effect = Object.Instantiate(sceneChangeEffectPrefab, playerPos, Quaternion.identity);
        Object.Destroy(effect, fadeDuration);

        for (float t = fadeDuration; t > 0; t -= Time.deltaTime)
        {
            color.a = t / fadeDuration;
            sceneChangeBG.color = color;
            yield return null;
        }
        color.a = 0;
        sceneChangeBG.color = color;
        sceneChangeBG.gameObject.SetActive(false);
    }
}