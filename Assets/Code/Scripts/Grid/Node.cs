// File     : Node.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;

public class Node
{
    private Grid<Node> NodeGrid;
    public int GridX;
    public int GridY;

    public bool IsWalkable;
    public Vector3 Position;

    public Node ParentNode;

    public int GCost;
    public int HCost;
    public int FCost;

    public Node(Grid<Node> grid, int _gridX, int _gridY)
    {
        this.NodeGrid = grid;
        GridX = _gridX;
        GridY = _gridY;

        IsWalkable = true;
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

}