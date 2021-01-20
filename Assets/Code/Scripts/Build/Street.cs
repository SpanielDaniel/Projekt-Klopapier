// File     : Street.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts.Grid.DanielB;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Build
{
    public class Street : MonoBehaviour
    {
        public static Action<Street> OnCreation;
        [SerializeField] private MeshRenderer MeshRenderer;

        private int PosX;
        private int PosY;
        public int GetPosX => PosX;
        public int GetPosY => PosY;

        private int IsLeftOpen;
        private int IsRightOpen;
        private int IsUpOpen;
        private int IsDownOpen;

        public void Init(int _posX, int _posY)
        {
            PosX = _posX;
            PosY = _posY;
            OnCreation?.Invoke(this);
        }

        public void SetOpenSides(int _left, int _up, int _right, int _down)
        {
            IsLeftOpen = _left;
            IsUpOpen = _up;
            IsRightOpen = _right;
            IsDownOpen = _down;
        }
        
        public void SetSprite(Material _sprite)
        {
            MeshRenderer.material = _sprite;
        }
    }
}