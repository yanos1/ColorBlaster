using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Managers
{
    public class GameManager
    {
        private float lastObstacleUpdateTime;
        private float ChangeDifficultyInterval = 18f;


        public GameManager()
        {
            lastObstacleUpdateTime = Time.time;
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, StartGame);
        }

        private void StartGame(object obj)
        {
            CoreManager.instance.TimeManager.RunFunctionInfinitely(EventNames.IncreaseGameDifficulty,null,ChangeDifficultyInterval);
        }
    }
}