using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Database;
using Firebase.Extensions;
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

        // Firebase initialization
        public void InitializeFirebase()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    InitializeDatabase();  // Initialize database after Firebase is fully initialized
                    RetrieveDataFromFirebase(_id); // Now safe to retrieve data from Firebase
                }
                else
                {
                    Debug.LogError($"Firebase initialization failed: {task.Exception?.Message}");
                }
            });
        }

        // Initialize Firebase database and reference
        private void InitializeDatabase()
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseDatabase database = FirebaseDatabase.GetInstance(app, "https://colorblaster-8fe62-default-rtdb.europe-west1.firebasedatabase.app/");
            _dbReference = database.RootReference;

            // Set the user reference, replace _id with the actual user id logic you have
            userRef = _dbReference.Child("users").Child(_id);
            Debug.Log($"User reference: {userRef}");
        }

        // Retrieve data from Firebase
        private void RetrieveDataFromFirebase(string userId)
        {
            Debug.Log("Start getting data...");
            userRef.GetValueAsync().ContinueWithOnMainThread(task =>
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
                        // Gems owned
                        gemsOwned = int.Parse(snapshot.Child("gemsOwned").Value.ToString());
                        Debug.Log($"Gems in balance: {gemsOwned}");

                        // Styles owned
                        stylesOwned = new List<string>();
                        foreach (DataSnapshot styleSnapshot in snapshot.Child("stylesOwned").Children)
                        {
                            stylesOwned.Add(styleSnapshot.Value.ToString());
                        }

                        // Colors owned
                        colorsOwned = new List<string>();
                        foreach (DataSnapshot colorThemeSnapShot in snapshot.Child("colorThemesOwned").Children)
                        {
                            colorsOwned.Add(colorThemeSnapShot.Value.ToString());
                        }

                        // Item purchases
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
            userRef.Child("stylesOwned").SetValueAsync(new List<string> { StyleName.Pastel.ToString() }); // Default styles
            userRef.Child("colorThemesOwned").SetValueAsync(new List<string> { "Default" }); // Default color themes
            userRef.Child("inGamePurchases").SetValueAsync(new Dictionary<string, int>()); // Empty purchases

            Debug.Log("New user data initialized.");
        }

        public void SetNewHighScore(int newHighScore)
        {
            Debug.Log("New high score set!");
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

        public void AddGems(int amount)
        {
            gemsOwned = Mathf.Max(0, gemsOwned + amount);
            userRef.Child("gemsOwned").SetValueAsync(gemsOwned);
            Debug.Log($"Added {amount} gems. New total: {gemsOwned}");
        }

        public void AddStyle(string style)
        {
            if (!stylesOwned.Contains(style))
            {
                stylesOwned.Add(style);
                userRef.Child("stylesOwned").SetValueAsync(stylesOwned);
                Debug.Log($"Added new style: {style}");
            }
        }

        public void AddColorTheme(string colorTheme)
        {
            if (!colorsOwned.Contains(colorTheme))
            {
                colorsOwned.Add(colorTheme);
                userRef.Child("colorThemesOwned").SetValueAsync(colorsOwned);
                Debug.Log($"Added new color theme: {colorTheme}");
            }
        }

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
                await userRef.Child("gemsOwned").SetValueAsync(currentGems);
            }
            else
            {
                Debug.Log("gemsOwned does not exist.");
            }
        }


        public enum Boosters
        {
            None = 0,
            Shield = 101,
            ColorBlaster = 102,
            
        }

        public enum ColorThemes
        {
            None = 0,
            Default = 101,
            Cyber = 102,
            
        }

        public enum Styles {
            None = 0,
            Default = 101,
            Pastel = 102,
            Electric = 103,
            
            
        }

        public enum Avatars
        {
            None = 0,
            Default = 101,
            Turkey = 102,
            
        }
        
    }
}
