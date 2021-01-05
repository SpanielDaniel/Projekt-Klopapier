using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private int speed = 3;
    private int wayPointIndex;
    private Waypoints wPoints;

    private void Start()
    {
        wPoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wPoints.waypoints[wayPointIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, wPoints.waypoints[wayPointIndex].position) < 0.1f)
        {
            if (wayPointIndex < wPoints.waypoints.Length - 1)
                wayPointIndex++;
            else
                Destroy(gameObject);
        }

    }
}
