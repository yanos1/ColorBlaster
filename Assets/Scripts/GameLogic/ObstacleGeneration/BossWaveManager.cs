using System;
using System.Collections.Generic;
using System.Linq;
using Core.Managers;
using Extentions;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic.ObstacleGeneration
{
    public class BossWaveManager : MonoBehaviour
    {
        private float _currentBossLevelDifficulty;
        private int obstacleAddtitionAmount = 1;
        private Dictionary<ObstacleType, (int, List<Obstacle>)> _bossObstacleMap;
        private List<List<Obstacle>> _bossLevels;

        private int _nextBossLevelIndex;

        private void Awake()
        {
            _bossLevels = new List<List<Obstacle>>();
            _bossObstacleMap = CoreManager.instance.ObstacleManager.GetBossObstacleMap();
            _currentBossLevelDifficulty = CoreManager.instance.ControlPanelManager.minBossLevelDifficulty;
            SpawnBossLevelsForSession(null);
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.SessionUp, SpawnBossLevelsForSession);
        }
        
        private void OnDisable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.SessionUp, SpawnBossLevelsForSession);
        }


        private void SpawnBossLevelsForSession(object obj)
        {
            _bossLevels.Clear();
            for (int i = 0; i < CoreManager.instance.ControlPanelManager.levelSpeeds.Length; ++i)
            {
                AddBossWave();
                AdjustObstacleDifficultyAndIncreaseBossLevel();
            }

            
        }

        private void AdjustObstacleDifficultyAndIncreaseBossLevel()
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


        private void AddBossWave()
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

                print(currentPoints);
                int amount = (int)currentPoints % currentObstacle.Difficulty;
                for (int i = 0; i < amount; ++i)
                {
                    obstaclesToAdd.Add(currentObstacle);
                }

                currentPoints -= currentObstacle.Difficulty * amount;


                if (--maxIndex <= 0)
                {
                    break;
                }
            }

            obstaclesToAdd.Reverse();
            _bossLevels.Add(obstaclesToAdd);
        }

        // public int GetTotalBossObstaclesInSession()  //fix and give seession agument
        // {
        //     int numsLevels = CoreManager.instance.ControlPanelManager.levelSpeeds.Length;
        //     ObstacleType chosenBoss = UtilityFunctions.GetRandomKey(_bossObstacleMap);
        //     int maxIndex = _bossObstacleMap[chosenBoss].Item1;
        //     List<Obstacle> obstacles = _bossObstacleMap[chosenBoss].Item2;
        //
        //     int total = 0;
        //     for (int i = 0; i < numsLevels; ++i)
        //     {
        //         int curIndex = maxIndex;
        //         Obstacle currentObstacle = obstacles[curIndex - 1];
        //
        //
        //         float currentPoints = _currentBossLevelDifficulty +
        //                               i * CoreManager.instance.ControlPanelManager.bossLevelDifficultyIncreasePerLevel;
        //
        //         while (currentPoints > 0)
        //         {
        //             int amount = (int)currentPoints % currentObstacle.Difficulty;
        //             currentObstacle = obstacles[curIndex - 1];
        //
        //             total += amount;
        //
        //             currentPoints -= currentObstacle.Difficulty * amount;
        //
        //             if (--curIndex <= 0)
        //             {
        //                 break;
        //             }
        //         }
        //
        //         if (_bossObstacleMap[chosenBoss].Item1 < obstacles.Count)
        //         {
        //             maxIndex++;
        //         }
        //     }
        //
        //     return total;
        // }

        public int GetNumberOfBossObstaclesForSession()
        {
            print($"boss wages generated : {_bossLevels.Count}");
            return _bossLevels.Sum(sub => sub.Count);
        }

        public List<Obstacle> GetNextBossLevel()
        {
            return _bossLevels[_nextBossLevelIndex++%CoreManager.instance.ControlPanelManager.levelSpeeds.Length];
        }
    }
}