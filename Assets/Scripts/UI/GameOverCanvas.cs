using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverCanvas : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject gameOverPanel;
        void Start()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, ShowGameOverPanel);

        }

        // Update is called once per frame

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, ShowGameOverPanel);
        }

        private void ShowGameOverPanel(object obj)
        {
            StartCoroutine(ShowGameOverPanelAfterDelay());
        }

        private IEnumerator ShowGameOverPanelAfterDelay()
        {
            yield return new WaitForSeconds(1.4f);
            gameOverPanel.SetActive(true);
            CoreManager.instance.TimeManager.PauseGame();

            
        }


        // will be called by a UI button
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