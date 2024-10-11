using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // If using regular UI Text, or TMPro if using TextMeshPro

namespace Core.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public TextMeshProUGUI
            levelDisplayText; // Reference to the UI Text or use TMPro.TextMeshProUGUI if using TextMeshPro

        public int ObstacleCrossedThisLevel
        {
            get => obstaclesCrossedThisLevel;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                obstaclesCrossedThisLevel = value;
            }
        }

        private int obstaclesCrossedThisLevel;


        private void Start()
        {
            // Initial setup if needed
            if (levelDisplayText != null)
            {
                levelDisplayText.gameObject.SetActive(false); // Make sure it's initially hidden
            }

        }
        private void OnEnable()
        {
            // Subscribe to the StartGame event
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, OnGameOver);
            CoreManager.instance.EventManager.AddListener(EventNames.LevelUp, OnLevelUp);
        }

        private void OnDisable()
        {
            // Unsubscribe from the event to prevent memory leaks
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, OnGameOver);
            CoreManager.instance.EventManager.RemoveListener(EventNames.LevelUp, OnLevelUp);
        }

        private void OnGameOver(object obj)
        {
            // Reset level and session when the game starts
            ResetLevelAndSession();
        }

        private void OnLevelUp(object obj)
        {
            obstaclesCrossedThisLevel = 0;
            CoreManager.instance.ControlPanelManager.Level++;

            if (CoreManager.instance.ControlPanelManager.Level ==
                CoreManager.instance.ControlPanelManager.levelSpeeds.Length)
            {
                CoreManager.instance.ControlPanelManager.Level = 0;
                CoreManager.instance.ControlPanelManager.Session++;
                CoreManager.instance.EventManager.InvokeEvent(EventNames.SessionUp, null);
                Debug.Log("New Session Is Called!");
            }

            // Display session and level when leveling up
            DisplaySessionAndLevel();
        }


        private void ResetLevelAndSession()
        {
            CoreManager.instance.ControlPanelManager.Level = 0;
            CoreManager.instance.ControlPanelManager.Session = 0;
            obstaclesCrossedThisLevel = 0;
            Debug.Log("Level and Session Reset");
        }

 

        // Coroutine to display session and level
        private void DisplaySessionAndLevel()
        {
            if (levelDisplayText != null)
            {
                string message =
                    $"Session: {CoreManager.instance.ControlPanelManager.Session}, Level: {CoreManager.instance.ControlPanelManager.Level}";
                levelDisplayText.text = message;
                StartCoroutine(DisplayTextForDuration(2.2f)); // Display the text for 2.2 seconds
            }
        }

        // Coroutine to handle showing the text for 2.2 seconds
        private IEnumerator DisplayTextForDuration(float duration)
        {
            levelDisplayText.gameObject.SetActive(true); // Show the text
            yield return new WaitForSeconds(duration); // Wait for 2.2 seconds
            levelDisplayText.gameObject.SetActive(false); // Hide the text
        }
    }
}