using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Managers
{
    public class GameManager
    {
        private float lastObstacleUpdateTime;
        private float ChangeDifficultyInterval = 18f;
        private bool isGameActive;
        public bool IsGameActive => isGameActive;


        public GameManager()
        {
            isGameActive = false;
            lastObstacleUpdateTime = Time.time;
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, OnStartGame);
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, OnGameOver);
        }


        public void OnDestroy()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, OnStartGame);
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, OnGameOver);
        }

        private void OnGameOver(object obj)
        {
            isGameActive = false;
        }

        private void OnStartGame(object obj)
        {
            isGameActive = true;
            CoreManager.instance.TimeManager.RunFunctionInfinitely(EventNames.IncreaseGameDifficulty,null,ChangeDifficultyInterval);
        }
    }
}