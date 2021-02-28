// File     : Unit.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.Unit_Scripts;
using Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using Buildings;
using Code.Scripts;
using Code.Scripts.Grid.DanielB;
using Code.Scripts.Map;
using Player;
using UnityEngine;

public class Unit : MonoBehaviour
    ,IMouseLeftDown
{
    public static event Action<int, float> OnHeal; 
    public static event Action<Unit> OnSelection;
    public static event Action<Unit> IsSpawned;
    public static event Action<Unit,Ground> OnMapEntrance; 
    public static event Action<Unit> CancledGather; 
    
    private float MaxHealthPoints;
    [SerializeField] private float CurrentHealthPoints;
    public float GetCurrentHealth => CurrentHealthPoints;
    private float Defence;
    private float AttackPoints;
    private float AttackSpeed;
    private float Range;
    private int ID;
    private string Name;
    private Sprite Icon;
    private float CountDownShoot;

    public bool IsSelected;

    private bool IsDead = false;

    [SerializeField] private UnitData UnitData;
    [SerializeField] private int XPos;
    [SerializeField] private int ZPos;
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject UnitObj;
    [SerializeField] private GameObject RotateObject;
    [SerializeField] private GameObject SelectedGround;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform FirePoint;

    public UnitData GetUnitData => UnitData;

    private float MoveSpeed;
    private float NormalMoveSpeed;

    public int GetXPosition => XPos;
    public int GetZPosition => ZPos;

    private List<Node> Path;
    private int NextNode = 1;
    private Vector3 ViewDirection;
    private bool IsMoving = false;
    private bool IsMovingIntoBuilding;
    private bool IsGoingGather;

    private GameObject Target;

    private bool isMoving = false;
    private bool isIlde = true;
    private float distance = 0f;

    
    public static List<Unit> Units = new List<Unit>();
    public int GetID => ID;

    public bool GetIsDead => IsDead;

    public float GetMaxHealth => MaxHealthPoints;
    

    private void Awake()
    {
        Initialize(UnitData);
        AddUnit(this);
        MapGenerator.MapIsBuild += AddPopulation;
        UnitManager.OnNodeReady += UpdateNodes;

    }

    private void UpdateNodes()
    {
        UnitManager.GetInstance.GetNodes[GetXPosition, GetZPosition].IsUnit = true;
    }

    private void AddPopulation()
    {
        //PlayerData.GetInstance.PopulationH += 1;
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
        NormalMoveSpeed = MoveSpeed;
        CurrentHealthPoints = MaxHealthPoints;
    }

    private void Start()
    {
        IsSelected = false;
        SetSelectedGround(false);
        UpdatePos();
        PlayerData.GetInstance.PopulationH += 1;
    }

    private void Update()
    {
        if (isIlde)
        {
            Animator.SetBool("IsMoving", false);
            Animator.SetBool("IsIdle", true);
            Animator.SetBool("IsCombat", false);
        }

        if (IsMoving)
        {
            Animator.SetBool("IsMoving", true);
            Animator.SetBool("IsIdle", false);
            Animator.SetBool("IsCombat", false);

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
                RotateObject.transform.eulerAngles = new Vector3(0,-angle, 0);

            }

            transform.position += ViewDirection * (MoveSpeed * Time.deltaTime);

            if (NextNode >= Path.Count)
            {
                
                bool isUnitOnGround = UnitManager.GetInstance.GetNodes[   Path[NextNode - 1].GridX   ,    Path[NextNode - 1].GridZ   ].IsUnit   ;

                
                if (isUnitOnGround)
                {
                    if (!IsMovingIntoBuilding)
                    {
                        Path = null;

                        UnitManager.GetInstance.FindPathToFreePosition(this);
                    }
                }
                else
                {
                    transform.position = Path[NextNode - 1].Pos;
                    
                    IsMoving = false;
                    isIlde = true;
                    if (!IsMovingIntoBuilding)
                    {
                        UnitManager.GetInstance.GetNodes[GetXPosition, GetZPosition].IsUnit = true;
                        Path = null;
                    }
                }
                
                
                if (IsMovingIntoBuilding)
                {
                    if (Path[0].GridX == GetXPosition && Path[0].GridZ == GetZPosition)
                    {
                        
                        UnitManager.GetInstance.GetNodes[GetXPosition, GetZPosition].IsUnit = false;
                    }

                    if (BuildingToEnter.AddUnit(ID))
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        SetPos(GetXPosition,GetZPosition);
                    }
                    IsMovingIntoBuilding = false;
                    
                    
                }

                if (IsGoingGather)
                {
                    UnitManager.GetInstance.GetNodes[GetXPosition, GetZPosition].IsUnit = false;
                    
                    gameObject.SetActive(false);
                    OnMapEntrance?.Invoke(this,UnitManager.GetInstance.GetUnitGround(this));
                    Path = null;

                }
                
                
                
            }
        }
        UpdateTarget();


        if (Target != null)
        {
            LockOnTarget();
            IsMoving = false;

            if (CountDownShoot <= 0f)
            {
                isIlde = false;
                Animator.SetBool("IsMoving", false);
                Animator.SetBool("IsCombat", true);
                Animator.SetBool("IsIdle", false);
                Attack(Target);
                CountDownShoot = AttackSpeed;

            }

            CountDownShoot -= Time.deltaTime;
        }
        else
        {
            isIlde = true;
        }
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
            float dis = Vector3.Distance(this.transform.position, enemy.transform.position);
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
        Vector3 direction = (Target.transform.position - RotateObject.transform.position);

        distance = direction.magnitude;
        ViewDirection = direction.normalized;


        float angle = Vector2.SignedAngle(Vector2.up, new Vector2(ViewDirection.x, ViewDirection.z));
        RotateObject.transform.eulerAngles = new Vector3(0, -angle, 0);
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

    public float GetAttack()
    {
        return AttackPoints;
    }

    public float GetRange()
    {
        return Range;
    }

    public float GetAttackSpeed()
    {
        return AttackSpeed;
    }

    public void TakeDamage(int _dmg)
    {
        CurrentHealthPoints -= (_dmg - Defence);
        if(CurrentHealthPoints <= 0) PlayerData.GetInstance.PopulationH -= 1;
        
        if (CurrentHealthPoints <= 0)
        {
            gameObject.SetActive(false);
            IsDead = true;
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
        
        Path = _path;
        IsMoving = true;
    }

    private Building BuildingToEnter;
    public void MoveIntoBuilding(Building _building)
    {
        BuildingToEnter = _building;
        IsMovingIntoBuilding = true;
    }

    public void CancelMovingIntoBuilding()
    {
        IsMovingIntoBuilding = false;
        BuildingToEnter = null;
        if(IsGoingGather) CancledGather?.Invoke(this);
        IsGoingGather = false;
    }

    public void OnMouseLeftDownAction()
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
        
        bool isUnitOnGround = UnitManager.GetInstance.GetNodes[ _x,_z].IsUnit;
        if (isUnitOnGround)
        {
            Path = null;
            UnitManager.GetInstance.FindPathToFreePosition(this);
        }
        else
        {
            UnitManager.GetInstance.GetNodes[GetXPosition, GetZPosition].IsUnit = true;
            IsMoving = false;
            UpdatePos();

        }
        
        
        
    }
    public void GoToMapEnd()
    {
        IsGoingGather = true;
        UnitManager.GetInstance.FindPathToWaveEntrance(this);
    }

    public void Heal(float _amount)
    {
        CurrentHealthPoints += _amount;
        if (CurrentHealthPoints > MaxHealthPoints) CurrentHealthPoints = MaxHealthPoints;
        OnHeal?.Invoke(ID,CurrentHealthPoints);
    }
    
}
