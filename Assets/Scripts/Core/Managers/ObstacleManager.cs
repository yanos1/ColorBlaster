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
        public float BaseSpeed
        {
            get => baseSpeed;
            set => baseSpeed = Mathf.Max(0, value); // Ensures speed is not set to a negative value
        }

        private Obstacle[] _obstacleData;

        private Dictionary<int, ValueTuple<int, List<Obstacle>>> weightToObstacleMap = new();
        private float baseSpeed;

        public ObstacleManager(Obstacle[] obstacleData, float obstaclesBaseSpeed)
        {
            _obstacleData = obstacleData;
            baseSpeed = obstaclesBaseSpeed;

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
            if (weightToObstacleMap.ContainsKey(1))
            {
                weightToObstacleMap[1] = (weightToObstacleMap[1].Item2.Count, weightToObstacleMap[1].Item2);
                weightToObstacleMap[2] = (weightToObstacleMap[2].Item2.Count, weightToObstacleMap[2].Item2);  // test
                weightToObstacleMap[3] = (weightToObstacleMap[3].Item2.Count, weightToObstacleMap[3].Item2);  // test
            }
        }
    }
}
