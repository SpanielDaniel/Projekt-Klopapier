// File     : Unit.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.Unit_Scripts;
using Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using Buildings;
using Code.Scripts;
using Code.Scripts.Map;
using UnityEngine;

public class Unit : MonoBehaviour
    ,IMouseLeftClick
{
    public static event Action<Unit> OnSelection;
    public static event Action<Unit> IsSpawned; 
    
    private float MaxHealthPoints;
    private float CurrentHealthPoints;
    private float Defence;
    private float AttackPoints;
    private float AttackSpeed;
    private float Range;
    private int ID;
    private string Name;
    private Sprite Icon;
    private float CountDownShoot;

    public bool IsSelected;

    [SerializeField] private UnitData UnitData;
    [SerializeField] private int XPos;
    [SerializeField] private int ZPos;
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject UnitObj;
    [SerializeField] private GameObject SelectedGround;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform FirePoint;

    public UnitData GetUnitData => UnitData;

    private float MoveSpeed;

    public int GetXPosition => XPos;
    public int GetZPosition => ZPos;

    private List<Node> Path;
    private int NextNode = 1;
    private Vector3 ViewDirection;
    private bool IsMoving = false;
    private bool IsMovingIntoBuilding;
    private static GameObject Target;

    private bool isMoving = false;
    private float distance = 0f;

    
    public static List<Unit> Units = new List<Unit>();
    

    private void Awake()
    {
        Initialize(UnitData);
        AddUnit(this);
    }
    
    /// <summary>
    /// Initialize unit
    /// </summary>
    /// <param name="_data">The data from unit</param>
    /// <param name="_pos">Position where the unit spawns</param>
    public void Initialize(UnitData _data)
    {
        Name = _data.Name;
        Icon = _data.Icon;
        MaxHealthPoints = _data.MaxHealthPoints;
        Defence = _data.Defence;
        AttackPoints = _data.AttackPoints;
        AttackSpeed = _data.AttackSpeed;
        MoveSpeed = _data.MoveSpeed;
    }

    private void Start()
    {
        IsSelected = false;
        SetSelectedGround(false);
        UpdatePos();
    }

    private void Update()
    {
        if (UnitData.MaxHealthPoints <= 0)
        {
            Destroy(gameObject);
        }

        if (IsMoving)
        {
            Animator.SetBool("IsMoving", true);
            Animator.SetBool("IsIdle", false);

            if (distance < 0.1f)
            {
                XPos = Path[NextNode].GridX;
                ZPos = Path[NextNode].GridZ;
                NextNode++;
            }
            
            if(NextNode  < Path.Count)
            {
                Vector3 direction = (Path[NextNode].Pos - transform.position);
                
                distance = direction.magnitude;
                ViewDirection = direction.normalized;
                
                
                float angle = Vector2.SignedAngle(Vector2.up, new Vector2(ViewDirection.x, ViewDirection.z));
                UnitObj.transform.eulerAngles = new Vector3(0,-angle,0);
            }

            transform.position += ViewDirection * (MoveSpeed * Time.deltaTime);

            if (NextNode >= Path.Count)
            {
                transform.position = Path[NextNode - 1].Pos;
                Path = null;
                IsMoving = false;
                
                Animator.SetBool("IsMoving", false);
                Animator.SetBool("IsIdle", true);
                if (IsMovingIntoBuilding)
                {
                    if (BuildingToEnter.AddUnit(ID))
                    {
                        gameObject.SetActive(false);
                    }
                    IsMovingIntoBuilding = false;
                }
            }
        }
        UpdateTarget();

        if (Target == null)
        {
            return;
        }

        LockOnTarget();

        if (CountDownShoot <= 0f)
        {
            Attack(Target);
            CountDownShoot = GetUnitData.AttackSpeed;
        }

        CountDownShoot -= Time.deltaTime;
    }

    /// <summary>
    /// Add unit to List.
    /// </summary>
    /// <param name="_unit">Added Unit</param>
    public void AddUnit(Unit _unit)
    {
        Units.Add(_unit);
        ID = Units.Count - 1;
    }

    public void UpdateTarget()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        float shortDis = Mathf.Infinity;
        GameObject nearEnemy = null;

        foreach (GameObject enemy in enemys)
        {
            float dis = Vector3.Distance(transform.position, enemy.transform.position);
            if (dis < shortDis)
            {
                shortDis = dis;
                nearEnemy = enemy;
            }
        }

        if (nearEnemy != null && shortDis <= GetUnitData.Range)
        {
            Target = nearEnemy;
        }
    }

    public void LockOnTarget()
    {
        Vector3 direction = (Target.transform.position - transform.position);

        distance = direction.magnitude;
        ViewDirection = direction.normalized;


        float angle = Vector2.SignedAngle(Vector2.up, new Vector2(ViewDirection.x, ViewDirection.z));
        UnitObj.transform.eulerAngles = new Vector3(0, -angle, 0);
    }

    public void Attack(GameObject _enemy)
    {
        GameObject bulletGO = (GameObject)Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        
        if (bullet != null)
        {
            bullet.Seek(Target);
            bullet.SetDMGValue(AttackPoints);
        }
    }

    /// <summary>
    /// Move unit to a position.
    /// </summary>
    /// <param name="_x">X coordinate from world</param>
    /// <param name="_y">Y coordinate from world</param>
    public void MoveToPosition(List<Node> _path)
    {
        
        NextNode = 1;
        distance = 1;
        if (_path == null)
        {
            return;
        }
        
        if(IsMovingIntoBuilding) FindObjectOfType<AudioManager>().Play("GetInside");
        else if(!IsMoving)FindObjectOfType<AudioManager>().Play("MoveUnit");
        else FindObjectOfType<AudioManager>().Play("KeepMovingUnit");
        
        
        Path = _path;
        IsMoving = true;
    }

    private Building BuildingToEnter;
    public void MoveIntoBuilding(Building _building)
    {
        BuildingToEnter = _building;
        IsMovingIntoBuilding = true;
    }

    public void OnMouseLeftClickAction()
    {
        
        OnSelection?.Invoke(this);
    }

    public void SetSelectedGround(bool _value)
    {
        SelectedGround.SetActive(_value);
    }

    public void UpdatePos()
    {
        IsSpawned?.Invoke(this);
    }

    public void SetPos(int _x,int _z)
    {
        XPos = _x;
        ZPos = _z;
    }
    
}
