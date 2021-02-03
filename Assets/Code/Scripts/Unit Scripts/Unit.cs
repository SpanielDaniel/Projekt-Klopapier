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
    private float MaxHealthPoints;
    private float CurrentHealthPoints;
    private float Defence;
    private float AttackPoints;
    private float AttackSpeed;
    private float Range;
    private int ID;
    private string Name;
    private Sprite Icon;

    public bool IsSelected;

    [SerializeField] private UnitData UnitData;
    [SerializeField] private int XPos;
    [SerializeField] private int ZPos;
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject UnitObj;


    public UnitData GetUnitData => UnitData;

    private float MoveSpeed;

    public int GetXPosition => XPos;
    public int GetZPosition => ZPos;

    private List<Node> Path;
    private int NextNode = 1;
    private Vector3 ViewDirection;
    private bool IsMoving = false;
    private bool IsMovingIntoBuilding;

    private bool isMoving = false;
    private float distance = 0f;


    public static List<Unit> Units = new List<Unit>();

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

    private void Awake()
    {
        Initialize(UnitData);
        AddUnit(this);
    }

    private void Start()
    {
        IsSelected = false;
    }

    private void Update()
    {
        Animator.SetBool("IsMoving", true);

        if (IsMoving)
        {
            if (distance < 0.01f)
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
                Debug.Log(angle);
                UnitObj.transform.eulerAngles = new Vector3(0,-angle,0); //new Quaternion(0,angle,0,0);   ;
            }

            transform.position += ViewDirection * (MoveSpeed * Time.deltaTime);

            if (NextNode >= Path.Count)
            {
                XPos = Path[NextNode -1].GridX;
                ZPos = Path[NextNode -1].GridZ;
                transform.position = Path[NextNode - 1].Pos;
                Path = null;
                IsMoving = false;
                if (IsMovingIntoBuilding)
                {
                    IsMovingIntoBuilding = false;
                    BuildingToEnter.AddUnit(ID);
                    gameObject.SetActive(false);
                }
            }

            
        }
        //UpdateTarget();
    }

    /// <summary>
    /// Add unit to List.
    /// </summary>
    /// <param name="_unit">Added Unit</param>
    public void AddUnit(Unit _unit)
    {
        Units.Add(_unit);
        ID = Units.Count;
    }

    
    /// <summary>
    /// Move unit to a position.
    /// </summary>
    /// <param name="_x">X coordinate from world</param>
    /// <param name="_y">Y coordinate from world</param>
    public void MoveToPosition(List<Node> _path)
    {
        NextNode = 1;
        distance = 0;
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

    public void SetTarget(Vector3 _targetPosition)
    {
       // UnitManager.GetInstance.SetTargetPosition(_targetPosition);
    }

    public void OnMouseLeftClickAction()
    {
        OnSelection?.Invoke(this);
    }
    
    private void SetPos()
    {
        
    }

    
}
