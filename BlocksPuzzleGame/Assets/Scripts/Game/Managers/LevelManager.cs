using Data;
using Game.Core;
using Game.Core.GridSystem;
using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class LevelManager : MonoBehaviour, IProvidable
    {
        int currentLevel;
        LevelData currentLevelData;
        public static event Action<LevelData> currentLevelSettedEvent;
        public static event Action levelStartedEvent;
        public static event Action levelSuccesedEvent;
        private void Awake()
        {
            ServiceProvider.Register(this);
        }
        private void SetupLevel()
        {
            ServiceProvider.GetBlocksManager.CreateBlockObjects(currentLevelData.blockDatas);
            StartLevel(); //Delete later
        }
        public void GetCurrentLevel(GameData gameData)
        {
            currentLevel = gameData.currentLevel;
            currentLevelData = gameData.levelsData[currentLevel % gameData.levelsData.Count];
            currentLevelSettedEvent?.Invoke(currentLevelData);
            SetupLevel();
        }
        public void StartLevel()
        {
            levelStartedEvent?.Invoke();
        }
        public void LevelSuccesed()
        {
            currentLevel++;
            ServiceProvider.GetDataHandler.SaveCurrentLevel(currentLevel);
            levelSuccesedEvent?.Invoke();
        }
        public void RestartScene()
        {
            SceneManager.LoadScene(0);
        }
        public void OnEnable()
        {
            GridObject.gridCompletedEvent += LevelSuccesed;
        }
        public void OnDisable()
        {
            GridObject.gridCompletedEvent -= LevelSuccesed;
        }
    }
}
