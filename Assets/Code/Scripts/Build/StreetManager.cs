// File     : StreetManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts.Grid.DanielB;
using UnityEngine;

namespace Build
{
    public class StreetManager : MonoBehaviour
    {
        private MyGrid<Street> Streets;
        [SerializeField] private MapManager MapManager;

        [SerializeField] private  Material[] StreetsMaterial;
        
        private void Awake()
        {
            Street.OnCreation += StreetCreated;
            Streets = new MyGrid<Street>(MapManager.GetWidth , MapManager.GetHeight );
        }
        private void StreetCreated(Street _obj)
        {
            Streets.Grid[(_obj.GetPosX / 2), (_obj.GetPosY / 2)] = _obj;
        }
        public void ChangeStreetMaterial()
        {
            foreach (Street street in Streets.Grid)
            {
                if (street != null)
                {
                    int hash = CheckLeftSide(street);
                    street.SetSprite(StreetsMaterial[hash]);
                }
            }
        }

        private int CheckLeftSide(Street _obj)
        {
            // int wert speichern
            int IsLeftSideStreet  = 0;
            int IsRightSideStreet = 0;
            int IsTopSideStreet   = 0;
            int IsDownSideStreet  = 0;


            int leftPosX = _obj.GetPosX / 2 - 1;

            if (!(leftPosX < 0))
            {
                if (Streets.Grid[leftPosX, _obj.GetPosY / 2] != null)
                {
                    IsLeftSideStreet = 1;
                }
            }
            else
            {
                IsLeftSideStreet = 1;
            }

            int rightPosX = _obj.GetPosX / 2 + 1;
            
            if (!(rightPosX >= Streets.GetWidth ))
            {
                if (Streets.Grid[rightPosX, _obj.GetPosY / 2] != null)
                {
                    IsRightSideStreet = 1;
                }
            }
            else
            {
                IsRightSideStreet = 1;
            }


            int topPosY = _obj.GetPosY / 2 + 1;

            if (!(topPosY >= Streets.GetHeight))
            {
                if (Streets.Grid[_obj.GetPosX / 2, topPosY] != null)
                {
                    IsTopSideStreet = 1;
                }
            }
            else
            {
                IsTopSideStreet = 1;
            }
            
            
            
            
            int downPosY = _obj.GetPosY / 2 - 1;
            

            if (!(downPosY < 0))
            {
                if (Streets.Grid[_obj.GetPosX/ 2, downPosY] != null)
                {
                    IsDownSideStreet = 1;
                }
            }
            else
            {
                IsDownSideStreet = 1;
            }
            
            _obj.SetOpenSides(IsLeftSideStreet,IsTopSideStreet,IsRightSideStreet,IsDownSideStreet);

            return 1 * IsLeftSideStreet + 2 * IsTopSideStreet + 4 * IsRightSideStreet + 8 * IsDownSideStreet;
        }
    }
}