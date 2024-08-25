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
        [SerializeField] private ObstacleGenerator obstacleGenerator;
        
        private  Dictionary<int, ValueTuple<int , List<Obstacle>>> weightToObstacleMap = new();
        private float baseSpeed;


        
        private void Awake()
        {
            InitializeMap();
            obstacleGenerator.Init(weightToObstacleMap);

        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.ChangeStyle, ChangeObstacleStyle);
        }
        
        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.ChangeStyle, ChangeObstacleStyle);
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
                weightToObstacleMap.GetValueOrDefault(obs.Difficulty,(0, new List<Obstacle>())).Item2.Add(obs);
            }
            
            // this line makes all the obstacles in difficulty 1 available, the rest of ddiculties arent available at start
            weightToObstacleMap[1] = (weightToObstacleMap[1].Item2.Count, weightToObstacleMap[1].Item2);
        }
        }
    }
