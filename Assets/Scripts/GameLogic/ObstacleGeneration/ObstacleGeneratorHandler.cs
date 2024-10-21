using System;
using System.Collections.Generic;
using System.Linq;
using Core.Managers;
using Extentions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.ObstacleGeneration
{
    public class ObstacleGeneratorHandler : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private BossWaveManager bossWaveManager;
        private Dictionary<int, ValueTuple<int, List<Obstacle>>> _difficultyToObstacleMap;
        // private Dictionary<ObstacleType, ValueTuple<int, List<Obstacle>>> _bossObstacleMap;
        private List<Obstacle>_currentBatch;
        private int numObstaclesPerLevel;
        private int obstacleAdditionAmount;
        private int generateTreasureAtLevel;

        public void Init(Dictionary<int, ValueTuple<int, List<Obstacle>>> difficultyToObstacleMap)
        {
            numObstaclesPerLevel = CoreManager.instance.ControlPanelManager.obstaclesPerLevel;
            obstacleAdditionAmount = 2;
            generateTreasureAtLevel = Random.Range(2, CoreManager.instance.ControlPanelManager.levelSpeeds.Length);

            _difficultyToObstacleMap = difficultyToObstacleMap;

            foreach (var kvp in difficultyToObstacleMap)
            {
                print(kvp.Key);
                foreach (var obs in kvp.Value.Item2)
                {
                    print(obs.name);
                }

            }

            _currentBatch = InitNewObstacleBatch();
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.LevelUp, AdjustObstacleDifficulty);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.LevelUp, AdjustObstacleDifficulty);
        }

        private List<Obstacle> InitNewObstacleBatch()
        {
            // int numTreasureObstacles = Random.Range(2,4);
            var newBatch = new List<Obstacle>();

            for (int i = 0; i < numObstaclesPerLevel; ++i)
            {
                newBatch.Add(GenerateRandomObstacle());
            }

            if (CoreManager.instance.ControlPanelManager.Level == generateTreasureAtLevel)
            {
                Obstacle treasure = GenerateTreasureObstacle();
                newBatch.Add(treasure);
            }
            UtilityFunctions.ShuffleArray(newBatch);

            if (CoreManager.instance.ControlPanelManager.spawnBossObstacleAtTheEndOfLevel)
            {
                // Increase the size of the array by 1
                newBatch.AddRange(bossWaveManager.GetNextBossLevel());

                // Add the boss obstacle to the last position
                // newBatch[^1] = GetBossObstacle();


                // inset boss wave here
            }


            // Add treasure obstacles from difficulty 0
            // for (int i = numObstaclesPerLevel; i < numTreasureObstacles+ numObstaclesPerLevel; i++)
            // {
            //     newBatch[i] = GenerateTreasureObstacle();
            // }

            return newBatch;
        }

        private Obstacle GenerateRandomObstacle()
        {
            int randomNumber = Random.Range(0, 101);
            print(randomNumber);
            
            int difficulty = GetRandomObstacleDifficulty(randomNumber);
            print(difficulty);


            var (maxIndex, obstacles) = _difficultyToObstacleMap[difficulty];
            return obstacles[Random.Range(0, maxIndex)];
        }

        private Obstacle GenerateTreasureObstacle()
        {
            var (maxIndex, treasureObstacles) = _difficultyToObstacleMap[0];
            return treasureObstacles[0];  // for test, this needs fixing
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
            for (int i = 0; i < levelDifficulties.Length; ++i)
            {
                if (levelDifficulties[i] >= currentNumber)
                {
                    // print($"chance: {chance} number: {currentNumber}  {difficulty}");
                    return i + 1;
                }

                currentNumber -= levelDifficulties[i];
            }

            // print($"{currentNumber}  {0}");
            return 1; // Fallback to easiest difficulty if nothing matches
        }


        public Obstacle GetNextObstacle()
        {
            // Check if the current batch is exhausted, generate a new one if necessary
            var nextObstacle = _currentBatch[levelManager.ObstacleCrossedThisLevel];

            print(
                $"obstacled crossed so far {levelManager.ObstacleCrossedThisLevel} needed to pass level: {_currentBatch.Count}");
            if (++levelManager.ObstacleCrossedThisLevel >= _currentBatch.Count)
            {
                CoreManager.instance.EventManager.InvokeEvent(
                    EventNames.LevelUp, null);
                _currentBatch = InitNewObstacleBatch();
            }

            return ResetObstacle(nextObstacle);
        }

        private void AdjustObstacleDifficulty(object obj)
        {

            // Update normal obstacle difficulty
            for (int difficulty = 1; difficulty <= 3; difficulty++)
            {
                var obstacleData = _difficultyToObstacleMap[difficulty];
                _difficultyToObstacleMap[difficulty] = UpdateObstacleMaxIndex(obstacleData, obstacleAdditionAmount);
            }

            // Update boss obstacle difficulty
          
        }


        // Helper function to update max index for obstacles
        private (int, List<Obstacle>) UpdateObstacleMaxIndex(
            (int currentMaxIndex, List<Obstacle> obstacleList) obstacleData, int amount)
        {
            var (currentMaxIndex, obstacleList) = obstacleData;
            int newMaxIndex = Mathf.Min(currentMaxIndex + amount, obstacleList.Count);
            return (newMaxIndex, obstacleList);
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