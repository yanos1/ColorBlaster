using System;
using System.Collections.Generic;
using System.Linq;
using Core.Managers;
using Extentions;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class BossWaveManager : MonoBehaviour
    {
        private float _currentBossLevelDifficulty;
        private int obstacleAddtitionAmount = 1;
        private Dictionary<ObstacleType, (int, List<Obstacle>)> _bossObstacleMap;

        private void Start()
        {
            _bossObstacleMap = CoreManager.instance.ObstacleManager.GetBossObstacleMap();
            _currentBossLevelDifficulty = CoreManager.instance.ControlPanelManager.minBossLevelDifficulty;
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.LevelUp,
                AdjustObstacleDifficultyAndIncreaseBossLevel);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.LevelUp,
                AdjustObstacleDifficultyAndIncreaseBossLevel);
        }

        private void AdjustObstacleDifficultyAndIncreaseBossLevel(object obj)
        {
            foreach (var bossKey in _bossObstacleMap.Keys.ToList())
            {
                var obstacleData = _bossObstacleMap[bossKey];
                _bossObstacleMap[bossKey] = UpdateObstacleMaxIndex(obstacleData, obstacleAddtitionAmount);
            }


            _currentBossLevelDifficulty = Mathf.Min(_currentBossLevelDifficulty + CoreManager.instance
                    .ControlPanelManager
                    .bossLevelDifficultyIncreasePerLevel,
                CoreManager.instance.ControlPanelManager.maxBossLevelDifficulty);
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
            if (chosenBoss == default) return;

            List<Obstacle> obstacles = _bossObstacleMap[chosenBoss].Item2;
            List<Obstacle> obstaclesToAdd = new();
            int maxIndex = _bossObstacleMap[chosenBoss].Item1;

            // print($"max index: {maxIndex}  obstacles length: {obstacles.Count}");
            float currentPoints = _currentBossLevelDifficulty;
            print($"current boss diff : {_currentBossLevelDifficulty}");

            while (currentPoints > 0)
            {
                print(currentPoints);
                Obstacle currentObstacle = obstacles[maxIndex - 1];
                while (currentObstacle.Difficulty <= currentPoints)
                {
                    print(currentPoints);
                    obstaclesToAdd.Add(currentObstacle);
                    currentPoints -= currentObstacle.Difficulty;
                }

                if (--maxIndex <= 0)
                {
                    break;
                }
            }

            obstaclesToAdd.Reverse();
            print($"boss level count : {obstaclesToAdd.Count}");
            newBatch.AddRange(obstaclesToAdd);
        }
    }
}