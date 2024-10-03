using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Database;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Managers
{
    public class UserDataManager
    {
        public int GemsOwned => gemsOwned;
        public List<string> StylesOwned => stylesOwned;
        public List<string> ColorsOwned => colorsOwned;

        private DatabaseReference _dbReference;
        private DatabaseReference userRef;
        private string _id;

        private int gemsOwned;
        private List<string> stylesOwned;
        private List<string> colorsOwned;
        private Dictionary<string, int> itemPurchases; // item name -> quantity purchased

        public UserDataManager(string id)
        {
            stylesOwned = new List<string>();
            colorsOwned = new List<string>();
            itemPurchases = new Dictionary<string, int>();
            _id = id;
            InitializeFirebase();
        }

        // Retrieve data from Firebase if it exists

        // Firebase initialization
        public async void InitializeFirebase()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                // Firebase is initialized, now you can initialize the database and user data manager
                InitializeDatabase();
                RetrieveDataFromFirebase(_id); // Now Firebase is initialized, you can call Firebase functions.
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                // Handle error (e.g., retry, show error to user, etc.)
            }
        }

        // Initialize Firebase database and reference
        private void InitializeDatabase()
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseDatabase database = FirebaseDatabase.GetInstance(app, "https://colorblaster-8fe62-default-rtdb.europe-west1.firebasedatabase.app/");
            _dbReference = database.RootReference;

            // Set the user reference, replace _id with the actual user id logic you have
            userRef = _dbReference.Child("users").Child(_id);
            Debug.Log($"user ref : {userRef.ToString()}");
            Debug.Log($"id : {_id}");
        }

        // Retrieve data from Firebase
        private void RetrieveDataFromFirebase(string userId)
        {
            Debug.Log("start getting data");
            userRef.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"Error retrieving data: {task.Exception}");
                    return;
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        gemsOwned = int.Parse(snapshot.Child("gemsOwned").Value.ToString());
                        Debug.Log($"gems in balance: {gemsOwned}");

                        stylesOwned = new List<string>();
                        foreach (DataSnapshot styleSnapshot in snapshot.Child("stylesOwned").Children)
                        {
                            stylesOwned.Add(styleSnapshot.Value.ToString());
                        }

                        colorsOwned = new List<string>();
                        foreach (DataSnapshot colorThemeSnapShot in snapshot.Child("colorThemesOwned").Children)
                        {
                            colorsOwned.Add(colorThemeSnapShot.Value.ToString());
                        }

                        itemPurchases = new Dictionary<string, int>();
                        foreach (DataSnapshot purchaseSnapshot in snapshot.Child("inGamePurchases").Children)
                        {
                            string item = purchaseSnapshot.Key;
                            int quantity = int.Parse(purchaseSnapshot.Value.ToString());
                            itemPurchases[item] = quantity;
                        }

                        Debug.Log("User data retrieved successfully.");
                    }
                    else
                    {
                        Debug.Log("No data found, initializing new user data.");
                        InitializeUserData(userId);
                    }
                }
            });
        }


        // Initialize default data for a new user
        private void InitializeUserData(string userId)
        {
            userRef.Child("highScore").SetValueAsync(0);
            userRef.Child("coinAmount").SetValueAsync(500); // Default coin amount
            userRef.Child("stylesOwned")
                .SetValueAsync(new List<string> { StyleName.Pastel.ToString() }); // Default styles
            userRef.Child("colorThemesOwned").SetValueAsync(new List<string>(){"Default"}); // Empty color themes
            userRef.Child("inGamePurchases").SetValueAsync(new Dictionary<string, int>()); // Empty purchases

            Debug.Log("New user data initialized.");
        }

        public void SetNewHighScore(int newHighScore)
        {
            Debug.Log("new high score set!");
            userRef.Child("highScore").SetValueAsync(newHighScore);
        }

        public async Task<int> GetHighScore()
        {
            var highScoreSnapshot = await userRef.Child("highScore").GetValueAsync();
            if (highScoreSnapshot.Exists)
            {
                return int.Parse(highScoreSnapshot.Value.ToString());
            }

            return 0;
        }

        // Update coins and sync to Firebase
        public void AddGems(int amount)
        {
            gemsOwned = Mathf.Max(0, gemsOwned + amount);
            userRef.Child("gemsOwned").SetValueAsync(gemsOwned);
            Debug.Log($"Added {amount} coins. New total: {gemsOwned}");
        }

        // Add a new style and sync to Firebase
        public void AddStyle(string style)
        {
            if (!stylesOwned.Contains(style))
            {
                stylesOwned.Add(style);
                userRef.Child("stylesOwned").SetValueAsync(stylesOwned);
                Debug.Log($"Added new style: {style}");
            }
        }

        // Add a new color theme and sync to Firebase
        public void AddColorTheme(string colorTheme)
        {
            if (!colorsOwned.Contains(colorTheme))
            {
                colorsOwned.Add(colorTheme);
                userRef.Child("colorThemesOwned").SetValueAsync(colorsOwned);
                Debug.Log($"Added new color theme: {colorTheme}");
            }
        }

        // Add item purchase and sync to Firebase
        public void AddItemPurchase(string itemName, int quantity)
        {
            if (itemPurchases.ContainsKey(itemName))
            {
                itemPurchases[itemName] += quantity;
            }
            else
            {
                itemPurchases[itemName] = quantity;
            }

            userRef.Child("inGamePurchases").SetValueAsync(itemPurchases);
            Debug.Log($"Added {quantity} of {itemName}. New total: {itemPurchases[itemName]}");
        }

        public async void UpdateGemsOwned(int gemsCollected)
        {
            var snapshot = await userRef.Child("gemsOwned").GetValueAsync();
            if (snapshot.Exists)
            {
                int currentGems = Convert.ToInt32(snapshot.Value) + gemsCollected;
                await userRef.Child("gemsOwned").SetValueAsync(currentGems);  // Sets gemsOwned to its current value
            }
            else
            {
                Debug.Log("gemsOwned does not exist.");
            }
        }

    }
}