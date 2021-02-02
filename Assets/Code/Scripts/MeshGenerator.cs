// File     : MeshGenerator.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = System.Random;

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
        private float[,] heightMap;
        [SerializeField] private Texture2D Texture;
        [SerializeField] private ColorHeight[] ColorHeights;
        
        [SerializeField] private float OffsetXSpeed;
        [SerializeField] private float OffsetYSpeed;
        
        [Serializable]
        struct ColorHeight
        {
             public float MaxHeight;
             public Color Color;
        }
        private void Start()
        {
            Random random = new Random();
            float f = random.Next(0, 2000);
            OffsetXSpeed = f;
            f = random.Next(0, 2000);
            OffsetYSpeed = f;
            
            Mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = Mesh;
            CreateShape();
            UpdateMesh();

            Texture = TextureGenerator.TextureFromHeightMap(heightMap);
            SetTexture();
        }
        
        
        


        
      

        private Vector2[] uvs;

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
            
            uvs = new Vector2[vertices.Length];
            
            for (int i =0,  z = 0; z <= ZSize; z++)
            {
                for (int x = 0; x <= XSize; x++)
                {
                    uvs[i] = new Vector2((float)x / XSize,(float)z / ZSize);
                    i++;
                }
            }
        }
        
        
        private void CreateVertices()
        {
            vertices = new Vector3[(XSize + 1) * (ZSize + 1)];
            heightMap = new float[XSize + 1,ZSize + 1];
            for (int i =0,  z = 0; z <= ZSize; z++)
            {
                for (int x = 0; x <= XSize; x++)
                {
                    float time = Time.realtimeSinceStartup;
                    float y = Mathf.PerlinNoise(x * XPerl + OffsetXSpeed * time, z * ZPerl + OffsetYSpeed * time) * Height;

                    if (y <= ColorHeights[3].MaxHeight)
                    {
                        y = ColorHeights[3].MaxHeight;
                    }
                    heightMap[x, z] = y;
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
            Mesh.uv = uvs;
            Mesh.RecalculateNormals();
        }

        
        private void SetTexture()
        {
            Texture2D tex = new Texture2D(XSize,ZSize);

            for (int i = 0; i < ZSize; i++)
            {
                for (int j = 0; j < XSize; j++)
                {
                    foreach (ColorHeight colorHeight in ColorHeights)
                    {
                        if (colorHeight.MaxHeight >= heightMap[j, i])
                        {
                            tex.SetPixel(j, i,colorHeight.Color);
                        }
                    }
                }
            }
            
            
            
            
            
            
            tex.Apply();
            tex.filterMode = FilterMode.Point;
            GetComponent<MeshRenderer>().material.mainTexture = tex;
        }
    }
}