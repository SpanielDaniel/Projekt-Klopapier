using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    private float timer;
    private float resetTimer;

    [SerializeField]
    private GameObject enemy;
    private Waypoints wPoints;

    private void Start()
    {
        resetTimer = 15f;
        timer = resetTimer;
        wPoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    private void Update()
    {
        if (timer <= 0)
        {
            Instantiate(enemy, wPoints.waypoints[0].position, Quaternion.identity);
            timer = resetTimer;
        }
        else
            timer -= Time.deltaTime;
    }
}
