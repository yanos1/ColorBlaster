using Core.Managers;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuCanvas : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private GameObject itemShopPanel;
        [SerializeField] private GameObject gemShopPanel;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OpenGemShop()
        {
            gemShopPanel.SetActive(true);
        }
        public void OpenItemShop()
        {
            itemShopPanel.SetActive(true);
        }

        public void OpenControlPanel()
        {
            controlPanel.SetActive(true);
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
