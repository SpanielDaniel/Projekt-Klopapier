// File     : UnitMovement.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : Unit
{
    public static UnitMovement _instance;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Pathfinding path;
    private int currentPathIndex;
    [SerializeField]
    private LayerMask layerMask;
    public bool fPath = false;
    [SerializeField]
    private GameObject selectedSphere;


    private void Awake()
    {
        units.Add(this);
        moveSpeed = 10f;
    }

    public void Update()
    {
        if (fPath)
        {
            selectedSphere.SetActive(true);

            if (Input.GetMouseButtonDown(1))//If the player has left clicked
            {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, layerMask))//If the raycast doesnt hit a wall
                {
                    path.FindPath(this.transform.position, hit.point);//Find path to Target.
                }
            }
            if (grid.FinalPath != null)
            {
                HandleMovement();
            }
        }
            

    }

    private void HandleMovement()
    {
        if (grid.FinalPath != null)
        {
            Vector3 targetPosition = grid.FinalPath[currentPathIndex].vPosition;
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= grid.FinalPath.Count)
                {
                    StopMoving();
                }
            }
        }
        
    }

    private void StopMoving()
    {
        grid.FinalPath = null;
        currentPathIndex = 0;
        fPath = false;
        selectedSphere.SetActive(false);
    }
}
