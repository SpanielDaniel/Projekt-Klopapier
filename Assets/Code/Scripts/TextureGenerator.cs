// File     : TextureGenerator.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier


using UnityEngine;
using System.Collections;

namespace Code.Scripts
{
    public static class TextureGenerator
    {
        public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
        {
            Texture2D texture = new Texture2D(width,height);
            texture.SetPixels(colorMap);
            texture.Apply();
            return texture;
        }

        public static Texture2D TextureFromHeightMap(float[,] heightMap)
        {
            int w = heightMap.GetLength(0);
            int h = heightMap.GetLength(1);
            
            Color[] colorMap = new Color[w * h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    colorMap[y * w + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
                }
            }

            return TextureFromColorMap(colorMap, w, h);
        }
    }
}