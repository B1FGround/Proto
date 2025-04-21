using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject monsters;
    [SerializeField] private GameObject player;

    private float spawnRate = 5f;
    private float curSpawnRate = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        curSpawnRate = spawnRate;
    }

    // Update is called once per frame
    private void Update()
    {
        SpawnMonster();
    }

    private void SpawnMonster()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Late)
            return;

        curSpawnRate -= Time.deltaTime;

        if (curSpawnRate <= 0)
        {
            curSpawnRate = spawnRate;
            var monsterObj = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            monsterObj.transform.SetParent(monsters.transform);
        }
    }
}