using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.Managers
{
    public class ControlPanelUI : MonoBehaviour
    {
        public TMP_InputField levelsSpeedInput;
        public TMP_InputField colorWheelRotationSpeedInput;
        public TMP_InputField colorWheelShootingRotationSpeed;
        public TMP_InputField colorWheelRotationSpeedIncrease;
        public TMP_InputField shootingCooldownInput;
        public TMP_InputField playerMovementSpeedInput;
        public TMP_InputField chasingObstaclesMovespeedInput;
        public TMP_InputField offsettingObstaclesMovespeedInput;
        public TMP_InputField movingObstaclesMovespeedInput;
        public TMP_InputField obstacleRotationSpeed;
        public TMP_InputField obstacleRotationSpeedIncreasePerLevelInput;
        public TMP_InputField obstaclesPerLevelInput;
        public TMP_InputField sessionMultiplierInput;
        public TMP_InputField obstacleToDifficultyPerLevel;
        public TMP_InputField minBossLevelDifficultyInput;
        public TMP_InputField maxBossLevelDifficultyInput;
        public TMP_InputField bossLevelDifficultyIncreasePerLevelInput;
        public TMP_InputField distanceBetweenObstaclesInput;

        public Toggle canSpawnRotatingObstaclesToggle;
        public Toggle canSpawnChasingObstaclesToggle;
        public Toggle canSpawnOffsettingObstaclesToggle;
        public Toggle canSpawnMovingObstaclesToggle;
        public Toggle canSpawnRocketsToggle;
        public Toggle canSpawnMultipleHitsToggle;
        public Toggle SpawnBossObstacleAtTheEndOfLevel;

        private ControlPanelManager controlPanelManager;

        private void Start()
        {
            controlPanelManager = CoreManager.instance.ControlPanelManager;

            // Subscribe to OnDeselect event for each input field
            SubscribeToInputFieldEvents();
            // SubscribeToToggleEvents();

            // Load initial values into input fields and toggles
            LoadUIValues(); 
        }

        // private void SubscribeToToggleEvents()
        // {
        //     canSpawnRotatingObstaclesToggle.
        // }

        public void LoadUIValues()
        {

            colorWheelRotationSpeedInput.text = controlPanelManager.colorWheelRotationSpeed.ToString();
            shootingCooldownInput.text = controlPanelManager.shootingCooldown.ToString();
            playerMovementSpeedInput.text = controlPanelManager.playerMovementSpeed.ToString();
            chasingObstaclesMovespeedInput.text = controlPanelManager.chasingObstaclesMovespeed.ToString();
            offsettingObstaclesMovespeedInput.text = controlPanelManager.offsettingobstaclesMovespeed.ToString();
            movingObstaclesMovespeedInput.text = controlPanelManager.movingObstaclesMovespeed.ToString();
            obstaclesPerLevelInput.text = controlPanelManager.obstaclesPerLevel.ToString();
            sessionMultiplierInput.text = controlPanelManager.sessionMultiplier.ToString();
            colorWheelRotationSpeedIncrease.text = controlPanelManager.colorWheelRotationSpeedIncrease.ToString();
            colorWheelShootingRotationSpeed.text = controlPanelManager.colorWheelShootingRotationSpeed.ToString();
            obstacleRotationSpeed.text = controlPanelManager.obstacleRotationSpeed.ToString();
            obstacleRotationSpeedIncreasePerLevelInput.text = controlPanelManager.obstacleRotationSpeedIncreasePerLevel.ToString();
            levelsSpeedInput.text = string.Join(",", controlPanelManager.levelSpeeds);
            minBossLevelDifficultyInput.text = controlPanelManager.minBossLevelDifficulty.ToString();
            maxBossLevelDifficultyInput.text = controlPanelManager.maxBossLevelDifficulty.ToString();
            bossLevelDifficultyIncreasePerLevelInput.text =
                controlPanelManager.bossLevelDifficultyIncreasePerLevel.ToString();
            distanceBetweenObstaclesInput.text = controlPanelManager.distanceBetweenObstacles.ToString();
            var numbers = controlPanelManager.obstacleToDifficultyPerLevel.SelectMany(x => x).ToArray();
            var result = new List<string>();

            for (int i = 0; i < numbers.Length; i++)
            {
                result.Add(numbers[i].ToString());
    
                // Add ",\n" after every 3 numbers, and just "," between others
                if ((i + 1) % 3 == 0 && i != numbers.Length - 1)
                {
                    result.Add(",\n");
                }
                else if (i != numbers.Length - 1)
                {
                    result.Add(",");
                }
            }
            obstacleToDifficultyPerLevel.text = string.Join("", result);

            // Toggles
            canSpawnRotatingObstaclesToggle.isOn = controlPanelManager.canSpawnRotatingObstacles;
            canSpawnChasingObstaclesToggle.isOn = controlPanelManager.canSpawnChasingObstacles;
            SpawnBossObstacleAtTheEndOfLevel.isOn = controlPanelManager.spawnBossObstacleAtTheEndOfLevel;
            canSpawnOffsettingObstaclesToggle.isOn = controlPanelManager.canSpawnOffsettingObstacles;
            canSpawnMovingObstaclesToggle.isOn = controlPanelManager.canSpawnMovingObstacles;
            canSpawnRocketsToggle.isOn = controlPanelManager.canSpawnRockets;
            canSpawnMultipleHitsToggle.isOn = controlPanelManager.canSpawnMultipleHits;
        }

        private void SubscribeToInputFieldEvents()
        {

            colorWheelRotationSpeedInput.onDeselect.AddListener(delegate { OnTextInputChanged(colorWheelRotationSpeedInput); });
            levelsSpeedInput.onDeselect.AddListener(delegate { OnTextInputChanged(levelsSpeedInput); });
            colorWheelShootingRotationSpeed.onDeselect.AddListener(delegate { OnTextInputChanged(colorWheelShootingRotationSpeed); });
            colorWheelRotationSpeedIncrease.onDeselect.AddListener(delegate { OnTextInputChanged(colorWheelRotationSpeedIncrease); });
            shootingCooldownInput.onDeselect.AddListener(delegate { OnTextInputChanged(shootingCooldownInput); });
            playerMovementSpeedInput.onDeselect.AddListener(delegate { OnTextInputChanged(playerMovementSpeedInput); });
            chasingObstaclesMovespeedInput.onDeselect.AddListener(delegate { OnTextInputChanged(chasingObstaclesMovespeedInput); });
            offsettingObstaclesMovespeedInput.onDeselect.AddListener(delegate { OnTextInputChanged(offsettingObstaclesMovespeedInput); });
            movingObstaclesMovespeedInput.onDeselect.AddListener(delegate { OnTextInputChanged(movingObstaclesMovespeedInput); });
            obstacleRotationSpeed.onDeselect.AddListener(delegate { OnTextInputChanged(obstacleRotationSpeed); });
            obstaclesPerLevelInput.onDeselect.AddListener(delegate { OnTextInputChanged(obstaclesPerLevelInput); });
            sessionMultiplierInput.onDeselect.AddListener(delegate { OnTextInputChanged(sessionMultiplierInput); });
            obstacleToDifficultyPerLevel.onDeselect.AddListener(delegate { OnTextInputChanged(obstacleToDifficultyPerLevel); });
            obstacleRotationSpeedIncreasePerLevelInput.onDeselect.AddListener(delegate { OnTextInputChanged(obstacleRotationSpeedIncreasePerLevelInput); });
            canSpawnChasingObstaclesToggle.onValueChanged.AddListener(delegate {OnToggleChanged((canSpawnChasingObstaclesToggle));});
            canSpawnRotatingObstaclesToggle.onValueChanged.AddListener(delegate {OnToggleChanged((canSpawnRotatingObstaclesToggle));});
            SpawnBossObstacleAtTheEndOfLevel.onValueChanged.AddListener(delegate {OnToggleChanged((SpawnBossObstacleAtTheEndOfLevel));});
            canSpawnRocketsToggle.onValueChanged.AddListener(delegate {OnToggleChanged((canSpawnRocketsToggle));});
            canSpawnMultipleHitsToggle.onValueChanged.AddListener(delegate {OnToggleChanged((canSpawnMultipleHitsToggle));});
            canSpawnMovingObstaclesToggle.onValueChanged.AddListener(delegate {OnToggleChanged((canSpawnMovingObstaclesToggle));});
            canSpawnOffsettingObstaclesToggle.onValueChanged.AddListener(delegate {OnToggleChanged((canSpawnOffsettingObstaclesToggle));});
            minBossLevelDifficultyInput.onDeselect.AddListener(delegate {OnTextInputChanged((minBossLevelDifficultyInput));});
            maxBossLevelDifficultyInput.onDeselect.AddListener(delegate {OnTextInputChanged((maxBossLevelDifficultyInput));});
            bossLevelDifficultyIncreasePerLevelInput.onDeselect.AddListener(delegate {OnTextInputChanged(bossLevelDifficultyIncreasePerLevelInput);});
            distanceBetweenObstaclesInput.onDeselect.AddListener(delegate {OnTextInputChanged(distanceBetweenObstaclesInput);});
        }

        public void OnTextInputChanged(TMP_InputField inputField)
        {
            string parameterName = inputField.name; // You can name your InputFields accordingly
            string inputValue = inputField.text;

            controlPanelManager.UpdateParameterFromInput(parameterName, inputValue);
        }

        public void OnToggleChanged(Toggle toggle)
        {
            string parameterName = toggle.name; // You can name your InputFields accordingly
            bool isOn = toggle.isOn;
            controlPanelManager.OnToggleChanged(parameterName, isOn);
        }

        public void OnSaveButtonClicked()
        {
            controlPanelManager.SaveControlPanelSettings();
            gameObject.SetActive(false);
        }
    }
}