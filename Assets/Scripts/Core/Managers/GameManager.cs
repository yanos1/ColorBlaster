using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Managers
{
    public class GameManager
    {
        public bool IsGameActive => isGameActive;

        public float CurrentObjectsSpeed
        {
            get => currentObjectsSpeed;
            set => currentObjectsSpeed = Mathf.Min(maxObstacleSpeed, value);
        }

        private static int maxObstacleSpeed => 7;

        private float lastObstacleUpdateTime;
        private float ChangeDifficultyInterval = 18f;
        private float currentObjectsSpeed;
        private float savedObjectSpeed;
        private bool isGameActive;


        public GameManager(float baseObjectSpeed)
        {
            currentObjectsSpeed = baseObjectSpeed;
            isGameActive = false;
            lastObstacleUpdateTime = Time.time;
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, OnStartGame);
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, OnGameOver);
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, StopAllObjects);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, ResumeAllObjects);
        }

        public void OnDestroy()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, OnStartGame);
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, OnGameOver);
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, StopAllObjects);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, ResumeAllObjects);
        }

        private void ResumeAllObjects(object obj)
        {
            currentObjectsSpeed = savedObjectSpeed;
        }

        private void StopAllObjects(object obj)
        {
            savedObjectSpeed = CurrentObjectsSpeed;
            currentObjectsSpeed = 0;
        }


        private void OnGameOver(object obj)
        {
            isGameActive = false;
        }

        private void OnStartGame(object obj)
        {
            isGameActive = true;
            CoreManager.instance.TimeManager.RunFunctionInfinitely(EventNames.IncreaseGameDifficulty, null,
                ChangeDifficultyInterval);
        }
    }
}