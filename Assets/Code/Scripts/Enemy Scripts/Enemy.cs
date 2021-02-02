// File     : Enemy.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;

//ToDo: Umschreiben

public class Enemy : MonoBehaviour
{
    private int SchadensPunkte;
    private int LebensPunkte;
    private int VerteidigungsPunkte;
    private int BewegungsGeschwindigkeit = 2;

    private Transform target;
    private float range = 2.5f;

    private float fireRate;
    private float countdownShoot;

    private int wayPointIndex;
    private Waypoints wPoints;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
        wPoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wPoints.waypoints[wayPointIndex].position, BewegungsGeschwindigkeit * Time.deltaTime);

        if (Vector3.Distance(transform.position, wPoints.waypoints[wayPointIndex].position) < 0.1f)
        {
            if (wayPointIndex < wPoints.waypoints.Length - 1)
                wayPointIndex++;
            else
                Destroy(gameObject);
        }

        if (target == null)
            return;

        if (countdownShoot <= 0f)
        {
            Shoot();
            countdownShoot = 1f / fireRate;
        }

        countdownShoot -= Time.deltaTime;
    }

    void Shoot()
    {
        Debug.Log("Dmg");
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("PlayerUnit");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
            target = nearestEnemy.transform;
        else
            target = null;
    }

    public void Die()
    {
        //ToDo: Enemys bearbeiten
        Wave_Spawner.EnemiesAlive--;
    }
}
