// File     : MeshGenerator.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Code.Scripts
{
    
    public class MeshGenerator : MonoBehaviour
    {
        private Mesh Mesh;

        private Vector3[] vertices;
        private int[] triangles;


        [SerializeField] private int XSize = 20;
        [SerializeField] private int ZSize = 20;
        [SerializeField] private float Height;
        [SerializeField] private float XPerl;
        [SerializeField] private float ZPerl;
        private void Start()
        {
            Mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = Mesh;
            
        }


        [SerializeField] private float OffsetXSpeed;
        [SerializeField] private float OffsetYSpeed;
        private void Update()
        {
            CreateShape();
            
            UpdateMesh();
        }

        

        private void CreateShape()
        {
            
            CreateVertices();
            
            
            triangles = new int[XSize * ZSize * 6];
            
            int vert = 0;
            int tris = 0;

            for (int z = 0; z < ZSize; z++)
            {
                for (int i = 0; i < XSize; i++)
                {
                    
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + XSize + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert +1;
                    triangles[tris + 4] = vert + XSize + 1;
                    triangles[tris + 5] = vert + XSize + 2;
                    vert++;
                    tris += 6;
                }

                vert++;
            }

        }
        
        
        private void CreateVertices()
        {
            vertices = new Vector3[(XSize + 1) * (ZSize + 1)];
            
            
            
            for (int i =0,  z = 0; z <= ZSize; z++)
            {
                for (int x = 0; x <= XSize; x++)
                {
                    float time = Time.realtimeSinceStartup;
                    float y = Mathf.PerlinNoise(x * XPerl + OffsetXSpeed * time, z * ZPerl + OffsetYSpeed * time) * Height;
                    vertices[i] = new Vector3(x,y,z);
                    i++;
                }
            }
        }
        
        private void UpdateMesh()
        {
            Mesh.Clear();
            Mesh.vertices = vertices;
            Mesh.triangles = triangles;
            
            Mesh.RecalculateNormals();
        }

        private void OnDrawGizmos()
        {
            if (vertices == null) return;
            
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i],0.1f);
            }
        }
    }
}