using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using GameLogic.ConsumablesGeneration;
using GameLogic.ObstacleGeneration;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// using Firebase;
// using Firebase.Database;
// using Firebase.Extensions;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace LoaderLogic
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private GameLoaderUI loaderUI;
        [SerializeField] private Style[] stylesList;
        [SerializeField] private List<ColorTheme> colorThemes;
        [SerializeField] private Obstacle[] baseObstaclesList;
        [SerializeField] private Obstacle[] bossObstaclesList;
        [SerializeField] private PoolEntry[] poolEntries;
        [SerializeField] private TextAsset itemCosts;
        [SerializeField] private TreasureChestBuff[] treasureChestBuffs;
        
        // TEST FIELDS
        [SerializeField] private bool TEST_MODE;
        [SerializeField] private Obstacle[] testObstacleList;
        
        
        private void Start()
        {
            StartCoroutine(StartLoadingAsync());
            loaderUI.Init(100);
        }

        private IEnumerator StartLoadingAsync()
        {
            yield return new WaitForSeconds(0.05f);  // fixes rare bugs
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loaderUI.transform.root.gameObject);
            LoadCoreManager();
        }

        private void LoadCoreManager()
        {
            var coreManager = new CoreManager(loaderUI,itemCosts, stylesList, colorThemes, baseObstaclesList,bossObstaclesList, poolEntries,treasureChestBuffs, OnCoreManagersLoaded);
        }

        private void OnCoreManagersLoaded()
        {
            SceneManager.sceneLoaded += OnLoadData;
            SceneManager.LoadScene("MainMenu");
        }
    

    private void LoadLocalData()
    {
        // Load Map Style
        string mapStyle = PlayerPrefs.GetString("MapStyle", "DefaultStyle");
        ApplyMapStyle(mapStyle);

        // Load Character Choice
        string characterChoice = PlayerPrefs.GetString("CharacterChoice", "DefaultCharacter");
        ApplyCharacterChoice(characterChoice);

    }

    private void ApplyCharacterChoice(string characterChoice)
    {
        return;
    }

    private void ApplyMapStyle(string style)
    {
        // Apply map style logic
    }

    // private void ProcessPurchasesData(DataSnapshot snapshot)
    // {
    //     // Process purchase data from Firebase
    // }
    
    private void OnLoadData(Scene scene, LoadSceneMode mode)
    {
        print("called onload data");
        SceneManager.sceneLoaded -= OnLoadData;
        OnLoadComplete();
    }

    private void OnLoadComplete()
    {
        print("finished loading");
        Destroy(loaderUI.transform.root.gameObject);
        Destroy(gameObject);
    }
    
}

}