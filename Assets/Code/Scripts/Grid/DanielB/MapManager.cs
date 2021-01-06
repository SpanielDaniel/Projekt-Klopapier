// File     : MapManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;

namespace Code.Scripts.Grid.DanielB
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private int Width;
        [SerializeField] private int Height;

        public int GetWidth => Width;
        public int GetHeight => Height;

        // public void SetGroundMovable(int _x, int _y, bool _isMovable)
        // {
        //     
        // }
    }
}