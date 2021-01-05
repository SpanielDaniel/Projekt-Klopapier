using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : Unit
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Pathfinding path;
    private int currentPathIndex;
    private Vector3 MousePos;
    [SerializeField]
    private LayerMask layerMask;

    private void Awake()
    {
        moveSpeed = 10f;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))//If the player has left clicked
        {
            Vector3 mouse = Input.mousePosition;//Get the mouse Position
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);//Cast a ray to get where the mouse is pointing at
            RaycastHit hit;//Stores the position where the ray hit.
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, layerMask))//If the raycast doesnt hit a wall
            {
                path.FindPath(this.transform.position, hit.point);//Move the target to the mouse position
            }
        }
        if (grid.FinalPath != null)
        {
            HandleMovement();
        }
        else if(grid.FinalPath == null)
        {
            return;
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
    }

}
