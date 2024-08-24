using System;
using System.Collections.Generic;
using Extentions;
using ObstacleGeneration;
using UnityEngine;
using Random = System.Random;


namespace Core.ObstacleGeneration
{
    public class ObstacleGeneratorHandler : MonoBehaviour
    {
        // this dict is arranging all obstacles by difficulty levels (1,2,3),
        // the int in the tuple is reffering to the max index currently available for that list of obstacles
        private Dictionary<int, ValueTuple<int, List<Obstacle>>> _difficultyToObstacleMap;

        // this list will hold the chances for generating each class of obstacle
        [SerializeField] List<SerializableTuple<int, float>> _difficultyToChanceMap;


        public void Init(Dictionary<int, ValueTuple<int, List<Obstacle>>> difficultyToObstacleMap)
        {
            _difficultyToObstacleMap = difficultyToObstacleMap;
        }

        void Start()
        {
            // Run 20 iterations
            List<Vector3> results20Positive = new List<Vector3>();
            for (int i = 0; i < 20; i++)
            {
                AdjustWeights();
                // Print the results
                foreach (SerializableTuple<int, float> pair in _difficultyToChanceMap)
                {
                    Debug.Log($"{pair.second}");
                }

                Debug.Log("-------");
            }
        }

        public void AddObstacles(int difficulty, int amount)
        {
            var (currentMaxIndex, obstacleList) = _difficultyToObstacleMap[difficulty];
            int newMaxIndex = Mathf.Min(currentMaxIndex + amount, obstacleList.Count);
            _difficultyToObstacleMap[difficulty] = (newMaxIndex, obstacleList);
        }

        public Obstacle GetRandomObstacle()
        {
            int randomNumber = UnityEngine.Random.Range(0, 101);
            int difficultyRandomed = RandomNextObstacleDifficulty(randomNumber);
            if (_difficultyToObstacleMap[difficultyRandomed].Item1 == 0)
            {
                Debug.Log($"There has been an invalid difficulty selection with diffuculty {difficultyRandomed}");
            }

            return _difficultyToObstacleMap[difficultyRandomed].Item2[
                UnityEngine.Random.Range(0, _difficultyToObstacleMap[difficultyRandomed].Item1)];
        }

        public void AdjustWeights()
        {
            // Adjust the numbers with a minimum check to avoid negative values
            float aAdjusted =
                Mathf.Max((_difficultyToChanceMap[0].second - 5), 0); // First number goes lower, but not below 0
            float bAdjusted = Mathf.Min(_difficultyToChanceMap[1].second + 5, 90f); // Second number goes bigger
            float cAdjusted =
                Mathf.Min(_difficultyToChanceMap[2].second + 2.5f, 50f); // Third number goes bigger by a smaller margin

            // Ensure the sum is exactly 100 by normalizing
            float totalAdjusted = aAdjusted + bAdjusted + cAdjusted;

            float scale = 100 / totalAdjusted;
            _difficultyToChanceMap[0].second = (aAdjusted * scale);
            _difficultyToChanceMap[1].second = (bAdjusted * scale);
            _difficultyToChanceMap[2].second = (cAdjusted * scale);
        }


        private int RandomNextObstacleDifficulty(int randomNumber)
        {
            float currentNumber = randomNumber;
            foreach (SerializableTuple<int, float> pair in _difficultyToChanceMap)
            {
                if (pair.second >= currentNumber)
                {
                    return pair.first;
                }

                currentNumber -= pair.second;
            }

            return 0;
        }
    }
}