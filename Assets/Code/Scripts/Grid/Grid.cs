using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask Wall;

    public Transform startPosition;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distanceNodes;

    Node[,] NodeArray;
    public List<Node> FinalPath;


    float nodeDiameter;
    int gridSizeX, gridSizeY;


    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        NodeArray = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool Wall = true;

                if (Physics.CheckSphere(worldPoint, nodeRadius, this.Wall))
                {
                    Wall = false;
                }

                NodeArray[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();
        int Xcheck;
        int Ycheck;

        //Check the right side of the current node.
        Xcheck = a_NeighborNode.iGridX + 1;
        Ycheck = a_NeighborNode.iGridY;
        if (Xcheck >= 0 && Xcheck < gridSizeX)
        {
            if (Ycheck >= 0 && Ycheck < gridSizeY)
            {
                NeighborList.Add(NodeArray[Xcheck, Ycheck]);
            }
        }
        //Check the Left side of the current node.
        Xcheck = a_NeighborNode.iGridX - 1;
        Ycheck = a_NeighborNode.iGridY;
        if (Xcheck >= 0 && Xcheck < gridSizeX)
        {
            if (Ycheck >= 0 && Ycheck < gridSizeY)
            {
                NeighborList.Add(NodeArray[Xcheck, Ycheck]);
            }
        }
        //Check the Top side of the current node.
        Xcheck = a_NeighborNode.iGridX;
        Ycheck = a_NeighborNode.iGridY + 1;
        if (Xcheck >= 0 && Xcheck < gridSizeX)
        {
            if (Ycheck >= 0 && Ycheck < gridSizeY)
            {
                NeighborList.Add(NodeArray[Xcheck, Ycheck]);
            }
        }
        //Check the Bottom side of the current node.
        Xcheck = a_NeighborNode.iGridX;
        Ycheck = a_NeighborNode.iGridY - 1;
        if (Xcheck >= 0 && Xcheck < gridSizeX)
        {
            if (Ycheck >= 0 && Ycheck < gridSizeY)
            {
                NeighborList.Add(NodeArray[Xcheck, Ycheck]);
            }
        }

        return NeighborList;
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((gridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((gridSizeY - 1) * iyPos);

        return NodeArray[ix, iy];
    }


    //Function that draws the wireframe
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (NodeArray != null)
        {
            foreach (Node n in NodeArray)
            {
                if (n.bIsWall) Gizmos.color = Color.white;
                else Gizmos.color = Color.yellow;

                if (FinalPath != null)
                {
                    if (FinalPath.Contains(n)) Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(n.vPosition, Vector3.one * (nodeDiameter - distanceNodes));
            }
        }
    }
}