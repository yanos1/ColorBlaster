using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.ObstacleGeneration;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Core.Managers
{
    public class ControlPanelManager
    {
        public int[][] obstacleToDifficultyPerLevel;
        public float[] levelSpeeds = new float[5];
        public float colorWheelRotationSpeed;
        public float colorWheelShootingRotationSpeed;
        public float colorWheelRotationSpeedIncrease;
        public float obstacleRotationSpeed;
        public float obstacleRotationSpeedIncreasePerLevel;
        public float chasingObstaclesMovespeed;
        public float offsettingobstaclesMovespeed;
        public float movingObstaclesMovespeed;
        public float shootingCooldown;
        public float playerMovementSpeed;

        public int obstaclesPerLevel;
        public float sessionMultiplier;

        public int numbersOfRocketsToShootPerLevel;
        public bool canSpawnRotatingObstacles;
        public bool canSpawnChasingObstacles;
        public bool canSpawnOffsettingObstacles;
        public bool canSpawnMovingObstacles;
        public bool canSpawnRockets;
        public bool canSpawnMultipleHits;


        public bool spawnBossObstacleAtTheEndOfLevel;


        public int Level = 0;
        public int Session = 0;
        public int minBossLevelDifficulty;
        public int maxBossLevelDifficulty;
        public float bossLevelDifficultyIncreasePerLevel;
        public float distanceBetweenObstacles;


        public ControlPanelManager()
        {
            LoadControlPanelSettings();
        }


        // Load settings from PlayerPrefs or default values
        public void LoadControlPanelSettings()
        {
            colorWheelRotationSpeed = PlayerPrefs.GetFloat("ColorWheelRotationSpeed", 150f);
            levelSpeeds = Array.ConvertAll(PlayerPrefs.GetString("LevelSpeeds", "2.1,2.3,2.5,2.7,2.9").Split(","),
                float.Parse);
            colorWheelShootingRotationSpeed = PlayerPrefs.GetFloat("ColorWheelShootingRotationSpeed", 20f);
            obstacleRotationSpeed = PlayerPrefs.GetFloat("ObstacleRotationSpeed", 12f);
            colorWheelRotationSpeedIncrease = PlayerPrefs.GetInt("colorWheelRotationSpeedIncrease", 13);
            obstacleRotationSpeedIncreasePerLevel = PlayerPrefs.GetFloat("ObstacleRotationSpeedIncreasePerLevel", 2f);
            shootingCooldown = PlayerPrefs.GetFloat("ShootingCooldown", 0.15f);
            playerMovementSpeed = PlayerPrefs.GetFloat("PlayerMovementSpeed", 6.5f);
            chasingObstaclesMovespeed = PlayerPrefs.GetFloat("ChasingObstaclesMovespeed", 1.3f);
            movingObstaclesMovespeed = PlayerPrefs.GetFloat("MovingObstaclesMovespeed", 1.9f);
            offsettingobstaclesMovespeed = PlayerPrefs.GetFloat("OffsettingObstaclesMovespeed", 1.5f);

            obstaclesPerLevel = PlayerPrefs.GetInt("ObstaclesPerLevel", 10);
            sessionMultiplier = PlayerPrefs.GetFloat("SessionMultiplier", 1.1f);

            numbersOfRocketsToShootPerLevel = PlayerPrefs.GetInt("NumbersOfRocketsToShootPerLevel", 2);

            canSpawnRotatingObstacles = PlayerPrefs.GetInt("CanSpawnRotatingObstacles", 1) == 1;
            canSpawnChasingObstacles = PlayerPrefs.GetInt("CanSpawnChasingObstacles", 1) == 1;
            canSpawnOffsettingObstacles = PlayerPrefs.GetInt("CanSpawnOffsettingObstacles", 1) == 1;
            canSpawnMovingObstacles = PlayerPrefs.GetInt("CanSpawnMovingObstacles", 1) == 1;
            canSpawnRockets = PlayerPrefs.GetInt("CanSpawnRockets", 1) == 1;
            canSpawnMultipleHits = PlayerPrefs.GetInt("CanSpawnMultipleHits", 1) == 1;
            obstacleToDifficultyPerLevel = ParseArray(PlayerPrefs.GetString("ObstacleToDifficultyPerLevel",
                "80,20,0,70,20,10,60,30,10,50,30,20,40,30,30"));
            spawnBossObstacleAtTheEndOfLevel = PlayerPrefs.GetInt("SpawnBossObstacleAtTheEndOfLevel", 1) == 1;
            minBossLevelDifficulty = PlayerPrefs.GetInt("MinBossLevelDifficulty", 4);
            maxBossLevelDifficulty = PlayerPrefs.GetInt("MaxBossLevelDifficulty", 12);
            bossLevelDifficultyIncreasePerLevel = PlayerPrefs.GetFloat("bossLevelDifficultyIncreasePerLevel", 1.5f);
            distanceBetweenObstacles = PlayerPrefs.GetFloat("DistanceBetweenObstacles", 70f);
        }

        private int[][] ParseArray(string str)
        {
            int[] arr = Array.ConvertAll(str.Split(','), int.Parse);

            // Create a jagged array with 5 arrays, each containing 3 elements
            int[][] subArrays = new int[5][];

            // Fill the sub-arrays
            for (int i = 0; i < 5; i++)
            {
                subArrays[i] = new int[3];
                Array.Copy(arr, i * 3, subArrays[i], 0, 3);
            }

            return subArrays;
        }

        // Save settings to PlayerPrefs
        public void SaveControlPanelSettings()
        {
            PlayerPrefs.SetFloat("LevelSpeed1", levelSpeeds[0]);
            PlayerPrefs.SetFloat("LevelSpeed2", levelSpeeds[1]);
            PlayerPrefs.SetFloat("LevelSpeed3", levelSpeeds[2]);
            PlayerPrefs.SetFloat("LevelSpeed4", levelSpeeds[3]);
            PlayerPrefs.SetFloat("LevelSpeed5", levelSpeeds[4]);

            PlayerPrefs.SetFloat("ColorWheelRotationSpeed", colorWheelRotationSpeed);
            PlayerPrefs.SetFloat("ColorWheelShootingRotationSpeed", colorWheelShootingRotationSpeed);
            PlayerPrefs.SetFloat("ObstacleRotationSpeedIncreasePerLevel", colorWheelRotationSpeedIncrease);
            PlayerPrefs.SetFloat("ObstacleRotationSpeed", obstacleRotationSpeed);
            PlayerPrefs.SetFloat("ObstacleRotationSpeedIncreasePerLevel", obstacleRotationSpeedIncreasePerLevel);
            PlayerPrefs.SetFloat("ShootingCooldown", shootingCooldown);
            PlayerPrefs.SetFloat("PlayerMovementSpeed", playerMovementSpeed);
            PlayerPrefs.SetFloat("ChasingObstaclesMovespeed", chasingObstaclesMovespeed);
            PlayerPrefs.SetFloat("MovingObstaclesMovespeed", movingObstaclesMovespeed);
            PlayerPrefs.SetFloat("OffsettingObstaclesMovespeed", offsettingobstaclesMovespeed);
            PlayerPrefs.SetInt("ObstaclesPerLevel", obstaclesPerLevel);
            PlayerPrefs.SetFloat("SessionMultiplier", sessionMultiplier);

            PlayerPrefs.SetInt("NumbersOfRocketsToShootPerLevel", numbersOfRocketsToShootPerLevel);
            PlayerPrefs.SetInt("CanSpawnRotatingObstacles", canSpawnRotatingObstacles ? 1 : 0);
            PlayerPrefs.SetInt("CanSpawnChasingObstacles", canSpawnChasingObstacles ? 1 : 0);
            PlayerPrefs.SetInt("CanSpawnOffsettingObstacles", canSpawnOffsettingObstacles ? 1 : 0);
            PlayerPrefs.SetInt("CanSpawnMovingObstacles", canSpawnMovingObstacles ? 1 : 0);
            PlayerPrefs.SetInt("CanSpawnRockets", canSpawnRockets ? 1 : 0);
            PlayerPrefs.SetInt("CanSpawnMultipleHits", canSpawnMultipleHits ? 1 : 0);
            canSpawnMultipleHits = PlayerPrefs.GetInt("CanSpawnMultipleHits", 1) == 1;
            PlayerPrefs.SetString("ObstacleToDifficultyPerLevel",
                string.Join(",", obstacleToDifficultyPerLevel.SelectMany(x => x).ToArray()));
            PlayerPrefs.SetString("LevelSpeeds", String.Join(",", levelSpeeds));
            PlayerPrefs.SetInt("SpawnBossObstacleAtTheEndOfLevel", spawnBossObstacleAtTheEndOfLevel ? 1 : 0);
            PlayerPrefs.SetInt("MinBossLevelDifficulty", minBossLevelDifficulty);
            PlayerPrefs.SetInt("MaxBossLevelDifficulty", maxBossLevelDifficulty);
            PlayerPrefs.SetFloat("BossLevelDifficultyIncreasePerLevel", bossLevelDifficultyIncreasePerLevel);
            PlayerPrefs.SetFloat("DistanceBetweenObstacles", distanceBetweenObstacles);


            PlayerPrefs.Save();
        }


        // Print current parameters
        // public void PrintParametersAtEndOfSession()
        // {
        //     string output = "Game Parameters:\n";
        //
        //     output += $"Level Speeds: {string.Join(",", levelSpeeds)}\n";
        //     output += $"Color Wheel Rotation Speed: {colorWheelRotationSpeed}\n";
        //     output += $"Color Wheel Shooting Rotation Speed: {colorWheelShootingRotationSpeed}\n";
        //     output += $"Color Wheel Shooting Rotation Speed Increase Per Level: {colorWheelRotationSpeedIncrease}\n";
        //     output += $"Obstacle Rotation Speed Increase Per Level: {obstacleRotationSpeedIncreasePerLevel}\n";
        //     output += $"Obstacle Rotation Speed: {obstacleRotationSpeed}\n";
        //     output += $"Shooting Speed: {shootingCooldown}\n";
        //     output += $"Player Movement Speed: {playerMovementSpeed}\n";
        //     output += $"Obstacles Per Level: {obstaclesPerLevel}\n";
        //     output += $"Session Multiplier: {sessionMultiplier}\n";
        //     output += $"Rockets to Shoot: {numbersOfRocketsToShootPerLevel}\n";
        //     output += $"Can Spawn Rotating Obstacles: {canSpawnRotatingObstacles}\n";
        //     output += $"Can Spawn Chasing Obstacles: {canSpawnChasingObstacles}\n";
        //
        //     Debug.Log(output);
        // }

        // Update parameter values from UI text input or toggle
        public void UpdateParameterFromInput(string parameterName, string value)
        {
            Debug.Log(parameterName + "aaaaa");

            switch (parameterName)
            {
                case "ColorWheelRotationSpeed":
                    Debug.Log("UPDATED ROTATION SPEED");
                    colorWheelRotationSpeed = float.Parse(value);
                    break;
                case "ColorWheelShootingRotationSpeed":
                    colorWheelShootingRotationSpeed = float.Parse(value);
                    break;
                case "ShootingCooldown":
                    shootingCooldown = float.Parse(value);
                    break;
                case "PlayerMovementSpeed":
                    playerMovementSpeed = float.Parse(value);
                    break;
                case "ObstaclesPerLevel":
                    obstaclesPerLevel = int.Parse(value);
                    break;
                case "ObstacleRotationSpeed":
                    obstacleRotationSpeed = float.Parse(value);
                    break;

                case "SessionMultiplier":
                    sessionMultiplier = float.Parse(value);
                    break;
                case "NumberOfRocketsPerLevel":
                    numbersOfRocketsToShootPerLevel = int.Parse(value);
                    break;
                case "ColorWheelRotationSpeedIncrease":
                    colorWheelRotationSpeedIncrease = int.Parse(value);
                    break;
                case "ObstacleRotationSpeedIncreasePerLevel":
                    obstacleRotationSpeedIncreasePerLevel = float.Parse(value);
                    break;
                case "ObstacleDifficultyToCountPerLevel":
                    obstacleToDifficultyPerLevel = ParseArray(value.Replace("\n", ""));
                    break;
                case "LevelSpeeds":
                    levelSpeeds = Array.ConvertAll(value.Split(','), float.Parse);
                    break;
                case "MinBossLevelDifficulty":
                    minBossLevelDifficulty = int.Parse(value);
                    break;
                case "MaxBossLevelDifficulty":
                    Debug.Log($"UPDATING MAXBOSSLEVELDIFFICULTY TO : {value}");
                    maxBossLevelDifficulty = int.Parse(value);
                    break;
                case "BossLevelDifficultyIncreasePerLevel":
                    bossLevelDifficultyIncreasePerLevel = float.Parse(value);
                    break;
                case "DistanceBetweenObstacles":
                    distanceBetweenObstacles = float.Parse(value);
                    break;
                case "OffsettingObstaclesMovespeed":
                    offsettingobstaclesMovespeed = float.Parse(value);
                    break;
                case "ChasingObstaclesMovespeed":
                    chasingObstaclesMovespeed = float.Parse(value);
                    break;
                case "MovingObstaclesMovespeed":
                    movingObstaclesMovespeed = float.Parse(value);
                    break;
                default:
                    Debug.LogWarning($"Unknown parameter: {parameterName}");
                    break;
            }
        }

        public void OnToggleChanged(string parameterName, bool isOn)
        {
            switch (parameterName)
            {
                case "CanSpawnRotatingObstacles":
                    canSpawnRotatingObstacles = isOn;
                    break;
                case "CanSpawnChasingObstacles":
                    canSpawnChasingObstacles = isOn;
                    break;
                case "CanSpawnOffsettingObstacles":
                    canSpawnOffsettingObstacles = isOn;
                    break;
                case "SpawnBossObstacleAtTheEndOfLevel":
                    spawnBossObstacleAtTheEndOfLevel = isOn;
                    break;
                case "CanSpawnMovingObstacles":
                    canSpawnMovingObstacles = isOn;
                    break;
                case "CanSpawnMultipleHits":
                    canSpawnMultipleHits = isOn;
                    break;
                case "CanSpawnRockets":
                    canSpawnRockets = isOn;
                    break;
                default:
                    Debug.LogWarning($"Toggle {parameterName} is not recognized!");
                    break;
            }
        }


        public float GetGameMoveSpeed()
        {
            if (!CoreManager.instance.GameManager.IsRunActive)
            {
                return 0f;
            }

            float currentSpeed = levelSpeeds[Level];
            for (int i = 0; i < Session; ++i)
            {
                currentSpeed *= sessionMultiplier;
            }

            return currentSpeed;
        }

        public float GetObstacleRotationSpeed()
        {
            return obstacleRotationSpeed +
                   obstacleRotationSpeedIncreasePerLevel * (levelSpeeds.Length * (Session) + Level);
        }

        public float GetObstacleSpeedMuliplier(float baseSpeed)
        {
            float curSpeed = baseSpeed;
            for (int i = 0; i < Session; i++)
            {
                curSpeed *= sessionMultiplier;
            }

            return curSpeed;
        }

        public float GetWheelRotationSpeedWhileShooting()
        {
            return colorWheelShootingRotationSpeed;
        }

        public float GetWheeRotationSpeed()
        {
            float speed = colorWheelRotationSpeed +
                          colorWheelRotationSpeedIncrease * ((levelSpeeds.Length * Session) + Level);
            // Debug.Log(speed);
            return speed;
        }
    }
}