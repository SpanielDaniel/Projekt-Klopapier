// File     : Waypoints.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints
{
    public List<GameObject> waypoints;

    public void SetWayPoints(List<GameObject> _path)
    {
        waypoints = _path;
    }
}
