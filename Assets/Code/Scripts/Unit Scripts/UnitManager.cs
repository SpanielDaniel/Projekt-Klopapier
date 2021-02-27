// File     : UnitManager.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System;
using Assets.Code.Scripts.Unit_Scripts;
using Code.Scripts.Grid.DanielB;
using System.Collections.Generic;
using Assets.Code.Scripts.Grid;
using Code.Scripts;
using Code.Scripts.Map;
using Code.Scripts.Wave_Scripts;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = System.Random;

public class UnitManager : Singleton<UnitManager>
{
    // -------------------------------------------------------------------------------------------------------------

    #region Init

    // Events ------------------------------------------------------------------------------------------------------

    public static event Action OnNodeReady;

    // Serialize Fields --------------------------------------------------------------------------------------------

    [SerializeField] private MapManager MapManager;
    [SerializeField] private MapGenerator MapGenerator;

    // private -----------------------------------------------------------------------------------------------------

    private Node[,] Nodes;
    private List<Node> Path;
    private List<Node> PathDijkstra;
    private Pathfinding Pathfinding;
    private PathfindingDijkstra PathfindingDijkstra;

    // Get properties ----------------------------------------------------------------------------------------------

    public Node[,] GetNodes => Nodes;

    #endregion

    // -------------------------------------------------------------------------------------------------------------


    protected override void AwakeFunction()
    {
        Unit.IsSpawned += SetUnitToPos;
        MapGenerator.MapIsBuild += SetUnitPos;
        MapGenerator.MapIsBuild += GenerateNodes;
        base.AwakeFunction();
    }

    /// <summary>
    /// Generate Nodes for Pathginding
    /// </summary>
    private void GenerateNodes()
    {
        CreateNodes();
        Pathfinding = new Pathfinding(Nodes,false);
        PathfindingDijkstra = new PathfindingDijkstra(Nodes);
        OnNodeReady?.Invoke();
    }

    /// <summary>
    /// Create Nodes over the Map
    /// </summary>
    private void CreateNodes()
    {
        int x = MapManager.GetWidth * 2;
        int z = MapManager.GetHeight * 2;
        Nodes = new Node[x, z];
        for (int currentZ = 0; currentZ < z; currentZ++)
        {
            for (int currentX = 0; currentX < x; currentX++)
            {
                Nodes[currentX,currentZ] = new Node(currentX,currentZ, MapGenerator.GetGroundFromPosition(currentX,currentZ).transform.position);
            }
        }
    }

    /// <summary>
    /// Find Path for Unit
    /// </summary>
    /// <param name="_unit">Moving Unit</param>
    /// <param name="_endPosX">End Position X</param>
    /// <param name="_endPosZ">End Position Y</param>
    public void FindPathForUnit(Unit _unit, int _endPosX, int _endPosZ)
    {
        foreach (var node in Nodes)
        {
            node.Init();
        }
        Path = Pathfinding.FindPath(_unit.GetXPosition, _unit.GetZPosition, _endPosX, _endPosZ);
        UnitManager.GetInstance.GetNodes[_unit.GetXPosition, _unit.GetZPosition].IsUnit = false;
        _unit.MoveToPosition(Path);
    }

    /// <summary>
    /// Move Unit to Free Position
    /// </summary>
    /// <param name="_unit">Moving Unit</param>
    public void FindPathToFreePosition(Unit _unit)
    {
        foreach (var node in Nodes)
        {
            node.Init();
        }
        PathDijkstra = PathfindingDijkstra.FindPath(_unit.GetXPosition, _unit.GetZPosition);
        _unit.MoveToPosition(PathDijkstra);
    }

    /// <summary>
    /// Set all Units in Units List to a Position
    /// </summary>
    private void SetUnitPos()
   {
       foreach (Unit _unit in Unit.Units)
       {
           SetUnitToPos(_unit);
       }
   }

    /// <summary>
    /// Set Unit Transform Position
    /// </summary>
    /// <param name="_unit"></param>
    private void SetUnitToPos(Unit _unit)
    {
        if(!MapGenerator.MapIsReady) return;
        _unit.transform.position = MapGenerator.GetGroundFromPosition(_unit.GetXPosition, _unit.GetZPosition).transform.position;
    }

    /// <summary>
    /// ich habe keine Ahnung
    /// </summary>
    /// <param name="_unit"></param>
    public void FindPathToWaveEntrance(Unit _unit)
    {
        Random random = new Random();
        int randomEntranceIndex = random.Next(0, MapGenerator.GetEntrancePoints.Count);
        List<GameObject> entrances =  MapGenerator.GetEntrancePoints;
        int endPosX = entrances[randomEntranceIndex].GetComponent<Waypoint>().GetXPos * 2;
        int endPosZ = entrances[randomEntranceIndex].GetComponent<Waypoint>().GetZPos * 2;
        
         FindPathForUnit(_unit,endPosX,endPosZ);
    }
    
    /// <summary>
    /// Get Ground from Unit
    /// </summary>
    /// <param name="_unit"></param>
    /// <returns></returns>
    public Ground GetUnitGround(Unit _unit)
    {
        return MapGenerator.GetGroundFromPosition(_unit.GetXPosition, _unit.GetZPosition);
    }
    
}
