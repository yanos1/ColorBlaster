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
            return weightToObstacleMap;
        }

        public Dictionary<ObstacleType, ValueTuple<int, List<Obstacle>>> GetBossObstacleMap()
        {
            InitializeBossObstacleMap();
            return bossObstaclesMap;
        }

        private void InitializeBossObstacleMap()
        {
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

                bossObstaclesMap[obs.ObstacleType].Item2.Add(obs);
            }

            foreach (var kvp in bossObstaclesMap) // error check
            {
                if (kvp.Value.Item1 > kvp.Value.Item2.Count)
                {
                    Debug.Log(
                        $"Missmatch between min number of obstacles {_minNumOfAvailableObstacles} and actaul" +
                        $" obstacle count  {kvp.Value.Item2.Count} in obstacle type : {kvp.Key}");
                }
            }
        }

        private void InitializeBaseObstacleMap()
        {
            foreach (var obs in _obstacleData)
            {
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
            bool skip = (type == ObstacleType.Chasing &&
                         !CoreManager.instance.ControlPanelManager.canSpawnChasingObstacles) ||
                        type == ObstacleType.Rotating &&
                        !CoreManager.instance.ControlPanelManager.canSpawnRotatingObstacles;
            Debug.Log(skip);
            Debug.Log("---------");
            return skip;
        }
    }
}