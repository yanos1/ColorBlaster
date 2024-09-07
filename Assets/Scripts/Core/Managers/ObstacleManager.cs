using System;
using System.Collections.Generic;
using Core.ObstacleGeneration;
using ObstacleGeneration;
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
            CoreManager.instance.EventManager.AddListener(EventNames.SetStyle, ChangeObstacleStyle);
            
            InitializeMap();
        }

        public void OnDestroy()
        {
            // Clean up event subscriptions
            CoreManager.instance.EventManager.RemoveListener(EventNames.SetStyle, ChangeObstacleStyle);
        }

        public Dictionary<int, ValueTuple<int, List<Obstacle>>> GetParsedObstacleData()
        {
            return weightToObstacleMap;
        }

        private void ChangeObstacleStyle(object obj)
        {
            foreach (var obstacle in _obstacleData)
            {
                obstacle.ApplyStyle();
            }
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
            for (int i=1; i < weightToObstacleMap.Count; ++i)
            {
                if (weightToObstacleMap.ContainsKey(i))
                {
                    weightToObstacleMap[i] = (weightToObstacleMap[i].Item2.Count, weightToObstacleMap[i].Item2);
                }
            }
          
        }
    }
}
