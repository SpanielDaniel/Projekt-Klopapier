// File     : Wave_Spawner.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System.Collections;
using UnityEngine;

public class Wave_Spawner : MonoBehaviour
{
    [SerializeField] private Wave[] Waves;
    private Waypoints wPoints;
    public static int EnemiesAlive;
    private int WaveIndex = 0;

    private void Start()
    {
        wPoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    private void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }

        if (Timer.GetStunden == 6 && Timer.GetMinuten == 0)
        {
            StartCoroutine(WaveSpawn());
        }

    }

    IEnumerator WaveSpawn()
    {
        Wave wave = Waves[WaveIndex];

        for (int i = 0; i < wave.EnemyPrefab.Length; i++)
        {
            SpawnEnemy(wave.EnemyPrefab[i]);
        }
        yield return new WaitForSeconds(1f / wave.WaitTimeForSpawn);

        WaveIndex++;

        if (WaveIndex == Waves.Length)
        {
            gameObject.SetActive(false);
        }

    }

    public void SpawnEnemy(GameObject _enemy)
    {
        Instantiate(_enemy, wPoints.waypoints[0].position, Quaternion.identity);
        EnemiesAlive++;
    }
}
