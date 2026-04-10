using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform EnemySpawnPoint;
    public float EnemySpawnDelay = 5f;
    public AudioClip WarMusic;
    public void StartWaves()
    {
        SoundManager.Instance.PlayMusic(WarMusic);
        StartCoroutine(SpawnEnemies());
    }
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Instantiate(EnemyPrefab, EnemySpawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(EnemySpawnDelay); 
        }
    }
}
