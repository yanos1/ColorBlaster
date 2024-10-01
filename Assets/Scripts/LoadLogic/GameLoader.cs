﻿using System;
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
        [SerializeField] private Obstacle[] obstaclesList;
        [SerializeField] private PoolEntry[] poolEntries;
        [SerializeField] private TextAsset itemCosts;
        [SerializeField] private TreasureChestBuff[] treasureChestBuffs;

        [SerializeField] private float baseObjectSpeed;

        
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
           var coreManager =  new CoreManager();
           loaderUI.AddProgress(10);
           coreManager.InitializeManagers(itemCosts, stylesList, colorThemes, TEST_MODE ? testObstacleList :obstaclesList, poolEntries,treasureChestBuffs, baseObjectSpeed, OnCoreManagersLoaded, loaderUI);
            
        }

        private void OnCoreManagersLoaded()
        {
            loaderUI.AddProgress(10);
            // LoadLocalData(); // Load local data first
            StartCoroutine(LoadCloudData());
            StartCoroutine(LoadUserDataFromFirebase()); // Load cloud data in parallel
        }
    

    private void LoadLocalData()
    {
        // Load Map Style
        string mapStyle = PlayerPrefs.GetString("MapStyle", "DefaultStyle");
        ApplyMapStyle(mapStyle);

        // Load Character Choice
        string characterChoice = PlayerPrefs.GetString("CharacterChoice", "DefaultCharacter");
        ApplyCharacterChoice(characterChoice);

        // Load Coin Amount (Backup)
        int localCoinAmount = PlayerPrefs.GetInt("CoinAmount", 0);
        ApplyCoinAmount(localCoinAmount);

        loaderUI.AddProgress(30);
    }

    private IEnumerator LoadCloudData()
    {
        // Initialize Firebase if needed
        // if (FirebaseApp.DefaultInstance == null)
        // {
        //     FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        //     {
        //         if (task.Result == DependencyStatus.Available)
        //         {
        //             StartCoroutine(LoadUserDataFromFirebase());
        //         }
        //         else
        //         {
        //             Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
        //             OnLoadFailed();
        //         }
        //     });
        // }
        // else
        // {
        //     yield return StartCoroutine(LoadUserDataFromFirebase());
        // }

        yield return null;
    }

    private IEnumerator LoadUserDataFromFirebase()
    {
        // Load coin amount from Firebase

        // var coinAmountTask = FirebaseDatabase.DefaultInstance
        //     .GetReference("Users/" + GetUserID() + "/CoinAmount")
        //     .GetValueAsync();
        //
        // yield return new WaitUntil(() => coinAmountTask.IsCompleted);
        //
        // if (coinAmountTask.Exception != null)
        // {
        //     Debug.LogError("Failed to load coin amount: " + coinAmountTask.Exception);
        //     OnLoadFailed();
        //     yield break;
        // }
        //
        // DataSnapshot coinAmountSnapshot = coinAmountTask.Result;
        // int cloudCoinAmount = Convert.ToInt32(coinAmountSnapshot.Value);
        // ApplyCoinAmount(cloudCoinAmount);
        //
        // // Load overall purchases from Firebase
        // var purchasesTask = FirebaseDatabase.DefaultInstance
        //     .GetReference("Users/" + GetUserID() + "/Purchases")
        //     .GetValueAsync();
        //
        // yield return new WaitUntil(() => purchasesTask.IsCompleted);
        //
        // if (purchasesTask.Exception != null)
        // {
        //     Debug.LogError("Failed to load purchases: " + purchasesTask.Exception);
        //     OnLoadFailed();
        //     yield break;
        // }
        //
        // DataSnapshot purchasesSnapshot = purchasesTask.Result;
        // ProcessPurchasesData(purchasesSnapshot);
        //
        // loaderUI.AddProgress(50);

        SceneManager.sceneLoaded += OnLoadData;
        SceneManager.LoadScene("MainMenu");
        yield return null;
    }

    private void ApplyMapStyle(string style)
    {
        // Apply map style logic
    }

    private void ApplyCharacterChoice(string character)
    {
        // Apply character choice logic
    }

    private void ApplyCoinAmount(int amount)
    {
        // Apply the coin amount to the game
    }

    // private void ProcessPurchasesData(DataSnapshot snapshot)
    // {
    //     // Process purchase data from Firebase
    // }

    private string GetUserID()
    {
        // Logic to retrieve the current user's ID (for Firebase reference)
        return "user123"; // Replace with actual user ID retrieval logic
    }

    private void OnLoadData(Scene scene, LoadSceneMode mode)
    {
        print("called onload data");
        SceneManager.sceneLoaded -= OnLoadData;
        loaderUI.AddProgress(30);
        OnLoadComplete();
    }

    private void OnLoadComplete()
    {
        print("finished loading");
        Destroy(loaderUI.transform.root.gameObject);
        Destroy(gameObject);
    }

    private void OnLoadFailed()
    {
        // Handle load failure, e.g., show a retry button or return to the main menu
        Debug.LogError("Load process failed.");
    }
}

}