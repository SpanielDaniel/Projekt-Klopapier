﻿// File     : UnitManager.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.Unit_Scripts;
using Code.Scripts.Grid.DanielB;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private GameObject Unit;
    //[SerializeField] private MapManager MapManager;
    [SerializeField] private Transform Target;
    private Transform AttackTarget;
    private UnitData Data;
    private Pathfinding Pathfinding;
    private List<Vector3> VectorPathList;
    private int CurrentPathIndex;
    private float CountDown;
    private float StartCountDown = 10f;
    private Node[,] Nodes;
    public Node[,] GetNodes => Nodes; 

    private void Start()
    {
        //Nodes = new Node[MapManager.GetWidth * 2, MapManager.GetHeight * 2];
        Pathfinding = new Pathfinding(50, 50);
        CountDown = StartCountDown;
    }

    private void Update()
    {
        if (AttackTarget == null)
            return;

        if (CountDown <= 0f)
        {
            Shoot();
            CountDown = StartCountDown;
        }

        CountDown = (CountDown / 2) - Time.deltaTime;
    }

    /// <summary>
    /// Spawn unit on position
    /// </summary>
    /// <param name="_pos">The position where a new unit spawnes</param>
    public void SpawnUnitOnPos(UnitData _data, Vector3 _pos)
    {
        GameObject unit = Instantiate(Unit, _pos, Quaternion.identity);
        unit.GetComponent<Unit>().Initialize(_data ,_pos);
    }

    public void MoveUnitToPos(Unit _unit, int _x, int _y)
    {
        Pathfinding.GetGrid().GetXY(Target.position, out int x, out int y);
        List<Node> NodePath = Pathfinding.FindPath(0, 0, x, y);
        SetTargetPosition(Target.position);
    }

    public void SetTargetPosition(Vector3 _targetPosition)
    {
        CurrentPathIndex = 0;
        VectorPathList = Pathfinding.FindPath(transform.position, _targetPosition);

        if (VectorPathList != null && VectorPathList.Count > 1)
        {
            VectorPathList.RemoveAt(0);
        }
    }

    public void MoveHandler()
    {
        if (VectorPathList != null)
        {
            Vector3 targetPosition = VectorPathList[CurrentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDir * 10 * Time.deltaTime;
            }
            else
            {
                CurrentPathIndex++;
                if (CurrentPathIndex >= VectorPathList.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving()
    {
        VectorPathList = null;
    }

    public void UpdateTarget(Unit _unit)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(_unit.transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= 5)
            AttackTarget = nearestEnemy.transform;
        else
            AttackTarget = null;
    }

    void Shoot()
    {
        Debug.Log("Unit Dmg");
    }

    //private void Awake()
    //{
    //    units.Add(this);
    //    moveSpeed = 10f;
    //}

    //public void Update()
    //{
    //    if (fPath)
    //    {
    //        selectedSphere.SetActive(true);

    //        if (Input.GetMouseButtonDown(1))//If the player has left clicked
    //        {
    //            Vector3 mouse = Input.mousePosition;
    //            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
    //            RaycastHit hit;
    //            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, layerMask))//If the raycast doesnt hit a wall
    //            {
    //                path.FindPath(this.transform.position, hit.point);//Find path to Target.
    //            }
    //        }
    //        if (grid.FinalPath != null)
    //        {
    //            HandleMovement();
    //        }
    //    }
    //    else
    //    {
    //        selectedSphere.SetActive(false);
    //    }


    //}

    //private void HandleMovement()
    //{
    //    if (grid.FinalPath != null)
    //    {
    //        Vector3 targetPosition = grid.FinalPath[currentPathIndex].vPosition;
    //        if (Vector3.Distance(transform.position, targetPosition) > 1f)
    //        {
    //            Vector3 moveDir = (targetPosition - transform.position).normalized;

    //            float distanceBefore = Vector3.Distance(transform.position, targetPosition);
    //            transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;
    //        }
    //        else
    //        {
    //            currentPathIndex++;
    //            if (currentPathIndex >= grid.FinalPath.Count)
    //            {
    //                StopMoving();
    //            }
    //        }
    //    }

    //}

    //private void StopMoving()
    //{
    //    grid.FinalPath = null;
    //    currentPathIndex = 0;
    //    selectedSphere.SetActive(false);
    //}
}
