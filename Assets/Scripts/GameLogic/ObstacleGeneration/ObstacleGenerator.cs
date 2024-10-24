using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Extentions;
using GameLogic.Boosters;
using GameLogic.ConsumablesGeneration;
using GameLogic.StyleRelated;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class ObstacleGenerator : MonoBehaviour
    {
        [SerializeField] private ObstacleGeneratorHandler _generatorHandler;
        private float generationThreshold;
        private float returnToPoolThreshold = -5f;

        private List<Obstacle> activeObstacles;
        private Dictionary<int, ValueTuple<int, List<Obstacle>>> obstacleData;
        private Dictionary<ObstacleType, ValueTuple<int, List<Obstacle>>> bossObstacleData;
        private Coroutine colorRushCoroutine;
        private Obstacle _currentObstacle;
        private float maxTimeBetweenRockets = 4.9f;
        private float maxTimeBetweenObstacles = 6f;
        private float lastTimeGenerated;
        private bool isColorRushActive = false;
        private Color colorRushColor;


        // Update is called once per frame
        private void Start()
        {
            generationThreshold = MapDistance(CoreManager.instance.ControlPanelManager.distanceBetweenObstacles);
            activeObstacles = new List<Obstacle>();
            obstacleData = CoreManager.instance.ObstacleManager.GetBaseObstacleMap();
            _generatorHandler.Init(obstacleData);
            _currentObstacle = GenerateObstacle();
            StartCoroutine(ActiveObstaclesUpdate());
            // DontDestroyOnLoad(this.gameObject);
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, PauseObstacles);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, ResumeObstacles);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateColorRush, PaintActiveObstacles);
            CoreManager.instance.EventManager.AddListener(EventNames.DeactivateColorRush, RestoreActiveObstacles);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateDeleteColor, DisableDeletedColor);
        }


        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, PauseObstacles);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, ResumeObstacles);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateColorRush, PaintActiveObstacles);
            CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateColorRush, RestoreActiveObstacles);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateDeleteColor, DisableDeletedColor);
        }

        private void DisableDeletedColor(object obj)
        {
            if (obj is (Color color, float duration, BoosterButtonController buff))
            {
                foreach (var obstacle in activeObstacles)
                {
                    List<StyleableObject> obstacleParts = obstacle.ExtractObstacleParts();
                    foreach (var part in obstacleParts)
                    {
                        if (UtilityFunctions.CompareColors(part.GetColor(), color))
                        {
                            part.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        private void RestoreActiveObstacles(object obj)
        {
            // later add restoring obstacles if we want.
            isColorRushActive = false;
        }

        private void PaintActiveObstacles(object obj)
        {
            if (obj is (Color color, float duration, BoosterButtonController buff))
            {
                isColorRushActive = true;
                foreach (var obstacle in activeObstacles)
                {
                    foreach (var part in obstacle.ExtractObstacleParts())
                    {
                        part.ChangeColor(color);
                        colorRushColor = color;
                    }
                }
            }
        }

        private void PauseObstacles(object obj)
        {
            activeObstacles.Clear();
            StopAllCoroutines(); // this looks like bad practice !! better to save the coroutine, watch out !
        }

        private void ResumeObstacles(object obj)
        {
            StartCoroutine(ActiveObstaclesUpdate());
            _currentObstacle = GenerateObstacle();
        }


        private IEnumerator ActiveObstaclesUpdate()
        {
            while (CoreManager.instance.GameManager.IsGameActive)
            {
                // print($"Time  : {Time.time} time for new obstacle : {lastTimeGenerated + maxTimeBetweenRockets}");
                // if (_currentObstacle.ObstacleType == ObstacleType.Rocket && Time.time > lastTimeGenerated + maxTimeBetweenRockets)
                // {
                //     CoreManager.instance.PoolManager.ReturnToPool(_currentObstacle.PoolType, _currentObstacle.gameObject);
                //     activeObstacles.Remove(_currentObstacle);
                //     _currentObstacle = GenerateObstacle();
                //     yield return new WaitForSeconds(0.2f); // we check for updates every 0.2 seconds
                // }
                // else if (Time.time > lastTimeGenerated + maxTimeBetweenObstacles)
                // {
                //     _currentObstacle = GenerateObstacle();
                //     yield return new WaitForSeconds(0.2f); // we check for updates every 0.2 seconds
                //
                //
                // }

                for (int i = activeObstacles.Count - 1; i >= 0; i--)
                {
                    Obstacle obstacle = activeObstacles[i];

                    // Check if the obstacle crossed the generation threshold
                    print("44 " + obstacle.RightMostPosition + " " + generationThreshold);
                    if (obstacle == _currentObstacle && obstacle.RightMostPosition.y < generationThreshold)
                    {
                        
                        _currentObstacle = GenerateObstacle();
                    }

                    // Check if the obstacle crossed the return-to-pool threshold
                    if (obstacle.RightMostPosition.y < returnToPoolThreshold)
                    {
                        CoreManager.instance.PoolManager.ReturnToPool(obstacle.PoolType, obstacle.gameObject);
                        activeObstacles.RemoveAt(i); // Remove it from the active list
                        if (!obstacle.Crossed)
                        {
                            CoreManager.instance.EventManager.InvokeEvent(EventNames.ObstacleCrossed, null);
                        }
                    }
                }


                yield return new WaitForSeconds(0.2f); // we check for updates every 0.2 seconds
            }
        }

        private Obstacle GenerateObstacle()
        {
            lastTimeGenerated = Time.time;
            _currentObstacle = _generatorHandler.GetNextObstacle();
            if (isColorRushActive)
            {
                foreach (var part in _currentObstacle.ExtractObstacleParts())
                {
                    part.ChangeColor(colorRushColor);
                }
            }

            _currentObstacle.transform.position = transform.position;
            activeObstacles.Add(_currentObstacle);
            return _currentObstacle;
        }

        private static float MapDistance(float x)
        {
            return 3f - 0.06f * x;
        }
    }
}