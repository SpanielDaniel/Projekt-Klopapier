// File     : MapManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Assets.Code.Scripts.UI_Scripts;
using UnityEngine;

namespace Code.Scripts.Grid.DanielB
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private int Width;
        [SerializeField] private int Height;
        [SerializeField] private GameObject Hud;

        public int GetWidth => Width;
        public int GetHeight => Height;

        private void Awake()
        {
            GameManager.OnResCamActive += OnGather;
            GameManager.OnMapCamActiv += OnMap;
        }

        private void OnMap()
        {
            Hud.SetActive(true);
        }

        private void OnGather()
        {
            Hud.SetActive(false);
        }
    }
}