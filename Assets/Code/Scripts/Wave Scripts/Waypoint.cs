using System;
using UnityEngine;

namespace Code.Scripts.Wave_Scripts
{
    public enum EWaypointSignature
    {
        None,
        Entrance,
        Waypoint,
        Final,
    }
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private GameObject Sphere;
        [SerializeField] private Material EntranceMaterial;
        [SerializeField] private Material WayMaterial;
        [SerializeField] private Material FinalMaterial;
        [SerializeField] private MeshRenderer MeshRenderer;
        private EWaypointSignature WaypointSignature;

        private int PosX;
        private int PosZ;

        public int GetXPos => PosX;
        public int GetZPos => PosZ;

        public EWaypointSignature GetWaypointSignature => WaypointSignature;
        
        
        public void Init(EWaypointSignature _waypointSignature)
        {
            WaypointSignature = _waypointSignature;
            if (WaypointSignature == EWaypointSignature.None)
            {
                
            }

            switch (_waypointSignature)
            {
                case EWaypointSignature.None:
                    Sphere.SetActive(false);
                    break;
                case EWaypointSignature.Entrance:
                    MeshRenderer.material = EntranceMaterial;
                    break;
                case EWaypointSignature.Waypoint:
                    MeshRenderer.material = WayMaterial;
                    break;
                case EWaypointSignature.Final:
                    MeshRenderer.material = FinalMaterial;
                    break;
            }
        }

        public void SetPos(int _x, int _z)
        {
            PosX = _x;
            PosZ = _z;
        }
    }
}