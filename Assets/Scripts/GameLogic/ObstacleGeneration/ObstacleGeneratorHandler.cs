using System;
using System.Collections.Generic;
using Core.Managers;
using Extentions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.ObstacleGeneration
{
    public class ObstacleGeneratorHandler : MonoBehaviour
    {
        [SerializeField] private SerializableTuple<int, float>[] _difficultyToChanceMap;

        private Dictionary<int, ValueTuple<int, List<Obstacle>>> _difficultyToObstacleMap;
        private Obstacle[] _currentBatch;
        private int _currentBatchIndex;
        private const int NumObstaclesInBatch = 8;

        public void Init(Dictionary<int, ValueTuple<int, List<Obstacle>>> difficultyToObstacleMap)
        {
            _difficultyToObstacleMap = difficultyToObstacleMap;
            _currentBatch = InitNewObstacleBatch();
            _currentBatchIndex = 0;
        }

        private Obstacle[] InitNewObstacleBatch()
        {
            var newBatch = new Obstacle[NumObstaclesInBatch];
            int numTreasureObstacles = Random.Range(1, 3);

            // Fill with regular obstacles
            for (int i = 0; i < NumObstaclesInBatch; ++i)
            {
                newBatch[i] = GenerateRandomObstacle();
            }

            // Add treasure obstacles from difficulty 0
            for (int i = 0; i < numTreasureObstacles; i++)
            {
                newBatch[i] = GenerateTreasureObstacle();
            }

            return UtilityFunctions.ShuffleArray(newBatch);
        }

        private Obstacle GenerateRandomObstacle()
        {
            int randomNumber = Random.Range(0, 101);
            int difficulty = GetRandomObstacleDifficulty(randomNumber);

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
            float currentNumber = randomNumber;
            foreach (var (difficulty, chance) in _difficultyToChanceMap)
            {
                if (chance >= currentNumber)
                {
                    print($"chance: {chance} number: {currentNumber}  {difficulty}");
                    return difficulty;
                }
                currentNumber -= chance;
            }
            print($"{currentNumber}  {0}");
            return 0; // Fallback to easiest difficulty if nothing matches
        }


        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.IncreaseGameDifficulty, AdjustObstacleDifficulty);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.IncreaseGameDifficulty, AdjustObstacleDifficulty);
        }

        public Obstacle GetNextObstacle()
        {
            // Check if the current batch is exhausted, generate a new one if necessary
            if (_currentBatchIndex >= _currentBatch.Length)
            {
                _currentBatch = InitNewObstacleBatch();
                print("ADJUST WEIGHT");
                AdjustWeights();  // make each batch harder then the one before !
                _currentBatchIndex = 0;
            }

            var nextObstacle = _currentBatch[_currentBatchIndex];
            _currentBatchIndex++;
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

        private void AdjustWeights()
        {

            float aAdjusted = Mathf.Max(_difficultyToChanceMap[0].second - 10, 0);
            float bAdjusted = Mathf.Min(_difficultyToChanceMap[1].second + 6, 90f);
            float cAdjusted = Mathf.Min(_difficultyToChanceMap[2].second + 4, 50f);

            float totalAdjusted = aAdjusted + bAdjusted + cAdjusted;

            // Normalize chances to ensure they sum to 100
            float scale = 100f / totalAdjusted;
            _difficultyToChanceMap[0].second = aAdjusted * scale;
            _difficultyToChanceMap[1].second = bAdjusted * scale;
            _difficultyToChanceMap[2].second = cAdjusted * scale;
        }
    }
}
