using System;
using UnityEngine;


    [CreateAssetMenu(fileName = "New Building", menuName = "Building")]
    public class BuildingStats : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite BuldingTexture;
        public bool IsUpgradable;
        public Level[] Levels;
        
        [Serializable]
        public struct Level
        {
            public int MaxLife;
            public int WoodCosts;
            public int StoneCosts;
            public int SteelCosts;
        }
        
    }
