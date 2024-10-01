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

        public int ObstacleCrossedThisLevel
        {
            get => obstaclesCrossedThisLevel;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                obstaclesCrossedThisLevel = value;
                if (ObstacleCrossedThisLevel == CoreManager.instance.ControlPanelManager.obstaclesPerLevel)
                {
                    obstaclesCrossedThisLevel = 0;
                    CoreManager.instance.ControlPanelManager.Level++;
                    if (CoreManager.instance.ControlPanelManager.Level ==
                        CoreManager.instance.ControlPanelManager.levelSpeeds.Length)
                    {
                        CoreManager.instance.ControlPanelManager.Level = 0;
                        CoreManager.instance.ControlPanelManager.Session++;
                        Debug.Log("New Session Is Called!"); 

                    }
                    CoreManager.instance.EventManager.InvokeEvent(EventNames.LevelUp, (CoreManager.instance.ControlPanelManager.Session, CoreManager.instance.ControlPanelManager.Level));

                }
        }
    }

        private Coroutine increaseGameDiffucultyCoroutine;

        private float _lastObstacleUpdateTime;
        private bool _isGameActive;
        private bool _isRunActive;

        private int _totalObstaclesCrossed;
        private int obstaclesCrossedThisLevel;
        


        public GameManager()
        {
            _isGameActive = false;  // game is active as long as we dont get to the gameover screen
            _isRunActive = false;   // run is active when the player is currently playing
            _lastObstacleUpdateTime = Time.time;
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
            CoreManager.instance.ControlPanelManager.Level = 0;
            CoreManager.instance.ControlPanelManager.Session = 0;
            obstaclesCrossedThisLevel = 0;
            _isGameActive = true;
            _isRunActive = true;
        }
    }
}