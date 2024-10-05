using System;
using System.Collections.Generic;
using GameLogic.ObstacleGeneration;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Managers
{
    public class ObstacleManager
    {
        private Obstacle[] _obstacleData;
        private Obstacle[] _bossObstacleData;

        private Dictionary<int, ValueTuple<int, List<Obstacle>>> weightToObstacleMap = new();
        private Dictionary<ObstacleType, ValueTuple<int, List<Obstacle>>> bossObstaclesMap = new();

        private int _minNumOfAvailableObstacles;

        public ObstacleManager(Obstacle[] obstacleData, Obstacle[] bossObstacleData)
        {
            _obstacleData = obstacleData;
            _bossObstacleData = bossObstacleData;
            _minNumOfAvailableObstacles = 2;
            Debug.Log($"boss obstacles amount : {_bossObstacleData.Length}");
        // Manually handle event subscriptions
        }

        public ObstacleManager(int minNumOfAvailableObstacles)
        {
            _minNumOfAvailableObstacles = minNumOfAvailableObstacles;
        }


        public Dictionary<int, ValueTuple<int, List<Obstacle>>> GetBaseObstacleMap()
        {
            InitializeBaseObstacleMap();
            // SetInitialNumberOfObstaclesPerDifficulty();
            foreach (var kvp in weightToObstacleMap)
            {
                foreach (var obs in kvp.Value.Item2)
                {
                    Debug.Log("aaa " + obs.name);
                }
            }
            return weightToObstacleMap;
        }

        public Dictionary<ObstacleType, ValueTuple<int, List<Obstacle>>> GetBossObstacleMap()
        {
            InitializeBossObstacleMap();
            return bossObstaclesMap;
        }

        private void InitializeBossObstacleMap()
        {
            Debug.Log($"boss obstacles amount : {_bossObstacleData.Length}");
            foreach (var obs in _bossObstacleData)
            {
                if (SkipObstacleType(obs.ObstacleType))
                {
                    Debug.Log($"SKIPPED {obs.ObstacleType}");
                    continue;
                }

                if (!bossObstaclesMap.ContainsKey(obs.ObstacleType))
                {
                    bossObstaclesMap[obs.ObstacleType] = (_minNumOfAvailableObstacles, new List<Obstacle>());
                }
                Debug.Log($"added {obs.ObstacleType}");
                bossObstaclesMap[obs.ObstacleType].Item2.Add(obs);
            }

            // Sort each list by Difficulty (ascending)
            foreach (var kvp in bossObstaclesMap)
            {
                kvp.Value.Item2.Sort((a, b) => a.Difficulty.CompareTo(b.Difficulty)); // Sort by obs.Difficulty
            }

            foreach (var kvp in bossObstaclesMap) // error check
            {
                if (kvp.Value.Item1 > kvp.Value.Item2.Count)
                {
                    Debug.Log(
                        $"Mismatch between min number of obstacles {_minNumOfAvailableObstacles} and actual" +
                        $" obstacle count {kvp.Value.Item2.Count} in obstacle type: {kvp.Key}");
                }
            }
        }


        private void InitializeBaseObstacleMap()
        {
            foreach (var obs in _obstacleData)
            {
                Debug.Log("bbb" + obs.name);
                if (SkipObstacleType(obs.ObstacleType))
                {
                    Debug.Log($"SKIPPED {obs.ObstacleType}");
                    continue;
                }

                if (!weightToObstacleMap.ContainsKey(obs.Difficulty))
                {
                    weightToObstacleMap[obs.Difficulty] = (_minNumOfAvailableObstacles, new List<Obstacle>());
                }


                weightToObstacleMap[obs.Difficulty].Item2.Add(obs);
            }

            // Make all the obstacles in difficulty 1 available initially
        }

        private void SetInitialNumberOfObstaclesPerDifficulty()
        {
            for (int i = 0; i < weightToObstacleMap.Count; ++i)
            {
                if (weightToObstacleMap.ContainsKey(i))
                {
                    weightToObstacleMap[i] = (weightToObstacleMap[i].Item2.Count, weightToObstacleMap[i].Item2);
                }
            }

            if (weightToObstacleMap.ContainsKey(0))
            {
                weightToObstacleMap[0] = (weightToObstacleMap[0].Item2.Count, weightToObstacleMap[0].Item2);
            }

            if (weightToObstacleMap.ContainsKey(1))
            {
                weightToObstacleMap[1] = (weightToObstacleMap[1].Item2.Count, weightToObstacleMap[1].Item2);
            }

            if (weightToObstacleMap.ContainsKey(2) && weightToObstacleMap.ContainsKey(3))
            {
                weightToObstacleMap[2] = (weightToObstacleMap[2].Item2.Count, weightToObstacleMap[2].Item2);
                weightToObstacleMap[3] = (weightToObstacleMap[3].Item2.Count, weightToObstacleMap[3].Item2);
            }
        }

        private static bool SkipObstacleType(ObstacleType type)
        {
            Debug.Log("TRYING TO SKIP");
            Debug.Log(
                $"type : {type}  1. {CoreManager.instance.ControlPanelManager.canSpawnChasingObstacles} {CoreManager.instance.ControlPanelManager.canSpawnRotatingObstacles}");
            bool skip = ((type == ObstacleType.Chasing &&
                          !CoreManager.instance.ControlPanelManager.canSpawnChasingObstacles) ||
                         (type == ObstacleType.Rotating &&
                          !CoreManager.instance.ControlPanelManager.canSpawnRotatingObstacles) ||
                         (type == ObstacleType.Offsetting &&
                          !CoreManager.instance.ControlPanelManager.canSpawnOffsettingObstacles) ||
                         (type == ObstacleType.Moving &&
                          !CoreManager.instance.ControlPanelManager.canSpawnMovingObstacles) ||
                         type == ObstacleType.Rocket &&
                         !CoreManager.instance.ControlPanelManager.canSpawnRockets);
            Debug.Log(skip);
            Debug.Log("---------");
            return skip;
        }
    }
}