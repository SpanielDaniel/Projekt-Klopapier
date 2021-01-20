// File     : MyGrid.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;

namespace Code.Scripts.Grid.DanielB
{
    public class MyGrid<T>
    {
        private int Width;
        private int Height;

        public int GetWidth => Width;
        public int GetHeight => Height;

        public T[,] Grid;

        public MyGrid(int _width,int _height)
        {
            Width = _width;
            Height = _height;

            Grid = new T[Width, Height];
        }
    }
}