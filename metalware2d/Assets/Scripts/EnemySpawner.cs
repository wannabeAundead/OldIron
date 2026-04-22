using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1.5f;
    public float spawnRadius = 10f;
    public int maxEnemies = 30;

    private float nextSpawnTime;
    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing) return;
        if (enemyPrefab == null || player == null) return;
        if (Time.time < nextSpawnTime) return;

        GameObject[] existing = GameObject.FindGameObjectsWithTag("Enemy");
        if (existing.Length >= maxEnemies) return;

        nextSpawnTime = Time.time + spawnInterval;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * spawnRadius;
        Vector3 spawnPos = player.position + offset;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
