using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.GameData;
using Firebase;
using Firebase.Analytics;
using Firebase.Database;
using Firebase.Extensions;
using UnityEditor;
using UnityEngine;

namespace Core.Managers
{
    public class UserDataManager
    {
        public int GemsOwned => gemsOwned;
        public Dictionary<FirebasePath, Dictionary<Item, bool>> ItemsOwned => itemsOwned;
        public Dictionary<Item, int> BoostersOwned => boostersOwned;

        private DatabaseReference _dbReference;
        private DatabaseReference userRef;
        private string _id;

        private int gemsOwned;
        private Dictionary<Item, int> boostersOwned;
        private Dictionary<FirebasePath, Dictionary<Item, bool>> itemsOwned;

        public UserDataManager(string id)
        {
            boostersOwned = new Dictionary<Item, int>();
            itemsOwned = new Dictionary<FirebasePath, Dictionary<Item, bool>>();
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
                    InitializeDatabase();
                    RetrieveDataFromFirebase(_id);
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
            FirebaseDatabase database = FirebaseDatabase.GetInstance(app,
                "https://colorblaster-8fe62-default-rtdb.europe-west1.firebasedatabase.app/");
            _dbReference = database.RootReference;

            // Set the user reference
            userRef = _dbReference.Child("users").Child(_id);
        }

