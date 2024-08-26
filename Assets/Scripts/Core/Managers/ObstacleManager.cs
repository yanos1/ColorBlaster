using System;
using System.Collections.Generic;
using Core.ObstacleGeneration;
using ObstacleGeneration;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Managers
{
    public class ObstacleManager : MonoBehaviour
    {
        public float BaseSpeed
        {
            get => baseSpeed;
            set => baseSpeed = Mathf.Max(0, value); // Ensures speed is not set to a negative value
        }

        [SerializeField] private Obstacle[] obstacleData;

        private Dictionary<int, ValueTuple<int, List<Obstacle>>> weightToObstacleMap = new();
        [SerializeField] private float baseSpeed;


        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.SetStyle, ChangeObstacleStyle);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.SetStyle, ChangeObstacleStyle);
        }

        public Dictionary<int, ValueTuple<int, List<Obstacle>>> GetParsedObstacleData()
        {
            InitializeMap();
            return weightToObstacleMap;
        }

        private void ChangeObstacleStyle(object obj)
        {
            foreach (var obstacle in obstacleData)
            {
                obstacle.ChangeStyle();
            }
        }

        public float GetBaseSpeed()
        {
            return baseSpeed;
        }

        private void InitializeMap()
        {
            foreach (var obs in obstacleData)
            {
                // Check if the dictionary contains the key
                if (!weightToObstacleMap.ContainsKey(obs.Difficulty))
                {
                    // If not, add a new entry with the default weight and a new list of obstacles
                    weightToObstacleMap[obs.Difficulty] = (0, new List<Obstacle>());
                }

                // Add the obstacle to the appropriate list
                weightToObstacleMap[obs.Difficulty].Item2.Add(obs);

                // Print the current state of the dictionary
                foreach (var kvp in weightToObstacleMap)
                {
                    Debug.Log($"Key: {kvp.Key}, Value: (Weight: {kvp.Value.Item1}, Obstacles: {kvp.Value.Item2.Count})");
                }
                print("----------");
            }

           
            // this line makes all the obstacles in difficulty 1 available, the rest of ddiculties arent available at start
            weightToObstacleMap[1] = (weightToObstacleMap[1].Item2.Count, weightToObstacleMap[1].Item2);
        }
    }
}