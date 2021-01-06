// File     : MyGrid.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;

namespace Code.Scripts.Grid.DanielB
{
    public class MyGrid<T>
    {
        [SerializeField] private int Width;
        [SerializeField] private int Height;

        public T[,] Grid;

        public MyGrid(int _width,int _height)
        {
            Width = _width;
            Height = _height;

            Grid = new T[Width, Height];
        }
    }
}