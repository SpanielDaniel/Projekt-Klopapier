// File     : Pathfinding.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System.Collections.Generic;
using Code.Scripts.Grid.DanielB;
using UnityEngine;

public class Pathfinding
{
    private const int MoveStraightCost = 10;
    private const int MoveDiagonalCost = 14;

    public static Pathfinding Instance { get; private set; }

    private Grid<Node> Grid;
    private List<Node> OpenList;
    private List<Node> ClosedList;
    private bool IsBlocked;
    
    public Pathfinding(int _widht, int _height)    {        Instance = this;        Grid = new Grid<Node>(_widht, _height,(Grid<Node> g, int x, int z)=> new Node(g,x,z));    }

    public List<Vector3> FindPath(Vector3 _startWorldPosition, Vector3 _endWorldPosition)    {        Grid.GetXZ(_startWorldPosition, out int _startX, out int _startZ);        Grid.GetXZ(_endWorldPosition, out int _endX, out int _endZ);
        List<Node> path = FindPath(_startX, _startZ, _endX, _endZ);
        if (path == null)        {            return null;        }        else        {            List<Vector3> vectorList = new List<Vector3>();
            foreach (Node pathNode in path)            {                vectorList.Add(new Vector3(pathNode.GridX, 0, pathNode.GridZ) + Vector3.one * 0.5f);            }            return vectorList;        }    }

    public List<Node> FindPath(int _startX, int _startY, int _endX, int _endY)    {        Node startNode = Grid.GetGridObject(_startX, _startY);        Node endNode = Grid.GetGridObject(_endX, _endY);
        OpenList = new List<Node> { startNode };        ClosedList = new List<Node>();
        for (int x = 0; x < Grid.GetWidth(); x++)        {            for (int z = 0; z < Grid.GetHeight(); z++)            {                Node pathNode = Grid.GetGridObject(x, z);                pathNode.GCost = 99999999;                pathNode.CalculateFCost();                pathNode.ParentNode = null;            }        }
        startNode.GCost = 0;        startNode.HCost = CalculateDistanceCost(startNode, endNode);        startNode.CalculateFCost();
        while (OpenList.Count > 0)        {            Node currentNode = GetLowestFCostNode(OpenList);            if (currentNode == endNode)            {                //reached Final                return CalculatePath(endNode);            }
            OpenList.Remove(currentNode);            ClosedList.Add(currentNode);
            foreach (Node neighbourNodes in GetNeighbourList(currentNode))            {                if (ClosedList.Contains(neighbourNodes))                {                    continue;                }
                if (!neighbourNodes.IsWalkable)                {                    ClosedList.Add(neighbourNodes);                    continue;                }
                int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNodes);
                if (tentativeGCost < neighbourNodes.GCost)                {                    neighbourNodes.ParentNode = currentNode;                    neighbourNodes.GCost = tentativeGCost;                    neighbourNodes.HCost = CalculateDistanceCost(neighbourNodes, endNode);                    neighbourNodes.CalculateFCost();
                    if (!OpenList.Contains(neighbourNodes))                    {                        OpenList.Add(neighbourNodes);                    }                }            }        }        return null;    }

    private List<Node> GetNeighbourList(Node _currentNode)    {        List<Node> neighbourList = new List<Node>();
        if (_currentNode.GridX - 1 >= 0)        {            //left            neighbourList.Add(GetNode(_currentNode.GridX - 1, _currentNode.GridZ));
            //left down            if (_currentNode.GridZ - 1 >= 0)            {                neighbourList.Add(GetNode(_currentNode.GridX - 1, _currentNode.GridZ - 1));            }
            //left Up            if (_currentNode.GridZ + 1< Grid.GetHeight())            {                neighbourList.Add(GetNode(_currentNode.GridX - 1, _currentNode.GridZ + 1));            }        }
        if (_currentNode.GridX + 1 < Grid.GetWidth())        {            //right            neighbourList.Add(GetNode(_currentNode.GridX + 1, _currentNode.GridZ));
            //right down            if (_currentNode.GridZ - 1 >= 0)            {                neighbourList.Add(GetNode(_currentNode.GridX + 1, _currentNode.GridZ - 1));            }
            //right down            if (_currentNode.GridZ +1 < Grid.GetHeight())            {                neighbourList.Add(GetNode(_currentNode.GridX + 1, _currentNode.GridZ + 1));            }        }
        //down        if (_currentNode.GridZ - 1 >= 0)        {            neighbourList.Add(GetNode(_currentNode.GridX, _currentNode.GridX - 1));        }
        //up        if (_currentNode.GridZ + 1 < Grid.GetHeight())        {            neighbourList.Add(GetNode(_currentNode.GridX, _currentNode.GridZ + 1));        }        return neighbourList;    }

    private Node GetNode(int _x, int _z)    {        return Grid.GetGridObject(_x, _z);    }

    private List<Node> CalculatePath(Node _endNode)    {        List<Node> path = new List<Node>();        path.Add(_endNode);        Node currentNode = _endNode;
        while (currentNode.ParentNode != null)        {            path.Add(currentNode.ParentNode);            currentNode = currentNode.ParentNode;        }
        path.Reverse();        return path;    }

    private int CalculateDistanceCost(Node _a, Node _b)    {        int xDistance = Mathf.Abs(_a.GridX - _b.GridX);        int zDistance = Mathf.Abs(_a.GridZ - _b.GridZ);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MoveDiagonalCost * Mathf.Min(xDistance, zDistance) + MoveStraightCost * remaining;    }

    private Node GetLowestFCostNode(List<Node> _pathNodeList)    {        Node lowestFCostNode = _pathNodeList[0];        for (int i = 1; i < _pathNodeList.Count; i++)        {            if (_pathNodeList[i].FCost < lowestFCostNode.FCost)            {                lowestFCostNode = _pathNodeList[i];            }        }        return lowestFCostNode;    }

    public Grid<Node> GetGrid()    {        return Grid;    }
}