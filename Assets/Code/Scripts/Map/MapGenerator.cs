// File     : myBuildMap.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using System.Collections.Generic;
using Buildings;
using Code.Scripts.Grid.DanielB;
using Code.Scripts.Wave_Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Map
{
    public class MapGenerator : MonoBehaviour
    {

        public static event Action MapIsBuild; 
        private MyGrid<GameObject> GroundObjectsMap;
        private MyGrid<Ground> GroundsMap;

        private MyGrid<GameObject> Waypoints;
        
        private int MapMultiplicator = 2;
        public int GetGroundWidth => GroundObjectsMap.GetWidth;
        public int GetGroundHeight => GroundObjectsMap.GetHeight;
        
        [SerializeField] private MapManager MapManager;
        [SerializeField] private StreetManager StreetManager;
        [SerializeField] private GameObject GroundObj;
        [SerializeField] private GameObject PrefStreetStraight;
        [SerializeField] private GameObject PrefMapThings;
        [SerializeField] private GameObject GrasGround;
        
        [SerializeField] private BuildManager BuildManager;
        [SerializeField] private GameObject PrefWaypoint;
        
        
        

        private GameObject MapThings;

        public bool MapIsReady = false;
        public MyGrid<GameObject> GetWaypoints => Waypoints;

        public void Awake()
        {
            Building.OnBaseIsUnderConstruction += SetFinalWaypoint;
            UnitManager.OnNodeReady += GenerateBuildings;
            Scrap.OnScrapDestroyed += SetGroundsFree;
        }

        private void SetGroundsFree(int _width, int _height, int _posX, int _posY)
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    GetGroundFromPosition(_posX + j, _posY + i).IsBlocked = false;
                }
            }
        }

        private Node[,] Nodes;
        private GameObject FinalWayPoint;
        private List<GameObject> EntrancePoints = new List<GameObject>();
        private Pathfinding Pathfinding;
        private List<List<GameObject>> Pathes = new List<List<GameObject>>();
        
        public List<List<GameObject>> GetPathes => Pathes;
        public List<GameObject> GetEntrancePoints => EntrancePoints;

        private void SetFinalWaypoint(Base obj)
        {
            Waypoints.Grid[(int) (obj.GetEntranceGround.GetWidth/2), (int) (obj.GetEntranceGround.GetHeight/2) ].GetComponent<Waypoint>().Init(EWaypointSignature.Final);
            FinalWayPoint = Waypoints.Grid[(int) (obj.GetEntranceGround.GetWidth / 2),
                (int) (obj.GetEntranceGround.GetHeight / 2)];
            
            Nodes = new Node[Waypoints.GetWidth,Waypoints.GetHeight];

            for (int i = 0; i < Waypoints.GetHeight; i++)
            {
                for (int j = 0; j < Waypoints.GetWidth; j++)
                {
                    Nodes[j,i] = new Node(j,i,Waypoints.Grid[j,i].transform.position);
                    if (Waypoints.Grid[j, i].GetComponent<Waypoint>().GetWaypointSignature == EWaypointSignature.None)
                    {
                        Nodes[j, i].IsWalkable = false;
                    }
                    else
                    {
                        Nodes[j, i].IsWalkable = true;
                    }
                }
            }
            
            Pathfinding = new Pathfinding(Nodes,true);

            int endX = FinalWayPoint.GetComponent<Waypoint>().GetXPos;
            int endY = FinalWayPoint.GetComponent<Waypoint>().GetZPos;
            
            foreach (GameObject entrancePoint in EntrancePoints)
            {
                foreach (var node in Nodes)
                {
                    node.Init();
                }
                
                int startX = entrancePoint.GetComponent<Waypoint>().GetXPos;
                int startY = entrancePoint.GetComponent<Waypoint>().GetZPos;
                
                List<Node> nodes = Pathfinding.FindPath(startX, startY, endX, endY);
                List<GameObject> pathObjects = new List<GameObject>(); 
                
                foreach (Node node in nodes)
                {
                    pathObjects.Add(Waypoints.Grid[node.GridX,node.GridZ]);
                } 
                Pathes.Add(pathObjects);
            }
            
        }

        
        private void Start()
        {
            Init();
        }

        public void Init()
        {
            
            MapThings = Instantiate(PrefMapThings, Vector3.zero, Quaternion.identity, transform);
            
            GenerateMap();
            
            GenerateWaveEntrances();
            GenerateRings();
            
            
            float width = MapManager.GetWidth;
            float height = MapManager.GetHeight;
            
            GrasGround.transform.localScale = new Vector3(width/10,1, height/10);
            GrasGround.transform.position = new Vector3(0,0,0);
            MapThings.transform.position = new Vector3(-width/2,0.001f,-height/2);

            GenerateWayPoints();
            
            MapIsReady = true;
            MapIsBuild?.Invoke();
            
            //GenerateBuildings();
        }

        private void GenerateWayPoints()
        {
            Waypoints = new MyGrid<GameObject>(MapManager.GetWidth,MapManager.GetHeight);
            for (int i = 0; i < Waypoints.GetHeight; i++)
            {
                for (int j = 0; j < Waypoints.GetWidth; j++)
                {
                    GameObject waypoint = Instantiate(PrefWaypoint);
                    waypoint.transform.position = new Vector3(j - MapManager.GetWidth / 2 + 0.5f,0, i - MapManager.GetHeight/ 2 + 0.5f);
                    
                    if (GroundsMap.Grid[j * 2, i * 2].GetGroundSignature == EGround.Street)
                    {
                        if (i == 0 || j == 0 || j == Waypoints.GetWidth - 1 || i == Waypoints.GetHeight - 1)
                        {
                            waypoint.GetComponent<Waypoint>().Init(EWaypointSignature.Entrance);
                            EntrancePoints.Add(waypoint);
                        }
                        else
                        {
                            waypoint.GetComponent<Waypoint>().Init(EWaypointSignature.Waypoint);
                        }
                    }
                    else
                    {
                        waypoint.GetComponent<Waypoint>().Init(EWaypointSignature.None);
                    }
                    
                    waypoint.GetComponent<Waypoint>().SetPos(j,i);
                    Waypoints.Grid[j, i] = waypoint;
                    
                }
            }
            
            
        }

        public void DeleteMap()
        {
            Destroy(MapThings);
            MapThings = null;
        }

        private void GenerateMap()
        {
            int gridWidth = MapManager.GetWidth * MapMultiplicator;
            int gridHeight = MapManager.GetHeight * MapMultiplicator;
            
            GroundObjectsMap = new MyGrid<GameObject>(gridWidth,gridHeight);
            GroundsMap = new MyGrid<Ground>(gridWidth,gridHeight);
            Ground.SetGroundSize(gridWidth,gridHeight);
            
            for (int h = 0; h < gridHeight; h++)
            {
                for (int w = 0; w < gridWidth; w++)
                {
                    GroundObjectsMap.Grid[w, h] = Instantiate(GroundObj);
                    GroundObjectsMap.Grid[w, h].transform.position = new Vector3((float)w/MapMultiplicator + GroundObj.transform.position.x,0,(float)h/MapMultiplicator + GroundObj.transform.position.z);
                    GroundObjectsMap.Grid[w, h].GetComponent<Ground>().Init(w,h);
                    GroundObjectsMap.Grid[w, h].transform.SetParent(MapThings.transform);

                    GroundsMap.Grid[w, h] = GroundObjectsMap.Grid[w, h].GetComponent<Ground>();
                    
                    SetGroundBlocked(w,h ,false);

                }
            }
        }

        private void GenerateWaveEntrances()
        {
            int entrancesPerSide = 6;

            GenerateLeftEntrances(entrancesPerSide);
            GenerateRightEntrances(entrancesPerSide);
            GenerateTopEntrances(entrancesPerSide);
            GenerateDownEntrances(entrancesPerSide);
        }
        
        private void GenerateLeftEntrances(int _entrancesAmount)
        {
            for (int i = 0; i < MapManager.GetWidth/2 -1; i+=2)
            {
                GenerateEntrances2(PrefStreetStraight, i,i,i+1,MapManager.GetHeight - i,_entrancesAmount );
            }
            
        }

        private void GenerateRightEntrances(int _entrancesAmount)
        {

            int counter = 0;
            for (int i = MapManager.GetWidth - 1; i > MapManager.GetWidth/2 +1; i-=2)
            {
                
                GenerateEntrances2(PrefStreetStraight, i,i,counter+1,MapManager.GetHeight - counter, _entrancesAmount );
                counter += 2;
            }
        }

        private void GenerateTopEntrances(int _entrancesAmount)
        {

            int counter = 0;
            for (int i = MapManager.GetHeight - 1; i > MapManager.GetHeight / 2 + 1; i -= 2)
            {
                GenerateEntrances2(PrefStreetStraight,  counter + 1,MapManager.GetWidth - counter,i,i,_entrancesAmount);
                counter += 2;
            }
        }

        private void GenerateDownEntrances(int _entrancesAmount)
        {
            for (int i = 0; i < MapManager.GetHeight / 2 - 1; i += 2)
            {
                GenerateEntrances2(PrefStreetStraight, i + 1,MapManager.GetWidth - i  ,i,i ,_entrancesAmount);
            }
        }

        private void GenerateRings()
        {
            GenerateLeftSide();
            GenerateRightSide();
            GenerateTopSide();
            GenerateDownSide();
            
            StreetManager.ChangeStreetMaterial();
        }

        private void GenerateDownSide()
        {
            int a = MapManager.GetWidth * MapMultiplicator / 2 - 2;
            int x = 1;
            int x2 = 2;
            
            for (int i = 2; i < a; i+=4)
            {
                for (int j = x2; j < MapManager.GetHeight -x - 1; j ++)
                {
                    PlaceStreetOnPos(PrefStreetStraight,j *MapMultiplicator, i );
                }
                x2 += 2;
                x += 2;
            }
        }

        private void GenerateTopSide()
        {
            int a = MapManager.GetHeight * MapMultiplicator / 2 ;
            int x = 1;
            int x2 = 2;
            
            for (int i = MapManager.GetHeight * MapMultiplicator - 4 ; i > a; i-=4)
            {
                for (int j = x2; j < MapManager.GetWidth -x - 1; j ++)
                {
                    PlaceStreetOnPos(PrefStreetStraight,j * MapMultiplicator,i  );
                }

                x2 += 2;
                x += 2;
            }
        }

        private void GenerateRightSide()
        {
            int a = MapManager.GetWidth * MapMultiplicator / 2 ;
            int x = 1;
            int x2 = 2;
            
            for (int i = MapManager.GetWidth * MapMultiplicator - 4 ; i > a; i-=4)
            {
                for (int j = x2; j < MapManager.GetHeight -x - 1; j ++)
                {
                    PlaceStreetOnPos(PrefStreetStraight,i,j *MapMultiplicator );
                }
                PlaceStreetOnPos(PrefStreetStraight,i,MapManager.GetHeight * MapMultiplicator -2 - x * 2);
                PlaceStreetOnPos(PrefStreetStraight,i,x2 * 2 -2);


                x2 += 2;
                x += 2;
            }
        }

        private void GenerateLeftSide()
        {
            int a = MapManager.GetWidth * MapMultiplicator / 2 - 2;
            int x = 1;
            int x2 = 2;
            
            for (int i = 2; i < a; i+=4)
            {
                for (int j = x2; j < MapManager.GetHeight -x - 1; j ++)
                {
                    PlaceStreetOnPos(PrefStreetStraight,i,j *MapMultiplicator );
                }
                PlaceStreetOnPos(PrefStreetStraight,i,MapManager.GetHeight * MapMultiplicator -2 - x * 2);
                PlaceStreetOnPos(PrefStreetStraight,i,x2 * 2 -2);


                x2 += 2;
                x += 2;
            }
        }
        private void GenerateEntrances2(GameObject _street, int _startPosX, int _endPosX, int _startPosY, int _endPosY, int _entrancesAmount)
        {
            int startPosX = _startPosX;
            int endPosX = _endPosX ;
            
            
            int startPosY = _startPosY;
            int endPosY = _endPosY;

            
            int entranceAmount = 1;


            if (endPosX > startPosX) entranceAmount = (endPosX - startPosX) / 4;
            
            int stagesX = (endPosX - startPosX) / entranceAmount;
            int startStageX = startPosX;
            int endStageX = startStageX + stagesX;


            if (endPosY > startPosY) entranceAmount = (endPosY - startPosY) / 4;
            
            int stagesY = (endPosY - startPosY) / entranceAmount;
            int startStageY = startPosY;
            int endStageY = startStageY + stagesY;
            
            
            for (int i = 0; i < entranceAmount; i++)
            {
                if (endStageX >= _endPosX) endStageX = endPosX - 1;
                if (endStageY >= _endPosY) endStageY = endPosY - 1;
                
                int randomValueX = _endPosX ;
                if (_startPosX != _endPosX)
                {
                    randomValueX = (int) GetRandomValue(startStageX , endStageX );
                }
                
                
                int randomValueY = _endPosY;
                if (_startPosY != _endPosY)
                {
                    randomValueY = (int) GetRandomValue(startStageY, endStageY);
                }
                
                randomValueX *= 2;
                randomValueY *= 2;
                

                
                PlaceStreetOnPos(_street,randomValueX,randomValueY);

                startStageX = endStageX  + 1;
                endStageX += stagesX ;
                
                startStageY = endStageY + 1;
                endStageY += stagesY ;
            }
        }
        
        private float GetRandomValue(int _start, int _end)
        {
            return Random.Range(_start,_end);
        }
        
        // TODO: Ins Bau Manager packen, wodrin auch die Gebäude gebaut werden
        private void PlaceStreetOnPos(GameObject _street,int _posX,int _posY)
        {
            
            GameObject street = Instantiate(_street);
            street.transform.position = GroundObjectsMap.Grid[_posX, _posY].transform.position;
            
            SetGroundBlocked(_posX,_posY,true);
            SetGroundBlocked(_posX+1,_posY+1,true);
            SetGroundBlocked(_posX+1,_posY,true);
            SetGroundBlocked(_posX,_posY+1,true);
            
            GroundObjectsMap.Grid[_posX,_posY].GetComponent<Ground>().SetGroundSignature(EGround.Street);
            GroundObjectsMap.Grid[_posX + 1,_posY + 1].GetComponent<Ground>().SetGroundSignature(EGround.Street);
            GroundObjectsMap.Grid[_posX + 1,_posY].GetComponent<Ground>().SetGroundSignature(EGround.Street);
            GroundObjectsMap.Grid[_posX,_posY + 1].GetComponent<Ground>().SetGroundSignature(EGround.Street);

            
            street.GetComponent<Street>().Init(_posX,_posY);
            street.transform.SetParent(MapThings.transform);
            
        }

        private void GenerateBuildings()
        {

            foreach (GameObject groundObj in GroundObjectsMap.Grid)
            {
                if (!groundObj.GetComponent<Ground>().IsBlockedH)
                {
                    float randomNumber = Random.Range(0, 100);
                    if (randomNumber < 5)
                    {
                        
                        BuildManager.SetScrapOnPos(groundObj.GetComponent<Ground>().GetWidth,groundObj.GetComponent<Ground>().GetHeight, Random.Range(1, 10),Random.Range(1, 10),Random.Range(1, 10));
                    }
                }
            }
            
            foreach (GameObject groundObj in GroundObjectsMap.Grid)
            {
                if (!groundObj.GetComponent<Ground>().IsBlockedH)
                {
                    float randomNumber = Random.Range(0, 100);
                    if (randomNumber < 5)
                    {
                        BuildManager.SetDestroyedHouseOnPos(groundObj.GetComponent<Ground>().GetWidth,groundObj.GetComponent<Ground>().GetHeight,Random.Range(1, 20),Random.Range(1, 20),Random.Range(1, 20) );
                    }
                }
            }
            
            foreach (GameObject groundObj in GroundObjectsMap.Grid)
            {
                if (!groundObj.GetComponent<Ground>().IsBlockedH)
                {
                    float randomNumber = Random.Range(0, 100);
                    if (randomNumber < 5)
                    {
                        BuildManager.SetHouseOnPos(groundObj.GetComponent<Ground>().GetWidth,groundObj.GetComponent<Ground>().GetHeight);
                    }
                }
            }
        }   

        public void SetGroundBlocked(int _x, int _y, bool _isActive)
        {
            GroundsMap.Grid[_x, _y].IsBlocked = _isActive;
        }

        public bool IsGroundBlocked(int _x, int _y)
        {
            if (_x < 0 || _y < 0 || _x >=  GroundObjectsMap.GetWidth || _y >= GroundObjectsMap.GetHeight) return true;
            if (GetGroundHeight < _y +1 || GetGroundWidth < _x + 1)
            {
                return true;
            }
            
            return  GroundsMap.Grid[_x, _y].IsBlocked;
        }
        
        

        public EGround GetGroundSignature(int _x, int _y)
        {
            return GroundObjectsMap.Grid[_x, _y].GetComponent<Ground>().GetGroundSignature;
        }

        public Ground GetGroundFromPosition(int _x, int _y)
        {
            if (_x < 0 || _y < 0  || _x >=  GroundObjectsMap.GetWidth || _y >= GroundObjectsMap.GetHeight) return null;
            return GroundObjectsMap.Grid[_x, _y].GetComponent<Ground>();
        }

        public Ground GetGroundFromGlobalPosition(Vector2 _pos)
        {
            foreach (Ground ground in GroundsMap.Grid)
            {
                double x = ground.transform.position.x - _pos.x;
                double y = ground.transform.position.z - _pos.y;

                x = Math.Round(x,1);
                y = Math.Round(y,1);
                
                if (x == 0 && y == 0) return ground;
            }

            return null;
        }
        
    }
}