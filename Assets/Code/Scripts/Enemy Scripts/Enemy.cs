// File     : Enemy.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;
using Assets.Code.Scripts.Enemy_Scripts;
using Buildings;
using Player;
using System.Collections.Generic;

//ToDo: Umschreiben

public class Enemy : MonoBehaviour
{
    private float AttackPoints;
    private float MaxHealthPoints;
    private float CurrentHealthPoints;
    private float Defence;
    private float MovementSpeed;
    private float Range;
    private int DamageToBase;
    private float distance;
    private int OnDeathPaperDrop;
    private Vector3 ViewDirection;

    private GameObject Target;
    private bool ReachedEnd;

    private float FireRate;
    private float CountdownShoot;

    private int WayPointIndex;
    private Waypoints wPoints;

    public bool OnEnterBase
    {
        get => ReachedEnd;
        set
        {
            ReachedEnd = value;
            GiveDamageToBase();
        }
    }

    private Animator Animator;
    [SerializeField] private GameObject RotateObject;
    [SerializeField] private GameObject MovingBody;
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
        Animator = GetComponent<Animator>();
    }

    public void SetWaypoints(Waypoints _wpoint)
    {
        wPoints = _wpoint;
    }

    private void Initialize(EnemyData _data)
    {
        MaxHealthPoints = _data.MaxHealthPoints;
        AttackPoints = _data.AttackPoints;
        Defence = _data.Defence;
        MovementSpeed = _data.MoveSpeed;
        Range = _data.Range;
        FireRate = _data.AttackSpeed;
        OnDeathPaperDrop = _data.PaperDrop;
        DamageToBase = _data.DamageToBase;
        CurrentHealthPoints = MaxHealthPoints;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wPoints.waypoints[WayPointIndex].transform.position, MovementSpeed * Time.deltaTime);
        Vector3 direction = (wPoints.waypoints[WayPointIndex].transform.position - transform.position);

        distance = direction.magnitude;
        ViewDirection = direction.normalized;

        float angle = Vector2.SignedAngle(Vector2.up, new Vector2(ViewDirection.x, ViewDirection.z));
        RotateObject.transform.eulerAngles = new Vector3(0, -angle, 0);
        MovingBody.transform.eulerAngles = new Vector3(0, -angle, 0);
        Animator.SetBool("IsShooting", false);
        if (Vector3.Distance(transform.position, wPoints.waypoints[WayPointIndex].transform.position) < 0.1f)
        {
            if (WayPointIndex < wPoints.waypoints.Count - 1)
                WayPointIndex++;
            else
                OnEnterBase = true;
        }

        if (CurrentHealthPoints <= 0)
        {
            Die();
        }

        UpdateTarget();

        if (Target != null)
        {
            LockOnTarget();
            if (CountdownShoot <= 0f)
            {
                Shoot(Target);
                CountdownShoot = FireRate;
            }

            CountdownShoot -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Look at Target
    /// </summary>
    private void LockOnTarget()
    {
        Vector3 direction = (Target.transform.position - RotateObject.transform.position);

        distance = direction.magnitude;
        ViewDirection = direction.normalized;


        float angle = Vector2.SignedAngle(Vector2.up, new Vector2(ViewDirection.x, ViewDirection.z));
        RotateObject.transform.eulerAngles = new Vector3(0, -angle, 0);
    }

    /// <summary>
    /// If Target found Shoot at Target
    /// </summary>
    /// <param name="Target"></param>
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

    /// <summary>
    /// Look for Target in Range
    /// </summary>
    private void UpdateTarget()
    {
        List<GameObject> enemies = new List<GameObject>();
        foreach (var item in Unit.Units)
        {
            enemies.Add(item.gameObject);
        }
        List<GameObject> buildings = new List<GameObject>();
        foreach (var item in Building.Buildings)
        {
            if (item is Base)
            {
                
            }
            else
            {
                buildings.Add(item.gameObject);
            }
        }
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

    /// <summary>
    /// Destroy Object, remove 1 point from Enemies alive and if hit last Point Damage Base
    /// </summary>
    public void Die()
    {
        Wave_Spawner.EnemiesAlive--;
        if (!OnEnterBase)
        {
            PlayerData.GetInstance.IncreaseToiletPaper(OnDeathPaperDrop);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Damage Base
    /// </summary>
    private void GiveDamageToBase()
    {
        foreach (Building building in Building.Buildings)
        {
            if (building is Base)
            {
                building.CurrentHealthH -= DamageToBase;
            }
        }
        Die();
    }

    /// <summary>
    /// if Bullet hit get Damage
    /// </summary>
    /// <param name="_dmg"></param>
    public void TakeDamage(int _dmg)
    {
        CurrentHealthPoints -= (_dmg - Defence);
    }
}
