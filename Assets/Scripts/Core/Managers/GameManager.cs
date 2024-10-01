using System;
using UnityEngine;

namespace Core.Managers
{
    public class GameManager
    {
        public bool IsGameActive => _isGameActive;
        public bool IsRunActive => _isRunActive;

        private bool _isGameActive;
        private bool _isRunActive;

        public GameManager()
        {
            _isGameActive = false;  // Game is active as long as we don't get to the gameover screen
            _isRunActive = false;   // Run is active when the player is currently playing
            
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
            _isRunActive = true;
        }

        private void StopAllObjects(object obj)
        {
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
        }
    }
}