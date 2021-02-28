// File     : Street.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;

namespace Code.Scripts
{
    public class Street : MonoBehaviour
    {
        // -------------------------------------------------------------------------------------------------------------
        #region Init

        // Events ------------------------------------------------------------------------------------------------------
        
        public static Action<Street> OnCreation;
        
        // Serialize Fields---------------------------------------------------------------------------------------------
        [SerializeField] private MeshRenderer MeshRenderer;

        // private -----------------------------------------------------------------------------------------------------
        private int PosX;
        private int PosY;

        // public Properties -------------------------------------------------------------------------------------------
        public int GetPosX => PosX;
        public int GetPosY => PosY;

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------

        #region Functions
        
        // Init --------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initialize the street with a grid position.
        /// </summary>
        /// <param name="_posX">Hands over the x position of the grid.</param>
        /// <param name="_posY">Hands over the y position of the grid.</param>
        public void Init(int _posX, int _posY)
        {
            PosX = _posX;
            PosY = _posY;
            OnCreation?.Invoke(this);
        }

        
        /// <summary>
        /// Sets the sprite of the street.
        /// </summary>
        /// <param name="_sprite">Hands over a sprite to change the street material.</param>
        public void SetSprite(Material _sprite)
        {
            MeshRenderer.material = _sprite;
        }

        #endregion
        // -------------------------------------------------------------------------------------------------------------
    }
}