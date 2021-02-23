using System;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class GameManager : Singleton<GameManager>
    {
        // -------------------------------------------------------------------------------------------------------------

        #region Init

        [SerializeField] private float GameSpeed = 1f;
        public float GetGameSpeed => GameSpeed;

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------

        protected override void AwakeFunction()
        {
            base.AwakeFunction();
            
            DontDestroyOnLoad(this);
            
            LoadScene(1);
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
