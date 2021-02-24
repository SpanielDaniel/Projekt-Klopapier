using System;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class GameManager : Singleton<GameManager>
    {
        // -------------------------------------------------------------------------------------------------------------

        #region Init

        [SerializeField] private float GameSpeed = 1f;
        [SerializeField] private float ReduceFoodPerUnitEveryDay;
        public float GetGameSpeed => GameSpeed;

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------

        protected override void AwakeFunction()
        {
            base.AwakeFunction();
            
            DontDestroyOnLoad(this);
            Timer.OnDayChanged += ReduceFood;
            LoadScene(1);
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
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
    }
