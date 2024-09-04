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
        [SerializeField] private GameObject oneMoreAttemptPanel;
        void Start()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, ShowGameOverPanel);
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, ShowOneMoreAttemptMenu);

        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, ShowGameOverPanel);
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, ShowOneMoreAttemptMenu);
        }

        private void ShowGameOverPanel(object obj)
        {
            print("show panel!");
            StartCoroutine(ShowGamePanelPanelAfterDelay(gameOverPanel, 0.7f));
        }

        private void ShowOneMoreAttemptMenu(object obj)
        {
            StartCoroutine(ShowGamePanelPanelAfterDelay(oneMoreAttemptPanel, 1.2f,true));
        }

        private IEnumerator ShowGamePanelPanelAfterDelay(GameObject panel, float delay, bool pause=false)
        {
            yield return new WaitForSeconds(delay);
            panel.SetActive(true);
            // if (pause)
            // {
            //     CoreManager.instance.TimeManager.PauseGame();
            // }
        }


  
    }
}