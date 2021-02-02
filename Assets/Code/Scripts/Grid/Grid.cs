// File     : Grid.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int X;
        public int Y;
    }
    private int Width;
    private int Height;

    public TGridObject[,] GridArray;

    public Grid(int _width, int _height, Func<Grid<TGridObject>,int,int,TGridObject> createGridObject)
    {
        Width = _width;
        Height = _height;

        GridArray = new TGridObject[_width, _height];

        for (int x = 0; x < GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < GridArray.GetLength(1); y++)
            {
                GridArray[x, y] = createGridObject(this,x,y);
            }
        }
    }

    public Vector3 GetWorldPosition(int _x, int _y)
    {
        return new Vector3(_x, 0, _y);
    }

    public int GetWidth()
    {
        return Width;
    }

    public int GetHeight()
    {
        return Height;
    }

    public void GetXY(Vector3 _worldPosition, out int _x, out int _y)
    {
        _x = Mathf.FloorToInt(_worldPosition.x);
        _y = Mathf.FloorToInt(_worldPosition.y);
    }

    public void SetGridObject(int _x, int _y, TGridObject value)
    {
        if (_x >= 0 && _y >= 0 && _x < Width && _y < Height)
        {
            GridArray[_x, _y] = value;
            if (OnGridObjectChanged != null)
            {
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { X = _x, Y = _y });
            }
        }
    }

    public void SetGridObject(Vector3 _worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(_worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int _x, int _y)
    {
        if (_x >= 0 && _y >= 0 && _x < Width && _y < Height)
        {
            return GridArray[_x, _y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 _worldPosition)
    {
        int x, y;
        GetXY(_worldPosition ,out x ,out y);
        return GetGridObject(x, y);
    }
}