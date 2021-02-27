// File     : Wave_Spawner.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System.Collections;
using Code.Scripts.Map;
using Code.Scripts.Wave_Scripts;
using UnityEngine;

public class Wave_Spawner : MonoBehaviour
{
    [SerializeField] private Wave[] Waves;
    [SerializeField] private MapGenerator MapGenerator;
    private Waypoints wPoints = new Waypoints();
    public static int EnemiesAlive;
    private int WaveIndex = 0;
    

    private void Start()
    {
        for (int j = 0; j < MapGenerator.GetWaypoints.GetHeight; j++)
        {
            for (int k = 0; k < MapGenerator.GetWaypoints.GetWidth; k++)
            {
                MapGenerator.GetWaypoints.Grid[j, k].GetComponent<Waypoint>().ShowSphere(false);
            }
        }
    }

    private void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }

        int randomValue = 0;
        randomValue = Random.Range(0, MapGenerator.GetPathes.Count);

        if (Timer.GetStunden == 6 && Timer.GetMinuten == 0 && Timer.GetDay >= 1)
        {
            wPoints.SetWayPoints(MapGenerator.GetPathes[randomValue]);
            foreach (var waypoint in MapGenerator.GetPathes[randomValue])
            {
                waypoint.GetComponent<Waypoint>().ShowSphere(true);
            }


            
            
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
        GameObject enemy = Instantiate(_enemy, wPoints.waypoints[0].transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().SetWaypoints(wPoints);
        EnemiesAlive++;
    }
}
