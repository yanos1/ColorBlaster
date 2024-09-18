using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Extentions;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class ObstacleGenerator : MonoBehaviour
    {
        [SerializeField] private ObstacleGeneratorHandler _generatorHandler;
        [SerializeField] private float generationThreshold = 0f;
        [SerializeField] private float returnToPoolThreshold = -11f;

        private List<Obstacle> activeObstacles;
        private Dictionary<int, ValueTuple<int, List<Obstacle>>> obstacleData;
        private Coroutine colorRushCoroutine;
        private Obstacle currentObstacle;
        private float maxTimeBetweenObstacles = 6f;
        private float lastTimeGenerated;
        private bool isColorRushActive = false;
        private Color colorRushColor;


        // Update is called once per frame
        private void Start()
        {
            activeObstacles = new List<Obstacle>();
            obstacleData = CoreManager.instance.ObstacleManager.GetParsedObstacleData();
            _generatorHandler.Init(obstacleData);
            currentObstacle = GenerateObstacle();
            StartCoroutine(ActiveObstaclesUpdate());
            DontDestroyOnLoad(this.gameObject);
        }
        
        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, PauseObstacles);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, ResumeObstacles);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateColorRush, PaintActiveObstacles);
        
        }

   

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, PauseObstacles);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, ResumeObstacles);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateColorRush, PaintActiveObstacles);
        }

        private void PaintActiveObstacles(object obj)
        {
            print("event called IndED");
            if (obj is ValueTuple<Color,float> pair)
            {
                print(this); // will be null if i play again and dont put this object on dont destroy on load.
                
                this.StopAndStartCoroutine(ref colorRushCoroutine,PaintActiveObstaclesForDuration(pair.Item1,pair.Item2));
            }
        }

        private IEnumerator PaintActiveObstaclesForDuration(Color color, float duration)
        {
            isColorRushActive = true;
            foreach (var obstacle in activeObstacles)
            {
                print("obstace");
                foreach (var part in obstacle.ExtractStyleableObjects())
                {
                    print("paining part");
                    part.Renderer.color = color;
                    colorRushColor = color;
                }
            }

            yield return new WaitForSeconds(duration);
            isColorRushActive = false;

        }

        private void PauseObstacles(object obj)
        {
            StopAllCoroutines();   // this looks like bad practice !! better to save the coroutine, watch out !
        }

        private void ResumeObstacles(object obj)
        {
            StartCoroutine(ActiveObstaclesUpdate());

        }


        private IEnumerator ActiveObstaclesUpdate()
        {
            while (true)
            {
                if (Time.time > lastTimeGenerated + maxTimeBetweenObstacles)
                {
                    // CoreManager.instance.PoolManager.ReturnToPool(currentObstacle.PoolType, currentObstacle.gameObject);
                    currentObstacle = GenerateObstacle();
                }
                
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
            lastTimeGenerated = Time.time;
            currentObstacle = _generatorHandler.GetNextObstacle();
            if (isColorRushActive)
            {
                foreach (var part in currentObstacle.ExtractStyleableObjects())
                {
                    part.Renderer.color = colorRushColor;
                }
            }

            currentObstacle.transform.position = transform.position;
            activeObstacles.Add(currentObstacle);
            return currentObstacle;
        }
    }
}