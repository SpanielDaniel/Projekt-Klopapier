using System;
using UnityEngine;



    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private float GameSpeed = 1f;
        public float GetGameSpeed => GameSpeed;
    }
