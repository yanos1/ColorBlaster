using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using ObstacleGeneration;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.ObstacleGeneration
{
    public class ObstacleGenerator : MonoBehaviour
    {
        [SerializeField] private ObstacleGeneratorHandler _generatorHandler;
        [SerializeField] private float generationThreshold = 0f;
        [SerializeField] private float returnToPoolThreshold = -14f;

        private List<Obstacle> activeObstacles;
        private Dictionary<int, ValueTuple<int, List<Obstacle>>> obstacleData;
        private Obstacle currentObstacle;


        // Update is called once per frame
        private void Start()
        {
            activeObstacles = new List<Obstacle>();
            obstacleData = CoreManager.instance.ObstacleManager.GetParsedObstacleData();
            _generatorHandler.Init(obstacleData);
            currentObstacle = GenerateObstacle();
            StartCoroutine(ActiveObstaclesUpdate());
        }

        private IEnumerator ActiveObstaclesUpdate()
        {
            while (true)
            {
                for (int i = activeObstacles.Count - 1; i >= 0; i--)
                {
                    Obstacle obstacle = activeObstacles[i];

                    // Check if the obstacle crossed the generation threshold
                    if (obstacle == currentObstacle && obstacle.RightMostPosition.x < generationThreshold)
                    {
                        currentObstacle = GenerateObstacle();
                    }

                    // Check if the obstacle crossed the return-to-pool threshold
                    if (obstacle.RightMostPosition.x < returnToPoolThreshold)
                    {
                        CoreManager.instance.PoolManager.ReturnToPool(obstacle.PoolType, obstacle.gameObject);
                        activeObstacles.RemoveAt(i); // Remove it from the active list
                    }
                }

                yield return new WaitForSeconds(0.2f); // we check for updates every 0.2 seconds
            }
        }

        private Obstacle GenerateObstacle()
        {
            currentObstacle = _generatorHandler.GetRandomObstacle();
            currentObstacle.transform.position = transform.position;
            activeObstacles.Add(currentObstacle);
            return currentObstacle;
        }
    }
}