using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : Unit
{
    private Pathfinding path;
    private Grid grid;
    private Transform unitPosition;

    private void Awake()
    {
        unitPosition = GetComponent<Transform>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                transform.position = Vector3.MoveTowards(unitPosition.position, path.TargetPosition.position, moveSpeed);
            }
        }
    }
}
