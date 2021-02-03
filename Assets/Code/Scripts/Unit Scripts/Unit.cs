// File     : Unit.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.Unit_Scripts;
using Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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

    public bool IsSelected;

    [SerializeField] private int XPos;
    [SerializeField] private int ZPos;
    public int GetXPosition => XPos;
    public int GetZPosition => ZPos;
    
    private List<Node> Path;
    private int NextNode = 1;
    private Vector3 ViewDirection;
    private bool isMoving = false;
    [SerializeField] private float MoveSpeed;
    private float distance = 0f;

    
    public static List<Unit> Units = new List<Unit>();

    /// <summary>
    /// Initialize unit
    /// </summary>
    /// <param name="_data">The data from unit</param>
    /// <param name="_pos">Position where the unit spawns</param>
    public void Initialize(UnitData _data)
    {
        MaxHealthPoints = _data.MaxHealthPoints;
        Defence = _data.Defence;
        AttackPoints = _data.AttackPoints;
        AttackSpeed = _data.AttackSpeed;
        MoveSpeed = _data.MoveSpeed;
    }

    private void Awake()
    {
        AddUnit(this);
    }

    private void Start()
    {
        IsSelected = false;
    }

    private void Update()
    {
        
        
        
        
        
        if (isMoving)
        {

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
            }

            transform.position += ViewDirection * (MoveSpeed * Time.deltaTime);
            
            //distance -= (ViewDirection * (MoveSpeed * Time.deltaTime)) * 

            if (NextNode >= Path.Count)
            {
                XPos = Path[NextNode -1].GridX;
                ZPos = Path[NextNode -1].GridZ;
                transform.position = Path[NextNode - 1].Pos;
                
                isMoving = false;
            }

            
        }
        //UpdateTarget();
    }

    /// <summary>
    /// Add unit to List.
    /// </summary>
    /// <param name="_unit">Added Unit</param>
    public static void AddUnit(Unit _unit)
    {
        Units.Add(_unit);
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
        isMoving = true;
        
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

    // public void UpdateTarget()
    // {
    //     UnitManager.GetInstance.UpdateTarget(this);
    // }
}
