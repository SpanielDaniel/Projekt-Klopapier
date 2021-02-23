// File     : Enemy.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;
using Assets.Code.Scripts.Enemy_Scripts;

//ToDo: Umschreiben

public class Enemy : MonoBehaviour
{
    private float AttackPoints;
    private float MaxHealthPoints;
    private float CurrentHealthPoints;
    private float Defence;
    private float MovementSpeed;
    private float Range ;

    private GameObject Target;

    private float FireRate;
    private float CountdownShoot;

    private int WayPointIndex;
    private Waypoints wPoints;

    private Animator Animator;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private EnemyData Data;

    public EnemyData GetEnemyData => Data;

    private void Awake()
    {
        Initialize(Data);
    }

    private void Start()
    {
        //wPoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
        Animator = GetComponent<Animator>();
    }

    private void Initialize(EnemyData _data)
    {
        MaxHealthPoints = _data.MaxHealthPoints;
        AttackPoints = _data.AttackPoints;
        Defence = _data.Defence;
        MovementSpeed = _data.MoveSpeed;
        Range = _data.Range;
        FireRate = _data.AttackSpeed;
        CurrentHealthPoints = MaxHealthPoints;
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, wPoints.waypoints[WayPointIndex].position, BewegungsGeschwindigkeit * Time.deltaTime);

        //if (Vector3.Distance(transform.position, wPoints.waypoints[WayPointIndex].position) < 0.1f)
        //{
        //    if (WayPointIndex < wPoints.waypoints.Length - 1)
        //        WayPointIndex++;
        //}

        Animator.SetBool("IsShooting", false);

        if (CurrentHealthPoints <= 0)
        {
            Destroy(gameObject);
            Die();
        }
        UpdateTarget();

        if (Target == null)
            return;

        if (CountdownShoot <= 0f)
        {
            Shoot(Target);
            CountdownShoot = FireRate;
        }

        CountdownShoot -= Time.deltaTime;
    }

    void Shoot(GameObject Target)
    {
        Animator.SetBool("IsShooting", true);
        GameObject bulletGO = (GameObject)Instantiate(BulletPrefab, FirePoint.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(Target);
            bullet.SetDMGValue(AttackPoints);
        }
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("PlayerUnit");
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        float shortestDistanceToEnemy = Mathf.Infinity;
        float shortestDistanceToBuilding = Mathf.Infinity;
        GameObject nearestEnemy = null;
        GameObject nearestBuilding = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistanceToEnemy)
            {
                shortestDistanceToEnemy = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        foreach (GameObject building in buildings)
        {
            float distanceToBuilding = Vector3.Distance(transform.position, building.transform.position);
            if (distanceToBuilding < shortestDistanceToBuilding)
            {
                shortestDistanceToBuilding = distanceToBuilding;
                nearestBuilding = building;
            }
        }

        if (nearestEnemy != null && shortestDistanceToEnemy <= Range)
            Target = nearestEnemy;
        else if (nearestBuilding != null && shortestDistanceToBuilding <= Range)
            Target = nearestBuilding;
        else
            Target = null;
    }

    public void Die()
    {
        Wave_Spawner.EnemiesAlive--;
    }

    public void GetDMG(float _dmg)
    {
        CurrentHealthPoints -= (_dmg - Defence);
    }
}
