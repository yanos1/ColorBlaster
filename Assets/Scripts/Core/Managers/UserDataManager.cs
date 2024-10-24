﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private bool finishedLoading;

        public UserDataManager(string id)
        {
            boostersOwned = new Dictionary<Item, int>();
            itemsOwned = new Dictionary<FirebasePath, Dictionary<Item, bool>>();
            _id = id;
            finishedLoading = false;
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
                    Debug.Log("Starting To Load DB DATA !");
                    DataSnapshot snapshot = task.Result;
                    Debug.Log($"111 {task.Result.Exists}");
                    if (!snapshot.Exists)
                    {
                        Debug.Log("call new user");
                        InitializeUserData();
                        return;
                    }

                    Debug.Log("load data into data structures");
                    AssignDataLocally(snapshot);
                    Debug.Log("FINISHED LOADING DB DATA !");
                }
            });
        }

        private void AssignDataLocally(DataSnapshot snapshot)
        {
            gemsOwned = int.Parse(snapshot.Child(FirebasePath.gemsOwned.ToString()).Value.ToString());

            // Retrieve all dictionaries of items in a loop
            foreach (FirebasePath path in new[]
                         { FirebasePath.avatarsOwned, FirebasePath.stylesOwned, FirebasePath.colorThemesOwned })
            {
                Debug.Log($"LOADING {path.ToString()}");
                var dict = new Dictionary<Item, bool>();
                GetDataOfType(snapshot, path, dict);
                itemsOwned[path] = dict;
            }

            // Retrieve boosters
            GetBoosterData(snapshot);
            Debug.Log("DONE !");
            finishedLoading = true;
        }


        // Retrieve dictionary data for styles, color themes, avatars
        private void GetDataOfType(DataSnapshot snapshot, FirebasePath path, Dictionary<Item, bool> dict)
        {
            foreach (DataSnapshot data in snapshot.Child(path.ToString()).Children)
            {
                Debug.Log($"trying to load {data} 111 ");
                if (Enum.TryParse(data.Key, out Item item) && bool.TryParse(data.Value.ToString(), out bool isEquipped))
                {
                    Debug.Log($"{item.ToString()} found 111");
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
                    Debug.Log($"owned booster : {booster} amount: {boosterSnapshot.Value}");
                    boostersOwned[booster] = int.Parse(boosterSnapshot.Value.ToString());
                }
            }
        }

        // Initialize default data for a new user
        // Initialize default data for a new user safely
        private async void InitializeUserData()
        {
            Debug.Log("Initializing new user data...");

            // Create tasks for each set operation
            // Use Task.WhenAll to wait for all tasks to complete
            await Task.WhenAll(
                userRef.Child(FirebasePath.stylesOwned.ToString()).SetValueAsync(new Dictionary<string, object>
                {
                    { ((int)Item.DefaultStyle).ToString(), true }
                }),
                userRef.Child(FirebasePath.gemsOwned.ToString()).SetValueAsync(100),
                userRef.Child(FirebasePath.colorThemesOwned.ToString()).SetValueAsync(new Dictionary<string, object>
                {
                    { ((int)Item.DefaultColorTheme).ToString(), true }
                }),
                userRef.Child(FirebasePath.avatarsOwned.ToString()).SetValueAsync(new Dictionary<string, object>
                {
                    { ((int)Item.DefaultAvatar).ToString(), true }
                }),
                userRef.Child(FirebasePath.boostersOwned.ToString()).SetValueAsync(new Dictionary<string, object>
                {
                    { ((int)Item.ShieldBooster).ToString(), 0 }, { ((int)Item.ColorBlasterBooster).ToString(), 0 },
                    { ((int)Item.GunnersBooster).ToString(), 0 }
                })
            );

            // Get the updated user data after all set operations are complete
            DataSnapshot snapshot = await userRef.GetValueAsync();

            if (snapshot.Exists)
            {
                AssignDataLocally(snapshot);
            }
            else
            {
                Debug.Log("Error: User data not found.");
            }
        }


        public Item GetEquippedItem(FirebasePath firebasePath)
        {
            foreach (var (path, dictionary) in itemsOwned)
            {
                if (path == firebasePath)
                {
                    foreach (var v in dictionary)
                    {
                        Debug.Log($"item : {v.Key}  eqipped: {v.Value}");
                    }

                    return dictionary.FirstOrDefault(kvp => kvp.Value).Key;
                }
            }

            Debug.Log("ITEM WASNT FOUND !! FIX DATA BASE DATA");
            return Item.None;
        }

        public bool IsItemEquipped(Item item, FirebasePath firebasePath)
        {
            Debug.Log("ISITEM CALLED");
            foreach (var (path, dictionary) in itemsOwned)
            {
                if (path == firebasePath)
                {
                    foreach (var v in dictionary)
                    {
                        Debug.Log($"item : {v.Key}  eqipped: {v.Value}");
                    }

                    bool owned = dictionary.GetValueOrDefault(item, false);
                    Debug.Log(owned);
                    return owned;
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

            var newDict = new Dictionary<int, object>();

            foreach (var key in itemsOwned[path].Keys)
            {
                newDict[(int)key] = false; // Unequip all items
            }

            newDict[(int)item] = true; // Equip the new item

            userRef.Child(path.ToString()).SetValueAsync(newDict).ContinueWithOnMainThread(task =>
            {
                itemsOwned[path] = newDict.ToDictionary(k => (Item)k.Key, v => (bool)v.Value);
            });
        }

        public void EquipStyle(Item style) => EquipItem(style, FirebasePath.stylesOwned);

        public void EquipColorTheme(Item colorTheme) => EquipItem(colorTheme, FirebasePath.colorThemesOwned);

        public void EquipAvatar(Item avatar) => EquipItem(avatar, FirebasePath.avatarsOwned);

        // Add items
        public async Task AddItem(Item item, FirebasePath firebasePath)
        {
            if (!itemsOwned.ContainsKey(firebasePath))
            {
                Debug.LogError($"Path {firebasePath} not found in itemsOwned dictionary.");
                return;
            }

            var dict = itemsOwned[firebasePath];
            await AddItemToDataBase(item, firebasePath, dict);
        }

        private async Task AddItemToDataBase(Item item, FirebasePath path, Dictionary<Item, bool> dict)
        {
            if (dict.TryAdd(item, false)) // Add item to the local dictionary if it doesn't already exist
            {
                var newItem = new Dictionary<string, object>
                {
                    { ((int)item).ToString(), false }
                };

                await userRef.Child(path.ToString()).UpdateChildrenAsync(newItem).ContinueWith(task =>
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

        // public void AddStyle(Item style) => AddItem(style, FirebasePath.stylesOwned);
        //
        // public void AddColorTheme(Item colorTheme) => AddItem(colorTheme, FirebasePath.colorThemesOwned);
        //
        // public void AddAvatar(Item avatar) => AddItem(avatar, FirebasePath.avatarsOwned);

        // Add boosters
        public void AddBooster(Item booster, int quantity, Action onComplete)
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
                .SetValueAsync(boostersOwned[booster]).ContinueWithOnMainThread(task => { onComplete?.Invoke(); });
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
            Debug.Log($"{itemType} is : {itemsOwned[avatarFirebasePath].ContainsKey(itemType)}");
            return itemsOwned[avatarFirebasePath].ContainsKey(itemType);
        }

        public int GetBoosterAmount(Item booster, FirebasePath path)
        {
            return boostersOwned[booster];
        }

        public bool FinishedLoading()
        {
            return finishedLoading;
        }

        public void UseBooster(Item boosterType, Action onComplete = null)
        {
            userRef.Child(FirebasePath.boostersOwned.ToString()).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot boosterData = task.Result;
                    if (boosterData.Exists)
                    {
                        Dictionary<string, object> updates = new Dictionary<string, object>();

                        foreach (var booster in boosterData.Children)
                        {
                            Debug.Log($"key {booster.Key} type given {boosterType.ToString()}");
                            if (booster.Key == ((int)boosterType).ToString())
                            {
                                int currentBoosterCount = Convert.ToInt32(booster.Value);

                                // Decrease the booster count
                                int newBoosterCount = currentBoosterCount - 1;
                                boostersOwned[boosterType] -= 1;

                                // Prepare the path and the new value for the booster.
                                updates[booster.Key] = newBoosterCount;
                                Debug.Log("UPDATE UI 222");
                                onComplete?.Invoke();
                            }
                        }

                        // Update Firebase with the new booster count.
                        if (updates.Count > 0)
                        {
                            userRef.Child(FirebasePath.boostersOwned.ToString()).UpdateChildrenAsync(updates);
                        }
                    }
                }
            });
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
}