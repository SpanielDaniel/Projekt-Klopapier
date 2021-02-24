// File     : Wave_Spawner.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System.Collections;
using UnityEngine;

public class Wave_Spawner : MonoBehaviour
{
    [SerializeField] private Wave[] Waves;
    private Waypoints wPoints;
    private float CountDown;
    private float StartCountDown; //ToDo: combine with Timer
    public static int EnemiesAlive;
    private int WaveIndex = 0;

    private void Start()
    {
        wPoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
        CountDown = StartCountDown;
    }

    private void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }

        if (CountDown <= 0f)
        {
            StartCoroutine(WaveSpawn());
            CountDown = StartCountDown;
        }

        CountDown -= Time.deltaTime;
    }

    IEnumerator WaveSpawn()
    {
        Wave wave = Waves[WaveIndex];

        for (int i = 0; i < wave.Count; i++)
        {
            SpawnEnemy(wave.EnemyPrefab[i]);
            yield return new WaitForSeconds(1f / wave.Rate);
        }

        WaveIndex++;

        if (WaveIndex == Waves.Length)
        {
            //ToDo Waves besiegt / gewonnen
            this.enabled = false;
            Debug.Log("Ez Win");
        }
    }

    public void SpawnEnemy(GameObject _enemy)
    {
        Instantiate(_enemy, wPoints.waypoints[0].position, Quaternion.identity);
        EnemiesAlive++;
    }
}
