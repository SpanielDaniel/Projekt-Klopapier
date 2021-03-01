using System;
using Assets.Code.Scripts.UI_Scripts;
using Buildings;
using Code.Scripts;
using Player;
using UI_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class GameManager : Singleton<GameManager>
    {
        // -------------------------------------------------------------------------------------------------------------

        #region Init


        public static event Action OnMapCamActive;
        public static event Action OnResCamActive;
        public static event Action LostWindow;
        public static event Action WonWindow;

        [SerializeField] private float GameSpeed = 1f;
        [SerializeField] private float ReduceFoodPerUnitEveryDay;
        private bool CanLoseBool;
        private bool Lost;
        private bool Won;
        private Camera MapCamera;
        private Camera ResCamera;
        private Base Base;
        
        public bool Win
        {
            get => Won;

            set
            {
                Won = value;
                if (Won)
                {
                    PlayerWin();
                }
            }
        }

        public bool Lose
        {
            get => Lost;
            set
            {
                Lost = value;
                if (Lost)
                {
                    PlayerLose();
                }
            }
        }
        
        
        public float GetGameSpeed => GameSpeed;

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------

        protected override void AwakeFunction()
        {
            base.AwakeFunction();
            
            DontDestroyOnLoad(this);
            Timer.OnDayChanged += ReduceFood;
            UIUnitManager.OnButtonGather += OnGather;
            CameraManager.OnCameraCreation += SetMapCamera;
            MeshCameraHandler.OnCameraCreation += SetResCamera;
            ResourceMapManager.OnButtonClose += OnMap;
            UIResourcesManager.OnButton_MapRes += OnGather;
            Base.OnBaseCreated += CanLose;
            Base.OnDestroy += PlayerLose;

            LoadScene(1);
        }

        private void SetResCamera(Camera _obj)
        {
            ResCamera = _obj;
            OnGather();
            OnMap();
        }

        private void SetMapCamera(Camera _obj)
        {
            MapCamera = _obj;

        }

        private void OnGather()
        {
            ResCamera.enabled = true;
            MapCamera.enabled = false;
            OnResCamActive?.Invoke();
        }

        private void OnMap()
        {
            ResCamera.enabled = false;
            MapCamera.enabled = true;
            OnMapCamActive?.Invoke();
        }

        private void ReduceFood(int obj)
        {
            PlayerData.GetInstance.FoodAmountH -= (int) ((float) Unit.Units.Count * ReduceFoodPerUnitEveryDay);
        }

        private void CanLose()
        {
            CanLoseBool = true;
        }

        // -------------------------------------------------------------------------------------------------------------

        #region Functions

        public void LoadScene(int _sceneIndex)
        {
            SceneManager.LoadScene(_sceneIndex);
            if (_sceneIndex == 2)
            {
                SceneManager.LoadScene (3,LoadSceneMode.Additive);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void PlayerLose()
        {
            Time.timeScale = 0;
            LostWindow?.Invoke();
        }

        public void PlayerWin()
        {
            Time.timeScale = 0;
            WonWindow?.Invoke();
        }

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
    }
