﻿// File     : myBuildMap.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Build;
using Code.Scripts.Grid.DanielB;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Map
{
    public class MapGenerator : MonoBehaviour
    {

        public static event Action MapIsBuild; 
        private MyGrid<GameObject> GroundMap;
        private int MapMultiplicator = 2;

        
        public int GetGroundWidth => GroundMap.GetWidth;
        public int GetGroundHeight => GroundMap.GetHeight;
        
        [SerializeField] private MapManager MapManager;
        [SerializeField] private StreetManager StreetManager;
        [SerializeField] private GameObject GroundObj;
        [SerializeField] private GameObject PrefStreetStraight;
        [SerializeField] private GameObject PrefMapThings;
        [SerializeField] private GameObject GrasGround;

        private GameObject MapThings;

        public bool MapIsReady = false; 
        
        
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

            MapIsReady = true;
            MapIsBuild?.Invoke();

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
            
            GroundMap = new MyGrid<GameObject>(gridWidth,gridHeight);
            Ground.SetGroundSize(gridWidth,gridHeight);
            
            for (int h = 0; h < gridHeight; h++)
            {
                for (int w = 0; w < gridWidth; w++)
                {
                    GroundMap.Grid[w, h] = Instantiate(GroundObj);
                    GroundMap.Grid[w, h].transform.position = new Vector3((float)w/MapMultiplicator + GroundObj.transform.position.x,0,(float)h/MapMultiplicator + GroundObj.transform.position.z);
                    SetGroundBlocked(w,h ,false);
                    GroundMap.Grid[w, h].GetComponent<Ground>().Init(w,h);
                    GroundMap.Grid[w, h].transform.SetParent(MapThings.transform);
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
            //Debug.Log(_posX+ " " + _posY);
            GameObject street = Instantiate(_street);
            street.transform.position = GroundMap.Grid[_posX, _posY].transform.position;

            SetGroundBlocked(_posX,_posY,true);
            SetGroundBlocked(_posX+1,_posY+1,true);
            SetGroundBlocked(_posX+1,_posY,true);
            SetGroundBlocked(_posX,_posY+1,true);
            
            street.GetComponent<Street>().Init(_posX,_posY);
            street.transform.SetParent(MapThings.transform);
        }
                

        public void SetGroundBlocked(int _x, int _y, bool _isActive)
        {
            GroundMap.Grid[_x, _y].GetComponent<Ground>().IsBlocked = _isActive;
        }

        public bool IsGroundBlocked(int _x, int _y)
        {
            if (GetGroundHeight < _y +1 || GetGroundWidth < _x + 1)
            {
                return true;
            }
            return GroundMap.Grid[_x, _y].GetComponent<Ground>().IsBlocked;
        }

        public EGround GetGroundSignature(int _x, int _y)
        {
            return GroundMap.Grid[_x, _y].GetComponent<Ground>().GetGroundSignature;
        }

        public Ground GetGroundFromPosition(int _x, int _y)
        {
            return GroundMap.Grid[_x, _y].GetComponent<Ground>();
        }
    }
}