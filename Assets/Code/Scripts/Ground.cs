// File     : Ground.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts.Grid.DanielB;
using Code.Scripts.Map;
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

        public bool IsBlockedH
        {
            get => IsBlocked;
            set
            {
                IsBlocked = value;

                // MyGrid<Node> Nodes = new MyGrid<Node>(10,10);
                // Nodes[GetWidth,Height].IsMovable = value;
            }
        }
        
        [SerializeField] private EGround GroundSignature;
        [SerializeField] private MeshRenderer Renderer;
        
        private int Width;
        private int Height;

        public int GetWidth => Width;
        public int GetHeight => Height;
        

        public static Ground[,] Grounds;

        public static void SetGroundSize(int _width, int _height)
        {
            Grounds = new Ground[_width, _height];
        }

        private void Start()
        {
            SetMeshActive(false);
        }

        public EGround GetGroundSignature => GroundSignature;


        public void Init(int _widthPos, int _heightPos)
        {
            Grounds[_widthPos, _heightPos] = this;
            Width = _widthPos;
            Height = _heightPos;
            gameObject.name = "Gras [" + Width + ":" + Height + "]";
        }

        public void SetMeshActive(bool _isActive)
        {
            Renderer.enabled = _isActive;
        }

        public void SetGroundSignature(EGround _groundSignature)
        {
            GroundSignature = _groundSignature;
        }
        
        

    }
}