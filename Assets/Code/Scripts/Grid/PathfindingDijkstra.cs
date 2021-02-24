using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Grid
{
    class PathfindingDijkstra
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private Node[,] Nodes;
        private List<Node> Path;

        private List<Node> OpenList;
        private List<Node> ClosedList;

        public PathfindingDijkstra(Node[,] _nodes)
        {
            Nodes = _nodes;
        }



        public List<Node> FindPath(int _startX, int _startY)
        {
            Node startNode = Nodes[_startX, _startY];

            Path = null;
            OpenList = new List<Node> { startNode };
            ClosedList = new List<Node>();

            while (OpenList.Count > 0)
            {
                Node currentNode = GetLowestFCostNode(OpenList);

                if (currentNode.IsUnit == false)
                {
                    //reached Final
                    return CalculatePath(currentNode);
                }

                OpenList.Remove(currentNode);
                ClosedList.Add(currentNode);

                foreach (Node neighbourNodes in GetNeighbourList(currentNode))
                {
                    if (ClosedList.Contains(neighbourNodes)) continue;


                    if (!neighbourNodes.IsWalkable)
                    {
                        ClosedList.Add(neighbourNodes);
                        continue;
                    }

                    int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNodes);

                    if (tentativeGCost < neighbourNodes.GCost)
                    {
                        neighbourNodes.ParentNode = currentNode;
                        neighbourNodes.GCost = tentativeGCost;
                        neighbourNodes.CalculateFCost();

                        if (!OpenList.Contains(neighbourNodes))
                        {
                            OpenList.Add(neighbourNodes);
                        }
                    }
                }
            }
            return null;
        }

        private int CalculateDistanceCost(Node _firstNode, Node _secondNode)
        {
            int xDistance = Mathf.Abs(_firstNode.GridX - _secondNode.GridX);
            int zDistance = Mathf.Abs(_firstNode.GridZ - _secondNode.GridZ);
            int remaining = Mathf.Abs(xDistance - zDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        }
        private List<Node> CalculatePath(Node _endNode)
        {
            List<Node> path = new List<Node>();
            path.Add(_endNode);
            Node currentNode = _endNode;

            while (currentNode.ParentNode != null)
            {
                path.Add(currentNode.ParentNode);
                currentNode = currentNode.ParentNode;
            }

            path.Reverse();
            return path;
        }

        private List<Node> GetNeighbourList(Node _currentNode)
        {
            List<Node> neighbourList = new List<Node>();

            if (_currentNode.GridX - 1 >= 0)
            {
                //left
                neighbourList.Add(Nodes[_currentNode.GridX - 1, _currentNode.GridZ]);

                //left down
                if (_currentNode.GridZ - 1 >= 0)
                {
                    neighbourList.Add(Nodes[_currentNode.GridX - 1, _currentNode.GridZ - 1]);
                }

                //left Up
                if (_currentNode.GridZ + 1 < Nodes.GetLength(1))
                {
                    neighbourList.Add(Nodes[_currentNode.GridX - 1, _currentNode.GridZ + 1]);
                }
            }

            if (_currentNode.GridX + 1 < Nodes.GetLength(0))
            {
                //right
                neighbourList.Add(Nodes[_currentNode.GridX + 1, _currentNode.GridZ]);

                //right down
                if (_currentNode.GridZ - 1 >= 0)
                {
                    neighbourList.Add(Nodes[_currentNode.GridX + 1, _currentNode.GridZ - 1]);
                }

                //right down
                if (_currentNode.GridZ + 1 < Nodes.GetLength(1))
                {
                    neighbourList.Add(Nodes[_currentNode.GridX + 1, _currentNode.GridZ + 1]);
                }
            }

            //down
            if (_currentNode.GridZ - 1 >= 0)
            {
                neighbourList.Add(Nodes[_currentNode.GridX, _currentNode.GridZ - 1]);
            }

            //up
            if (_currentNode.GridZ + 1 < Nodes.GetLength(1))
            {
                neighbourList.Add(Nodes[_currentNode.GridX, _currentNode.GridZ + 1]);
            }
            return neighbourList;
        }

        private Node GetLowestFCostNode(List<Node> _pathNodeList)
        {
            Node lowestFCostNode = _pathNodeList[0];
            for (int i = 1; i < _pathNodeList.Count; i++)
            {
                if (_pathNodeList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = _pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }


    }
}
