using Core.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverMenu : MonoBehaviour
    {
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