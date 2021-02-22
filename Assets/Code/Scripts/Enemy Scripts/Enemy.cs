// File     : Enemy.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;

//ToDo: Umschreiben

public class Enemy : MonoBehaviour
{
    public float SchadensPunkte = 10;
    public float LebensPunkte = 100;
    public float VerteidigungsPunkte = 10;
    public float BewegungsGeschwindigkeit = 2;

    private GameObject Target;
    private float Range = 2.5f;

    private float FireRate;
    private float CountdownShoot;

    private int WayPointIndex;
    private Waypoints wPoints;

    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform FirePoint;

    private void Start()
    {
        //wPoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, wPoints.waypoints[WayPointIndex].position, BewegungsGeschwindigkeit * Time.deltaTime);

        //if (Vector3.Distance(transform.position, wPoints.waypoints[WayPointIndex].position) < 0.1f)
        //{
        //    if (WayPointIndex < wPoints.waypoints.Length - 1)
        //        WayPointIndex++;
        //}

        if (LebensPunkte <= 0)
        {
            Destroy(gameObject);
            Die();
        }
        UpdateTarget();

        if (Target == null)
            return;

        StopMoving();

        if (CountdownShoot <= 0f)
        {
            Shoot(Target);
            CountdownShoot = FireRate;
        }

        CountdownShoot -= Time.deltaTime;
    }

    void Shoot(GameObject Target)
    {
        GameObject bulletGO = (GameObject)Instantiate(BulletPrefab, FirePoint.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(Target);
            bullet.SetDMGValue(SchadensPunkte);
        }
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

        if (nearestEnemy != null && shortestDistance <= Range)
            Target = nearestEnemy;
        else
            Target = null;
    }

    public void Die()
    {
        Wave_Spawner.EnemiesAlive--;
    }

    public void StopMoving()
    {
        if (Target != null)
        {
            BewegungsGeschwindigkeit = 0;
        }
        else
        {
            BewegungsGeschwindigkeit = 20;
        }
    }

    public void GetDMG(float _dmg)
    {
        LebensPunkte -= (_dmg - VerteidigungsPunkte);
    }
}
