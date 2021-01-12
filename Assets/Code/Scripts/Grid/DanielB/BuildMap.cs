// File     : myBuildMap.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Code.Scripts.Grid.DanielB
{
    public class BuildMap : MonoBehaviour
    {
        private MyGrid<GameObject> GroundMap;
        [SerializeField] private MapManager MapManager;

        [SerializeField] private GameObject GroundObj;

        private int MapMultiplicator = 2;
        
        private void Start()
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
                    GroundMap.Grid[w, h].transform.position = new Vector3((float)w/MapMultiplicator,0,(float)h/MapMultiplicator);
                    SetGroundBlocked(w,h ,false);
                    GroundMap.Grid[w, h].GetComponent<Ground>().Init(w,h);
                }
            }
        }

        public void SetGroundBlocked(int _x, int _y, bool _isActive)
        {
            GroundMap.Grid[_x, _y].GetComponent<Ground>().IsBlocked = _isActive;
        }

        public bool IsGroundBlocked(int _x, int _y)
        {
            return GroundMap.Grid[_x, _y].GetComponent<Ground>().IsBlocked;
        }

        public EGround GetGroundSignature(int _x, int _y)
        {
            return GroundMap.Grid[_x, _y].GetComponent<Ground>().GetGroundSignature;
        }
        
    }
}