// File     : ResourceMapmanager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using System.Collections.Generic;
using Assets.Code.Scripts.UI_Scripts;
using UnityEngine;

namespace Code.Scripts
{
    public class ResourceMapManager : MonoBehaviour
    {
        public static bool HudIsActive = false;
        [SerializeField] private GameObject PathLinePrefab;
        [SerializeField] private GameObject HudResMap;
        [SerializeField] private GameObject Hud2;


        private void Awake()
        {
            GameManager.OnResCamActive += OnGather;
            GameManager.OnMapCamActiv += OnMap;
        }

        private void OnMap()
        {
            Hud2.SetActive(false);
        }

        private void OnGather()
        {
            Hud2.SetActive(true);
        }
    }
}