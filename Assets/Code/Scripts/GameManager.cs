using System;
using Assets.Code.Scripts.UI_Scripts;
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

        [SerializeField] private float GameSpeed = 1f;
        [SerializeField] private float ReduceFoodPerUnitEveryDay;
        private bool Lost;
        private bool Won;
        private Camera MapCamera;
        private Camera ResCamera;
        
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

            LoadScene(1);
        }

        private void SetResCamera(Camera _obj)
        {
            ResCamera = _obj;
            ResCamera.enabled = false;
            //OnMap();
        }

        private void SetMapCamera(Camera _obj)
        {
            MapCamera = _obj;

        }
        //
         private void OnGather()
         {
             ResCamera.enabled = true;
             MapCamera.enabled = false;
             OnResCamActive?.Invoke();
         }
        //
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
            //ToDo: Lose Window Game Speed = 0; return to Menu after clicking button 
        }

        public void PlayerWin()
        {
        //ToDo: Win Window Game Speed = 0; return to menu after clicking Button
        Debug.Log("Win");
        }

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
    }
