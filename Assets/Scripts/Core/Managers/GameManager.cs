using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Managers
{
    public class GameManager
    {
        public bool IsGameActive => _isGameActive;
        public bool IsRunActive => _isRunActive;

        public float CurrentObjectsSpeed
        {
            get => _currentObjectsSpeed;
            set => _currentObjectsSpeed = Mathf.Min(maxObstacleSpeed, value);
        }
        
        private Coroutine increaseGameDiffucultyCoroutine;
        private static int maxObstacleSpeed => 7;

        private float _lastObstacleUpdateTime;
        private readonly float ChangeDifficultyInterval = 18f;
        private float _baseObjectSpeed;
        private float _currentObjectsSpeed;
        private float _savedObjectSpeed;
        private bool _isGameActive;
        private bool _isRunActive;


        public GameManager(float baseObjectSpeed)
        {
            _baseObjectSpeed = baseObjectSpeed;
            _currentObjectsSpeed = _baseObjectSpeed;
            _isGameActive = false;
            _isRunActive = false;
            _lastObstacleUpdateTime = Time.time;
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, OnStartGame);
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, OnGameOver);
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, StopAllObjects);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, ResumeAllObjects);
            CoreManager.instance.EventManager.AddListener(EventNames.IncreaseGameDifficulty, IncreaseObstacleSpeed);
        }

        public void OnDestroy()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, OnStartGame);
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, OnGameOver);
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, StopAllObjects);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, ResumeAllObjects);
            CoreManager.instance.EventManager.RemoveListener(EventNames.IncreaseGameDifficulty, IncreaseObstacleSpeed);

        }

        private void IncreaseObstacleSpeed(object obj)
        {
            _currentObjectsSpeed += 0.3f;
        }

        private void ResumeAllObjects(object obj)
        {
            if (increaseGameDiffucultyCoroutine == null)
            {
                increaseGameDiffucultyCoroutine = CoreManager.instance.MonoRunner.StartCoroutine(
                    CoreManager.instance.TimeManager.RunFunctionInfinitely(EventNames.IncreaseGameDifficulty, 2, ChangeDifficultyInterval));
            }
            _currentObjectsSpeed = _savedObjectSpeed;
            _isRunActive = true;
        }

        private void StopAllObjects(object obj)
        {
            if (increaseGameDiffucultyCoroutine != null)
            {
                CoreManager.instance.MonoRunner.StopCoroutine(increaseGameDiffucultyCoroutine);
                increaseGameDiffucultyCoroutine = null;
            }
            _savedObjectSpeed = CurrentObjectsSpeed;
            _currentObjectsSpeed = 0;

            _isRunActive = false;
        }



        private void OnGameOver(object obj)
        {
            _isGameActive = false;
        }

        private void OnStartGame(object obj)
        {
            _isGameActive = true;
            _isRunActive = true;
            increaseGameDiffucultyCoroutine = CoreManager.instance.MonoRunner.StartCoroutine(CoreManager.instance.TimeManager.RunFunctionInfinitely(EventNames.IncreaseGameDifficulty, null,
                ChangeDifficultyInterval));
            CurrentObjectsSpeed = _baseObjectSpeed;
        }
    }
}