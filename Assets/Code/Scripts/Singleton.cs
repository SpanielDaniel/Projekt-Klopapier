﻿
    using System;
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T Instance;

        public static T GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = GameObject.FindObjectOfType<T>();
                    if (Instance == null)
                    {
                        Instance = new GameObject("Instance of" + typeof(T)).AddComponent<T>();
                    }
                }
                return Instance;
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
        }
    }