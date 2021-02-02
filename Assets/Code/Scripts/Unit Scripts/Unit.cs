// File     : Unit.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.Unit_Scripts;
using Interfaces;
using System;
using System.Collections.Generic;
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
    private float MoveSpeed;
    private float Range;
    public bool IsSelected;
    private int XPosition;
    private int YPosition;
    private int ID;
    
    public static List<Unit> Units = new List<Unit>();

    /// <summary>
    /// Initialize unit
    /// </summary>
    /// <param name="_data">The data from unit</param>
    /// <param name="_pos">Position where the unit spawns</param>
    public void Initialize(UnitData _data, Vector3 _pos)
    {
        XPosition = (int)_pos.x;
        YPosition = (int)_pos.y;
        MaxHealthPoints = _data.MaxHealthPoints;
        Defence = _data.Defence;
        AttackPoints = _data.AttackPoints;
        AttackSpeed = _data.AttackSpeed;
        MoveSpeed = _data.MoveSpeed;
    }

    private void Start()
    {
        AddUnit(this);
        IsSelected = false;
    }

    private void Update()
    {
        UpdateTarget();
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
    public void MoveToPosition(Vector3 _target)
    {
        UnitManager.GetInstance.MoveUnitToPos(this,_target);
    }

    public void SetTarget(Vector3 _targetPosition)
    {
        UnitManager.GetInstance.SetTargetPosition(_targetPosition);
    }

    public void OnMouseLeftClickAction()
    {
        OnSelection?.Invoke(this);
    }

    public void UpdateTarget()
    {
        UnitManager.GetInstance.UpdateTarget(this);
    }
}
