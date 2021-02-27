// File     : Node.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;

public class Node
{
    public Vector3 Pos;
    
    public int GridX;
    public int GridZ;

    public bool IsWalkable = true;
    public bool IsUnit = false;
    
    public Node ParentNode;

    public int GCost;
    public int HCost;
    public int FCost;

    public Node(int _gridX, int _gridY, Vector3 _pos)
    {
        Pos = _pos;
        GridX = _gridX;
        GridZ = _gridY;
        IsWalkable = true;
        IsUnit = false;
        Init();

    }

    public void Init()
    {
        GCost = int.MaxValue;
        ParentNode = null;
        HCost = 0;
        FCost = 0;
        CalculateFCost();
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

}