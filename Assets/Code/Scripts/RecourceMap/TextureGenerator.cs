// File     : TextureGenerator.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;

namespace Code.Scripts
{
    public static class TextureGenerator
    {
        /// <summary>
        /// Set pixel of a texture wit a color
        /// </summary>
        /// <param name="colorMap">Colors to add to the texture.</param>
        /// <param name="width">width position</param>
        /// <param name="height">height position</param>
        /// <returns>Colored Texture.</returns>
        private static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
        {
            Texture2D texture = new Texture2D(width,height);
            texture.SetPixels(colorMap);
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Get a specific color map value of a high map.
        /// </summary>
        /// <param name="heightMap"></param>
        /// <returns></returns>
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