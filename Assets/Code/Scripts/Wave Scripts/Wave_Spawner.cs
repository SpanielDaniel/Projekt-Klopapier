// File     : Wave_Spawner.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System.Collections;
using Code.Scripts.Map;
using Code.Scripts.Wave_Scripts;
using UnityEngine;

public class Wave_Spawner : MonoBehaviour
{
    // -------------------------------------------------------------------------------------------------------------

    #region Init

    // Serialize Fields --------------------------------------------------------------------------------------------

    [SerializeField] private Wave[] Waves;
    [SerializeField] private MapGenerator MapGenerator;

    // Static Variables --------------------------------------------------------------------------------------------

    public static int EnemiesAlive;

    // private -----------------------------------------------------------------------------------------------------

    private Waypoints wPoints = new Waypoints();
    private int WaveIndex = 0;

    #endregion

    // -------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        WaveFinished();
    }

    private void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }

        //Set Waypoints and Spawn Wave
        if (Timer.GetStunden == 6 && Timer.GetMinuten == 0 && Timer.GetDay >= 1)
        {
            int randomValue = 0;
            randomValue = Random.Range(0, MapGenerator.GetPathes.Count);
            wPoints.SetWayPoints(MapGenerator.GetPathes[randomValue]);
            foreach (var waypoint in MapGenerator.GetPathes[randomValue])
            {
                waypoint.GetComponent<Waypoint>().ShowSphere(true);
            }

            StartCoroutine(WaveSpawn());
        }

        if (EnemiesAlive <= 0)
        {
            WaveFinished();
        }
    }

    /// <summary>
    /// Set Wave with Enemys and check if finished all Waves for win or not
    /// </summary>
    /// <returns></returns>
    IEnumerator WaveSpawn()
    {
        Wave wave = Waves[WaveIndex];

        for (int i = 0; i < wave.EnemyPrefab.Length; i++)
        {
            SpawnEnemy(wave.EnemyPrefab[i]);
        }
        yield return new WaitForSeconds(wave.WaitTimeForSpawn * Time.deltaTime);

        WaveIndex++;
        if (WaveIndex == Waves.Length)
        {
            gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// Hide Way from Enemies after finished Wave
    /// </summary>
    private void WaveFinished()
    {
        for (int j = 0; j < MapGenerator.GetWaypoints.GetHeight; j++)
        {
            for (int k = 0; k < MapGenerator.GetWaypoints.GetWidth; k++)
            {
                MapGenerator.GetWaypoints.Grid[j, k].GetComponent<Waypoint>().ShowSphere(false);
            }
        }
    }

    /// <summary>
    /// Instantiate Enemy as GameObject 
    /// </summary>
    /// <param name="_enemy">Enemy Prefab</param>
    public void SpawnEnemy(GameObject _enemy)
    {
        GameObject enemy = Instantiate(_enemy, wPoints.waypoints[0].transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().SetWaypoints(wPoints);
        EnemiesAlive++;
    }
}
