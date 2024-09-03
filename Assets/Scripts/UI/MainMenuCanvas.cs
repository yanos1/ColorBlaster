using Core.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuCanvas : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void StartGame()
        {
            // enter juice
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("GameScene");
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            CoreManager.instance.EventManager.InvokeEvent(EventNames.StartGame, null);

        }
    }
}
