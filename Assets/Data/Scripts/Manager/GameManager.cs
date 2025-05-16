using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private float maxGameTimer = 60;
    private float currentGameTimer = 0;
    private float miniBossTime = 30;
    private bool miniBossSpawned = false;
    private bool bossSpawned = false;
    private Coroutine game;

    public enum GameState
    {
        Early,
        Middle,
        Late,
    }

    public GameState gameState = GameState.Early;

    public void StartGameTimer()
    {
        if (gameState == GameState.Late)
            return;

        gameState = GameState.Early;
        game = StartCoroutine(GameTimer());
    }

    public void StopGameTimer()
    {
        if (game == null)
            return;

        StopCoroutine(game);
        game = null;
    }

    public void ResetTimer()
    {
        StopGameTimer();
        currentGameTimer = 0;
        miniBossSpawned = false;
        bossSpawned = false;
        gameState = GameState.Early;
    }

    private IEnumerator GameTimer()
    {
        while (true)
        {
            currentGameTimer += Time.deltaTime;

            int curTimeToInt = (int)currentGameTimer;
            var timerUI = GameObject.Find("GameTimerUI");
            if (timerUI != null)
                timerUI.GetComponent<TMP_Text>().text = curTimeToInt.ToString() + " / " + maxGameTimer.ToString();

            if (currentGameTimer >= maxGameTimer && bossSpawned == false)
            {
                bossSpawned = true;
                gameState = GameState.Late;
                var bossPrefab = Resources.Load("Prefabs/Object/Boss");
                var boss = Instantiate(bossPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                boss.GetComponent<MonsterController>().minimumDistance = 5f;
                boss.GetComponent<MonsterController>().monsterType = MonsterController.MonsterType.Boss;
                boss.transform.SetParent(GameObject.Find("Monsters").transform);
                boss.GetComponent<MonsterController>().hp = 10;
                yield break;
            }
            if (currentGameTimer >= miniBossTime && miniBossSpawned == false)
            {
                var miniBossPrefab = Resources.Load("Prefabs/Object/MiniBoss");
                var miniBoss = Instantiate(miniBossPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                miniBoss.GetComponent<MonsterController>().minimumDistance = 5f;
                miniBoss.GetComponent<MonsterController>().monsterType = MonsterController.MonsterType.Elite;
                gameState = GameState.Middle;
                miniBossSpawned = true;
                miniBoss.transform.SetParent(GameObject.Find("Monsters").transform);
                miniBoss.GetComponent<MonsterController>().hp = 10;
            }

            yield return null;
        }
    }
}