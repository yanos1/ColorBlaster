using System;
using Core.Managers;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI gemsOwned;
        void Start()
        {
            gemsOwned.text = CoreManager.instance.UserDataManager.GemsOwned.ToString();
        }

        // Update is called once per frame

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.BoughtItem, OnBoughtItem);
        }
        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.BoughtItem, OnBoughtItem);
        }

        private void OnBoughtItem(object obj)
        {
            gemsOwned.text = CoreManager.instance.UserDataManager.GemsOwned.ToString();
        }

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
