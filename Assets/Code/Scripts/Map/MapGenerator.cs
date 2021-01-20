// File     : myBuildMap.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using Build;
using Code.Scripts.Grid.DanielB;
using UnityEngine;

namespace Code.Scripts.Map
{
    public class MapGenerator : MonoBehaviour
    {
        private MyGrid<GameObject> GroundMap;
        private int MapMultiplicator = 2;

        
        public int GetGroundWidth => GroundMap.GetWidth;
        public int GetGroundHeight => GroundMap.GetHeight;
        
        [SerializeField] private MapManager MapManager;
        [SerializeField] private StreetManager StreetManager;
        [SerializeField] private GameObject GroundObj;
        [SerializeField] private GameObject PrefStreetStraight;
        [SerializeField] private GameObject PrefMapThings;

        private GameObject MapThings;
        
        
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

            Debug.Log(startPosX + " " + endPosX);
            
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
        
        // TODO: sauber machen. Die Funktion ist viel zu lang.
        // private void GenerateEntrances(GameObject _street, int _startPosX, int _endPosX, int _startPosY, int _endPosY, int _entrancesAmount)
        // {
        //     
        //     int gridWidth  = MapManager.GetWidth;
        //     int gridHeight = MapManager.GetHeight;
        //
        //     int startPosX = gridWidth  * _startPosX;
        //     int endPosX   = gridWidth  * _endPosX;
        //     
        //     
        //     int startPosY = gridHeight * _startPosY;
        //     int endPosY   = gridHeight * _endPosY;
        //     
        //
        //     int stageAmountX = 0;
        //     int stageAmountY = 0;
        //     
        //     if ( startPosX !=  endPosX)
        //     {
        //         stageAmountX = gridWidth / _entrancesAmount - 1;
        //     }
        //
        //     if (startPosY != endPosY)
        //     {
        //          stageAmountY = gridHeight / _entrancesAmount - 1;
        //     }
        //     
        //     if (startPosX > 0 && startPosX == endPosX)
        //     {
        //         startPosX--;
        //         endPosX--;
        //     }
        //     
        //     if( endPosX > startPosX)
        //     {
        //         startPosX++;
        //     }
        //
        //     if (startPosY > 0 && startPosY == endPosY)
        //     {
        //         startPosY--;
        //         endPosY--;
        //     }
        //     
        //     if(endPosY > startPosY)
        //     {
        //         startPosY++;
        //     }
        //
        //
        //     int ePosX = stageAmountX;
        //     int ePosY = stageAmountY;
        //
        //
        //     for (int i = 0; i < _entrancesAmount; i++)
        //     {
        //         
        //         int randomValueX = endPosX ;
        //         if (startPosX != endPosX)
        //         {
        //             
        //             randomValueX = (int) GetRandomValue(startPosX, ePosX - 2);
        //         }
        //
        //         int randomValueY = endPosY;
        //         if (startPosY != endPosY)
        //         {
        //             
        //             randomValueY= (int) GetRandomValue(startPosY, ePosY - 2);
        //         }
        //         
        //         randomValueX *= 2;
        //         randomValueY *= 2;
        //         
        //         PlaceStreetOnPos(_street,randomValueX,randomValueY);
        //         
        //         startPosX += stageAmountX;
        //         startPosY += stageAmountY;
        //         ePosX += stageAmountX;
        //         ePosY += stageAmountY;
        //     }
        //
        // }

        private float GetRandomValue(int _start, int _end)
        {
            Debug.Log("------------");
            Debug.Log("Start:" + _start + "| End:" + _end + " | Range:" + (_end- _start));
            float value = Random.Range(_start,_end);
            Debug.Log("Var:" + (value - _start +" | Position" + value));
            return value; //value;
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
    }
}