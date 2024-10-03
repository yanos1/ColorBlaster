using System;
using System.Collections.Generic;
using System.Linq;
using Core.Managers;
using Extentions;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class BossWaveManager : MonoBehaviour
    {
        private float _currentBossLevelDifficulty;
        private Dictionary<ObstacleType, (int, List<Obstacle>)> _bossObstacleMap;

        private void Start()
        {
            _bossObstacleMap = CoreManager.instance.ObstacleManager.GetBossObstacleMap();
            _currentBossLevelDifficulty = CoreManager.instance.ControlPanelManager.minBossLevelDifficulty;
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.LevelUp, AdjustObstacleDifficultyAndIncreaseBossLevel);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.LevelUp, AdjustObstacleDifficultyAndIncreaseBossLevel);
        }

        private void AdjustObstacleDifficultyAndIncreaseBossLevel(object obj)
        {
            if (obj is int amount)
            {
                foreach (var bossKey in _bossObstacleMap.Keys.ToList())
                {
                    var obstacleData = _bossObstacleMap[bossKey];
                    _bossObstacleMap[bossKey] = UpdateObstacleMaxIndex(obstacleData, amount);
                }
            }

            _currentBossLevelDifficulty += CoreManager.instance.ControlPanelManager.bossLevelDifficultyIncreasePerLevel;
        }

        private (int, List<Obstacle>) UpdateObstacleMaxIndex(
            (int currentMaxIndex, List<Obstacle> obstacleList) obstacleData, int amount)
        {
            var (currentMaxIndex, obstacleList) = obstacleData;
            int newMaxIndex = Mathf.Min(currentMaxIndex + amount, obstacleList.Count);
            return (newMaxIndex, obstacleList);
        }


        public void AddBossWave(List<Obstacle> newBatch)
        {
            ObstacleType chosenBoss = UtilityFunctions.GetRandomKey(_bossObstacleMap);
            List<Obstacle> obstacles = _bossObstacleMap[chosenBoss].Item2;
            List<Obstacle> obstaclesToAdd = new();
            int maxIndex = _bossObstacleMap[chosenBoss].Item1;
            float currentPoints = _currentBossLevelDifficulty;
            
            while (currentPoints > 0)
            {
                Obstacle currentObstacle = obstacles[maxIndex - 1];
                while (currentObstacle.Difficulty <= currentPoints)
                {
                    obstaclesToAdd.Add(currentObstacle);
                    currentPoints -= currentObstacle.Difficulty;
                }

                maxIndex -= 1;
            }
            obstaclesToAdd.Reverse();
            newBatch.AddRange(obstaclesToAdd);

        }
    }
}