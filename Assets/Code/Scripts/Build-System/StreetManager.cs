// File     : StreetManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using Code.Scripts.Grid.DanielB;
using UnityEngine;

namespace Code.Scripts
{
    public class StreetManager : MonoBehaviour
    {
        // -------------------------------------------------------------------------------------------------------------
        
        #region Init
        
        // Serialize Fields --------------------------------------------------------------------------------------------
        [SerializeField] private MapManager MapManager;
        [SerializeField] private  Material[] StreetsMaterial;
        
        // private -----------------------------------------------------------------------------------------------------
        private MyGrid<Street> Streets;

        #endregion
        // -------------------------------------------------------------------------------------------------------------
        private void Awake()
        {
            Street.OnCreation += StreetCreated;
            Streets = new MyGrid<Street>(MapManager.GetWidth , MapManager.GetHeight );
        }
        
        // -------------------------------------------------------------------------------------------------------------
        
        #region Functions

        /// <summary>
        /// Adds a street to the list.
        /// </summary>
        /// <param name="_street">Hands over a street to add.</param>
        private void StreetCreated(Street _street)
        {
            Streets.Grid[(_street.GetPosX / 2), (_street.GetPosY / 2)] = _street;
        }
        
        /// <summary>
        /// Changes the material of all streets, that the streets are connected.
        /// </summary>
        public void ChangeStreetMaterial()
        {
            foreach (Street street in Streets.Grid)
            {
                if (street != null)
                {
                    int materialNumber = CalculateBinaryMaterialNumber(street);
                    street.SetSprite(StreetsMaterial[materialNumber]);
                }
            }
        }

        /// <summary>
        /// Calculates the material number of the a street.
        /// </summary>
        /// <param name="_street">Street to check neighbours.</param>
        /// <returns></returns>
        private int CalculateBinaryMaterialNumber(Street _street)
        {
            
            int IsLeftSideStreet  = 0;
            int IsRightSideStreet = 0;
            int IsTopSideStreet   = 0;
            int IsDownSideStreet  = 0;


            // is left neighbour a street

            int leftPosX = _street.GetPosX / 2 - 1;

            if (!(leftPosX < 0))
            {
                if (Streets.Grid[leftPosX, _street.GetPosY / 2] != null)
                {
                    IsLeftSideStreet = 1;
                }
            }
            else
            {
                IsLeftSideStreet = 1;
            }

            // is right neighbour a street
            int rightPosX = _street.GetPosX / 2 + 1;
            
            if (!(rightPosX >= Streets.GetWidth ))
            {
                if (Streets.Grid[rightPosX, _street.GetPosY / 2] != null)
                {
                    IsRightSideStreet = 1;
                }
            }
            else
            {
                IsRightSideStreet = 1;
            }

            // is top neighbour a street
            int topPosY = _street.GetPosY / 2 + 1;

            if (!(topPosY >= Streets.GetHeight))
            {
                if (Streets.Grid[_street.GetPosX / 2, topPosY] != null)
                {
                    IsTopSideStreet = 1;
                }
            }
            else
            {
                IsTopSideStreet = 1;
            }
            
            // is down neighbour a street
            int downPosY = _street.GetPosY / 2 - 1;
            
            if (!(downPosY < 0))
            {
                if (Streets.Grid[_street.GetPosX/ 2, downPosY] != null)
                {
                    IsDownSideStreet = 1;
                }
            }
            else
            {
                IsDownSideStreet = 1;
            }
            
            // binary code of the material
            return 1 * IsLeftSideStreet + 2 * IsTopSideStreet + 4 * IsRightSideStreet + 8 * IsDownSideStreet;
        }

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------

    }
}