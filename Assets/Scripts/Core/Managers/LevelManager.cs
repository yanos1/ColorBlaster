using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;  // If using regular UI Text, or TMPro if using TextMeshPro

namespace Core.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public TextMeshProUGUI levelDisplayText; // Reference to the UI Text or use TMPro.TextMeshProUGUI if using TextMeshPro

        public int ObstacleCrossedThisLevel
        {
            get => obstaclesCrossedThisLevel;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                obstaclesCrossedThisLevel = value;
                CheckLevelProgression();
            }
        }

        private int obstaclesCrossedThisLevel;

        private void OnEnable()
        {
            // Subscribe to the StartGame event
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, OnGameOver);
        }

        private void OnDisable()
        {
            // Unsubscribe from the event to prevent memory leaks
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, OnGameOver);
        }

        private void OnGameOver(object obj)
        {
            // Reset level and session when the game starts
            ResetLevelAndSession();
            DisplaySessionAndLevel();
        }

        private void CheckLevelProgression()
        {
            if (ObstacleCrossedThisLevel == CoreManager.instance.ControlPanelManager.obstaclesPerLevel + (CoreManager.instance.ControlPanelManager.spawnBossObstacleAtTheEndOfLevel ? 1:0))
            {
                obstaclesCrossedThisLevel = 0;
                CoreManager.instance.ControlPanelManager.Level++;

                if (CoreManager.instance.ControlPanelManager.Level == CoreManager.instance.ControlPanelManager.levelSpeeds.Length)
                {
                    CoreManager.instance.ControlPanelManager.Level = 0;
                    CoreManager.instance.ControlPanelManager.Session++;
                    Debug.Log("New Session Is Called!");
                }

                // Display session and level when leveling up
                DisplaySessionAndLevel();

                CoreManager.instance.EventManager.InvokeEvent(
                    EventNames.LevelUp, 
                    (CoreManager.instance.ControlPanelManager.Session, CoreManager.instance.ControlPanelManager.Level)
                );
            }
        }

        private void ResetLevelAndSession()
        {
            CoreManager.instance.ControlPanelManager.Level = 0;
            CoreManager.instance.ControlPanelManager.Session = 0;
            obstaclesCrossedThisLevel = 0;
            Debug.Log("Level and Session Reset");
        }

        private void Start()
        {
            // Initial setup if needed
            if (levelDisplayText != null)
            {
                levelDisplayText.gameObject.SetActive(false); // Make sure it's initially hidden
            }
        }

        // Coroutine to display session and level
        private void DisplaySessionAndLevel()
        {
            if (levelDisplayText != null)
            {
                string message = $"Session: {CoreManager.instance.ControlPanelManager.Session}, Level: {CoreManager.instance.ControlPanelManager.Level}";
                levelDisplayText.text = message;
                StartCoroutine(DisplayTextForDuration(2.2f));  // Display the text for 2.2 seconds
            }
        }

        // Coroutine to handle showing the text for 2.2 seconds
        private IEnumerator DisplayTextForDuration(float duration)
        {
            levelDisplayText.gameObject.SetActive(true);  // Show the text
            yield return new WaitForSeconds(duration);    // Wait for 2.2 seconds
            levelDisplayText.gameObject.SetActive(false); // Hide the text
        }
    }
}
