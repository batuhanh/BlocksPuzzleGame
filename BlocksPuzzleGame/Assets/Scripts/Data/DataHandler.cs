using Game.Core;
using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

namespace Data
{
    public class DataHandler : MonoBehaviour, IProvidable
    {
        private string rootDir = "/Data/";
        private GameData gameData;
        string fileName = "GameData.txt";

        private void Awake()
        {
            ServiceProvider.Register(this);

        }
        private void Start()
        {
            gameData = LoadGameData();
            if (ServiceProvider.GetLevelManager)
                ServiceProvider.GetLevelManager.GetCurrentLevel(gameData);

        }

        public void SaveLevelData(LevelData levelData)
        {
            gameData = LoadGameData();
            if (gameData.levelsData == null)
                gameData.levelsData = new List<LevelData>();
            gameData.levelsData.Add(levelData);
            SaveGameData(gameData);
        }
        public void SaveCurrentLevel(int newLevel)
        {
            gameData = LoadGameData();
            gameData.currentLevel = newLevel;
            SaveGameData(gameData);
        }
        public void SaveGameData(GameData gd)
        {
            string dir = Application.persistentDataPath + rootDir + "GameData/";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string json = JsonUtility.ToJson(gd);
            File.WriteAllText(dir + fileName, json);
        }
        public GameData LoadGameData()
        {
            string fullPath = Application.persistentDataPath + rootDir + "GameData/" + fileName;
            GameData gd = new GameData();
            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                gd = JsonUtility.FromJson<GameData>(json);
            }
            else
            {
                var textFile = Resources.Load<TextAsset>("Text/GameData");
                if(textFile != null)
                {
                    string json = textFile.text;
                    gd = JsonUtility.FromJson<GameData>(json);
                    SaveGameData(gd);
                }
                else
                {
                    Debug.Log("There is no file in that path" + textFile);
                }
                Debug.Log("There is no file in that path" + fullPath);
            }
            return gd;
        }

    }
}
