using System;
using System.Collections.Generic;
using Core.Managers;
using Extentions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.ObstacleGeneration
{
    public class ObstacleGeneratorHandler : MonoBehaviour
    {

        private Dictionary<int, ValueTuple<int, List<Obstacle>>> _difficultyToObstacleMap;
        private Obstacle[] _currentBatch;
        private int numObstaclesPerLevel;

        public void Init(Dictionary<int, ValueTuple<int, List<Obstacle>>> difficultyToObstacleMap)
        {
            numObstaclesPerLevel = CoreManager.instance.ControlPanelManager.obstaclesPerLevel;

            _difficultyToObstacleMap = difficultyToObstacleMap;
            foreach (var kvp in difficultyToObstacleMap)
            {
                print(kvp.Key);
                foreach (var obs in kvp.Value.Item2)
                {
                    print(obs.name);
                }
                print("----");
            }
            _currentBatch = InitNewObstacleBatch();
            CoreManager.instance.ControlPanelManager.PrintParametersAtEndOfSession();
        }
        
        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.LevelUp, AdjustObstacleDifficulty);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.LevelUp, AdjustObstacleDifficulty);
        }

        private Obstacle[] InitNewObstacleBatch()
        {
            // int numTreasureObstacles = Random.Range(2,4);
            var newBatch = new Obstacle[numObstaclesPerLevel];
            // Fill with regular obstacles
            for (int i = 0; i < numObstaclesPerLevel; ++i)
            {
                newBatch[i] = GenerateRandomObstacle();
            }

            // Add treasure obstacles from difficulty 0
            // for (int i = numObstaclesPerLevel; i < numTreasureObstacles+ numObstaclesPerLevel; i++)
            // {
            //     newBatch[i] = GenerateTreasureObstacle();
            // }
            foreach (var obs in newBatch)
            {
                print(obs.name);
            }

            return UtilityFunctions.ShuffleArray(newBatch);
        }

        private Obstacle GenerateRandomObstacle()
        {
            int randomNumber = Random.Range(0, 101);
            print(randomNumber); ;
            int difficulty = GetRandomObstacleDifficulty(randomNumber);
            print(difficulty);
            

            var (maxIndex, obstacles) = _difficultyToObstacleMap[difficulty];
            return obstacles[Random.Range(0, maxIndex)];
        }

        private Obstacle GenerateTreasureObstacle()
        {
            var (maxIndex, treasureObstacles) = _difficultyToObstacleMap[0];
            return treasureObstacles[Random.Range(0, maxIndex)];
        }

        private int GetRandomObstacleDifficulty(int randomNumber)
        {
            int level = CoreManager.instance.ControlPanelManager.Level;
            int[] levelDifficulties = CoreManager.instance.ControlPanelManager.obstacleToDifficultyPerLevel[level];
            foreach (var d in levelDifficulties)
            {
                print(d);
            }
            print("-------");
            float currentNumber = randomNumber;
            for(int i=0;i<levelDifficulties.Length; ++i)
            {
                if (levelDifficulties[i] >= currentNumber)
                {
                    // print($"chance: {chance} number: {currentNumber}  {difficulty}");
                    return i+1;
                }
                currentNumber -= levelDifficulties[i];
            }
            // print($"{currentNumber}  {0}");
            return 1; // Fallback to easiest difficulty if nothing matches
        }


  

        public Obstacle GetNextObstacle()
        {
            // Check if the current batch is exhausted, generate a new one if necessary
            var nextObstacle = _currentBatch[CoreManager.instance.GameManager.ObstacleCrossedThisLevel];

            print($"obstacled crossed so far {CoreManager.instance.GameManager.ObstacleCrossedThisLevel} needed to pass level: {numObstaclesPerLevel}");
            if (++CoreManager.instance.GameManager.ObstacleCrossedThisLevel >= numObstaclesPerLevel)
            {
                _currentBatch = InitNewObstacleBatch();
            }

            return ResetObstacle(nextObstacle);
        }

        private void AdjustObstacleDifficulty(object obj)
        {
            if (obj is int amount)
            {
                for (int i = 1; i < 3; ++i)
                {
                    var (currentMaxIndex, obstacleList) = _difficultyToObstacleMap[i];
                    int newMaxIndex = Mathf.Min(currentMaxIndex + amount, obstacleList.Count);
                    _difficultyToObstacleMap[i] = (newMaxIndex, obstacleList);
                }
            }
        }

        private static Obstacle ResetObstacle(Obstacle obstacle)
        {
            GameObject obstaclePrefab = CoreManager.instance.PoolManager.GetFromPool(obstacle.PoolType);
            Obstacle newObstacle = obstaclePrefab.GetComponent<Obstacle>();
            newObstacle.ChangeColors();
            newObstacle.ResetGameObject();
            return newObstacle;
        }

        // private void AdjustWeights()
        // {
        //
        //     float aAdjusted = Mathf.Max(_difficultyToChanceMap[0].second - 10, 0);
        //     float bAdjusted = Mathf.Min(_difficultyToChanceMap[1].second + 6, 90f);
        //     float cAdjusted = Mathf.Min(_difficultyToChanceMap[2].second + 4, 50f);
        //
        //     float totalAdjusted = aAdjusted + bAdjusted + cAdjusted;
        //
        //     // Normalize chances to ensure they sum to 100
        //     float scale = 100f / totalAdjusted;
        //     _difficultyToChanceMap[0].second = aAdjusted * scale;
        //     _difficultyToChanceMap[1].second = bAdjusted * scale;
        //     _difficultyToChanceMap[2].second = cAdjusted * scale;
        // }
    }
}
