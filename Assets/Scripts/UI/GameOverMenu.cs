using System;
using Core.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gemsOwnedText;


        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, AddGems);
        }

        public void RestartGame()
        {
            CoreManager.instance.EventManager.InvokeEvent(EventNames.RestartGame, null);
            SceneManager.sceneLoaded += OnGameReset;
            SceneManager.LoadScene("GameScene");
            
        }

        private void OnGameReset(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnGameReset;
            CoreManager.instance.TimeManager.ResumeTime();
            CoreManager.instance.EventManager.InvokeEvent(EventNames.StartGame, null);

        }
    }
}