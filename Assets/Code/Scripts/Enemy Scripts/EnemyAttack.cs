using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;
    public float range = 2.5f;

    public float fireRate;
    public float countdownShoot;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    private void Update()
    {
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
}
