// File     : Grid.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System;
using UnityEngine;

public class Grid<TGridObject> : MonoBehaviour
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int X;
        public int Z;
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
            for (int z = 0; z < GridArray.GetLength(1); z++)
            {
                GridArray[x, z] = createGridObject(this,x,z);
            }
        }
    }

    public Vector3 GetWorldPosition(int _x, int _z)
    {
        return new Vector3(_x, 0, _z);
    }

    public int GetWidth()
    {
        return Width;
    }

    public int GetHeight()
    {
        return Height;
    }

    public void GetXZ(Vector3 _worldPosition, out int _x, out int _z)
    {
        _x = Mathf.FloorToInt(_worldPosition.x);
        _z = Mathf.FloorToInt(_worldPosition.z);
    }

    public void SetGridObject(int _x, int _z, TGridObject value)
    {
        if (_x >= 0 && _z >= 0 && _x < Width && _z < Height)
        {
            GridArray[_x, _z] = value;
            if (OnGridObjectChanged != null)
            {
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { X = _x, Z = _z });
            }
        }
    }

    public void SetGridObject(Vector3 _worldPosition, TGridObject value)
    {
        int x, z;
        GetXZ(_worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject(int _x, int _z)
    {
        if (_x >= 0 && _z >= 0 && _x < Width && _z < Height)
        {
            return GridArray[_x, _z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 _worldPosition)
    {
        int x, z;
        GetXZ(_worldPosition ,out x ,out z);
        return GetGridObject(x, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, 0, Height));
    }
}