        // Retrieve data from Firebase
        private void RetrieveDataFromFirebase(string userId)
        {
            userRef.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Error retrieving data: {task.Exception}");
                    return;
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        // Retrieve owned data
                        gemsOwned = int.Parse(snapshot.Child(FirebasePath.gemsOwned.ToString()).Value.ToString());

                        // Retrieve all dictionaries of items in a loop
                        foreach (FirebasePath path in Enum.GetValues(typeof(FirebasePath)))
                        {
                            if (path != FirebasePath.gemsOwned && path != FirebasePath.boostersOwned &&
                                path != FirebasePath.highScore)
                            {
                                var dict = new Dictionary<Item, bool>();
                                GetDataOfType(snapshot, path, dict);
                                itemsOwned[path] = dict;
                            }
                        }

                        // Retrieve boosters
                        GetBoosterData(snapshot);
                    }
                    else
                    {
                        InitializeUserData(userId);
                    }
                }
            });
        }

        // Retrieve dictionary data for styles, color themes, avatars
        private void GetDataOfType(DataSnapshot snapshot, FirebasePath path, Dictionary<Item, bool> dict)
        {
            foreach (DataSnapshot data in snapshot.Child(path.ToString()).Children)
            {
                if (Enum.TryParse(data.Key, out Item item) && bool.TryParse(data.Value.ToString(), out bool isEquipped))
                {
                    dict[item] = isEquipped;
                }
            }
        }

        // Retrieve boosters
        private void GetBoosterData(DataSnapshot snapshot)
        {
            foreach (DataSnapshot boosterSnapshot in snapshot.Child(FirebasePath.boostersOwned.ToString()).Children)
            {
                if (Enum.TryParse(boosterSnapshot.Key, out Item booster))
                {
                    boostersOwned[booster] = int.Parse(boosterSnapshot.Value.ToString());
                }
            }
        }

        // Initialize default data for a new user
        private void InitializeUserData(string userId)
        {
            userRef.Child(FirebasePath.gemsOwned.ToString()).SetValueAsync(0);
            userRef.Child(FirebasePath.stylesOwned.ToString()).SetValueAsync(new Dictionary<int, bool>
                { { (int)Item.DefaultStyle, true } });
            userRef.Child(FirebasePath.colorThemesOwned.ToString()).SetValueAsync(new Dictionary<int, bool>
                { { (int)Item.DefaultColorTheme, true } });
            userRef.Child(FirebasePath.avatarsOwned.ToString()).SetValueAsync(new Dictionary<int, bool>
                { { (int)Item.DefaultAvatar, true } });
            userRef.Child(FirebasePath.boostersOwned.ToString()).SetValueAsync(new Dictionary<int, int>
                { { (int)Item.ShieldBooster, 0 } });
        }

        public bool IsItemEquipped(Item item, FirebasePath firebasePath)
        {
            foreach (var (path, dictionary) in itemsOwned)
            {
                if (path == firebasePath)
                {
                    if (!dictionary.ContainsKey(item))
                    {
                        return false;
                    }

                    return dictionary[item];
                }
            }

            return false;
        }

        // Equip items  can be optimised !
        public void EquipItem(Item item, FirebasePath path)
        {
            if (!itemsOwned.ContainsKey(path))
            {
                Debug.LogError($"Path {path} not found in itemsOwned dictionary.");
                return;
            }

            var dict = itemsOwned[path];

            foreach (var key in dict.Keys)
            {
                dict[key] = false; // Unequip all items
            }

            dict[item] = true; // Equip the new item

            // Prepare data for Firebase
            var firebaseData = new Dictionary<int, bool>();
            foreach (var entry in dict)
            {
                firebaseData[(int)entry.Key] = entry.Value;
            }

            userRef.Child(path.ToString()).SetValueAsync(firebaseData);
        }

        public void EquipStyle(Item style) => EquipItem(style, FirebasePath.stylesOwned);

        public void EquipColorTheme(Item colorTheme) => EquipItem(colorTheme, FirebasePath.colorThemesOwned);

        public void EquipAvatar(Item avatar) => EquipItem(avatar, FirebasePath.avatarsOwned);

        // Add items
        public void AddItem(Item item, FirebasePath firebasePath)
        {
            if (!itemsOwned.ContainsKey(firebasePath))
            {
                Debug.LogError($"Path {firebasePath} not found in itemsOwned dictionary.");
                return;
            }

            var dict = itemsOwned[firebasePath];
            AddItemToDataBase(item, firebasePath, dict);
        }

        private void AddItemToDataBase(Item item, FirebasePath path, Dictionary<Item, bool> dict)
        {
            if (dict.TryAdd(item, false)) // Add item to the local dictionary if it doesn't already exist
            {
                var newItem = new Dictionary<string, object>
                {
                    { ((int)item).ToString(), false }
                };

                userRef.Child(path.ToString()).UpdateChildrenAsync(newItem).ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"Successfully added {item} to Firebase under {path}.");
                    }
                    else
                    {
                        Debug.LogError($"Failed to add {item} to Firebase: {task.Exception}");
                    }
                });
            }
        }

        public void AddStyle(Item style) => AddItem(style, FirebasePath.stylesOwned);

        public void AddColorTheme(Item colorTheme) => AddItem(colorTheme, FirebasePath.colorThemesOwned);

        public void AddAvatar(Item avatar) => AddItem(avatar, FirebasePath.avatarsOwned);

        // Add boosters
        public void AddBooster(Item booster, int quantity)
        {
            if (boostersOwned.ContainsKey(booster))
            {
                boostersOwned[booster] += quantity;
            }
            else
            {
                boostersOwned[booster] = quantity;
            }

            userRef.Child(FirebasePath.boostersOwned.ToString()).Child(((int)booster).ToString())
                .SetValueAsync(boostersOwned[booster]);
        }

        // Add gems
        public void AddGems(int amount)
        {
            gemsOwned = Mathf.Max(0, gemsOwned + amount);
            userRef.Child(FirebasePath.gemsOwned.ToString()).SetValueAsync(gemsOwned);
        }

        // High score
        public async Task<int> GetHighScore()
        {
            var snapshot = await userRef.Child(FirebasePath.highScore.ToString()).GetValueAsync();
            return snapshot.Exists ? int.Parse(snapshot.Value.ToString()) : 0;
        }

        public void SetNewHighScore(int newScore)
        {
             userRef.Child(FirebasePath.highScore.ToString()).SetValueAsync(newScore);
        }

        public bool HasItem(Item itemType, FirebasePath avatarFirebasePath)
        {
            return itemsOwned[avatarFirebasePath].ContainsKey(itemType);
        }
    }

    public enum FirebasePath
    {
        stylesOwned,
        gemsOwned,
        colorThemesOwned,
        avatarsOwned,
        boostersOwned,
        highScore
    }
}