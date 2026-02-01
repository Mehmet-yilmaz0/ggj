using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    [Header("Spawn Zones")]
    [SerializeField] Transform redZone;
    [SerializeField] Transform greenZone;
    [SerializeField] Transform blueZone;
    [SerializeField] Transform blackZone;

    [Header("Blocks")]
    [SerializeField] GameObject redBlockPrefab;
    [SerializeField] GameObject greenBlockPrefab;
    [SerializeField] GameObject blueBlockPrefab;
    [SerializeField] GameObject blackBlockPrefab;

    [Header("Settings")]
    public float maxDistance = 15f; 
    public int maxBlock = 15;
    public float spawnInterval = 5f;
    public float minSpawnRadius = 5f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true) 
        {
            SpawnController();
            yield return new WaitForSeconds(spawnInterval); 
        }
    }

    void SpawnController()
    {
        SpawnAroundPlayer(redBlockPrefab, redZone.transform.position);
        SpawnAroundPlayer(greenBlockPrefab, greenZone.transform.position);
        SpawnAroundPlayer(blueBlockPrefab, blueZone.transform.position);
        SpawnAroundPlayer(blackBlockPrefab, blackZone.transform.position);
    }

    void SpawnAroundPlayer(GameObject blockPrefab, Vector2 zonePosition)
    {
        float distToZone = Vector2.Distance(player.transform.position, zonePosition);

        if (distToZone > maxDistance)
        {
            return;
        }

        int countAroundPlayer = player.GetComponent<Player>().GetClosestEnemies(360f, 100f).Count;
        Debug.Log(countAroundPlayer);

        if (countAroundPlayer >= maxBlock) return;

        float safeDistance = Mathf.Max(1f, distToZone);

        float distanceFactor = Mathf.Pow(safeDistance, 0.5f);

        int spawnCount = (int)((maxBlock - countAroundPlayer) / distanceFactor);

        if (spawnCount <= 0 && maxBlock > countAroundPlayer) spawnCount = 1;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = GetRandomPosAround();
            Instantiate(blockPrefab, randomPos, Quaternion.identity);
        }
    }

    Vector2 GetRandomPosAround()
    {

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        float distance = minSpawnRadius + Random.Range(0f, 3f);

        Vector2 spawnPos = (Vector2)player.transform.position + (randomDirection * distance);

        return spawnPos;
    }
}