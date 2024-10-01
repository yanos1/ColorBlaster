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

        private Dictionary<int, ValueTuple<int, List<Obstacle>>> weightToObstacleMap = new();

        public ObstacleManager(Obstacle[] obstacleData)
        {
            _obstacleData = obstacleData;

            // Manually handle event subscriptions

            InitializeMap();
        }


        public Dictionary<int, ValueTuple<int, List<Obstacle>>> GetParsedObstacleData()
        {
            return weightToObstacleMap;
        }

        private void InitializeMap()
        {
            foreach (var obs in _obstacleData)
            {
                if (!weightToObstacleMap.ContainsKey(obs.Difficulty))
                {
                    weightToObstacleMap[obs.Difficulty] = (0, new List<Obstacle>());
                }

                weightToObstacleMap[obs.Difficulty].Item2.Add(obs);
            }

            // Make all the obstacles in difficulty 1 available initially
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
    }
}