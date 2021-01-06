// File     : Ground.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;

namespace Code.Scripts
{

    public enum EGround
    {
        None,
        Street,
        Gras,
    }
    
    public class Ground : MonoBehaviour
    {
        
        
        public bool IsBlocked;
        [SerializeField] private EGround GroundSignature;
        
        private int Width;
        private int Height;

        public int GetWidth => Width;
        public int GetHeight => Height;
        

        public static Ground[,] Grounds;

        public static void SetGroundSize(int _width, int _height)
        {
            Grounds = new Ground[_width, _height];
        }

        public EGround GetGroundSignature => GroundSignature;


        public void Init(int _widthPos, int _heightPos)
        {
            //Grounds[_widthPos, _heightPos] = this;
            Width = _widthPos;
            Height = _heightPos;
            gameObject.name = "Gras [" + Width + ":" + Height + "]";
        }

    }
}