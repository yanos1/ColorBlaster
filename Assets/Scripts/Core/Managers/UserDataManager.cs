using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Managers
{
    public class UserDataManager
    {
        public int Coins => coins;
        public List<string> StylesOwned => stylesOwned;
        public List<string> ColorsOwned => colorsOwned;

        private DatabaseReference _dbReference;
        private DatabaseReference userRef;
        private string _id;

        private int coins;
        private List<string> stylesOwned;
        private List<string> colorsOwned;
        private Dictionary<string, int> itemPurchases; // item name -> quantity purchased

        public UserDataManager(string id)
        {
            stylesOwned = new List<string>();
            colorsOwned = new List<string>();
            itemPurchases = new Dictionary<string, int>();

            _id = id;
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseDatabase database = FirebaseDatabase.GetInstance(app,
                "https://colorblaster-8fe62-default-rtdb.europe-west1.firebasedatabase.app/");
            _dbReference = database.RootReference;

            userRef = _dbReference.Child("users").Child(_id);
            Debug.Log($"user ref : {userRef.ToString()}");
            Debug.Log($"id : {_id}");

            RetrieveDataFromFirebase(_id);
        }

        // Retrieve data from Firebase if it exists
        private void RetrieveDataFromFirebase(string userId)
        {
            userRef.GetValueAsync().ContinueWith(task =>
            {

                if (task.IsFaulted)
                {
                    // Handle any errors here
                    Debug.LogError($"Error retrieving data: {task.Exception}");
                    return;
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Exists)
                    {
                        // Retrieve coins
                        coins = int.Parse(snapshot.Child("coinAmount").Value.ToString());

                        // Retrieve styles owned
                        stylesOwned.Clear();
                        foreach (DataSnapshot styleSnapshot in snapshot.Child("stylesOwned").Children)
                        {
                            stylesOwned.Add(styleSnapshot.Value.ToString());
                        }

                        // Retrieve colors owned
                        colorsOwned.Clear();
                        foreach (DataSnapshot colorThemeSnapShot in snapshot.Child("colorThemesOwned").Children)
                        {
                            colorsOwned.Add(colorThemeSnapShot.Value.ToString());
                        }

                        // Retrieve item purchases
                        itemPurchases.Clear();
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
                        // No data found for this user, initialize data for them
                        Debug.Log("No data found, initializing new user data.");
                        InitializeUserData(userId);
                    }
                }
                else
                {
                    Debug.Log("Task not completed successfully.");
                }
            });
        }


        // Initialize default data for a new user
        private void InitializeUserData(string userId)
        {
            userRef.Child("coinAmount").SetValueAsync(500); // Default coin amount
            userRef.Child("stylesOwned")
                .SetValueAsync(new List<string> { StyleName.Pastel.ToString() }); // Default styles
            userRef.Child("colorThemesOwned").SetValueAsync(new List<string>(){"Default"}); // Empty color themes
            userRef.Child("inGamePurchases").SetValueAsync(new Dictionary<string, int>()); // Empty purchases

            Debug.Log("New user data initialized.");
        }

        // Update coins and sync to Firebase
        public void AddCoins(int amount)
        {
            coins = Mathf.Max(0, coins + amount);
            userRef.Child("coinAmount").SetValueAsync(coins);
            Debug.Log($"Added {amount} coins. New total: {coins}");
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
    }
}