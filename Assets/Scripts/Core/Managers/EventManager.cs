using UnityEngine;

namespace Core.Managers
{
    using System;
    using System.Collections.Generic;


    public class EventManager
    {
        private Dictionary<EventNames, List<Action<object>>> _activeListeners = new();

        public void AddListener(EventNames eventName, Action<object> listener)
        {
            if (_activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Add(listener);

                return;
            }

            _activeListeners.Add(eventName, new List<Action<object>> { listener });
        }

        public void RemoveListener(EventNames eventName, Action<object> listener)
        {
            if (_activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Remove(listener);

                if (listOfEvents.Count <= 0)
                {
                    _activeListeners.Remove(eventName);
                }
            }
        }

        public void InvokeEvent(EventNames eventName, object obj)
        {

            if (_activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                for (int i = 0; i < listOfEvents.Count; i++)
                {
                    listOfEvents[i].Invoke(obj);
                }
            }
        }
    }

    public enum EventNames
    {
        None = 0,
        SetStyle = 1,
        Shoot = 2,
        Move = 3,
        IncreaseGameDifficulty = 4,
        StartGame = 5,
        GameOver = 6,
        RestartGame =7,
        EndRun = 8,
        Revive = 9,
        AddCurrency = 10,
        KillPlayer =11,
        FinishedReviving = 12,
        GemPrefabArrived =13,
        ActivateColorRush=14,
        BroadcastGemsPicked = 15,
        GameOverPanelActive = 16,
        DeactivateColorRush = 17,
        ActivateShield = 18,
        DeactivateShield = 19,
        ColorRushPrefabArrived=20,
        ShieldPrefabArrived=21,
        ActivateGemRush =22,
        DeactivateGemRush=23,
        ActivateDeleteColor=24,
        DeactivateDeleteColor=25,
        DeactivateColorPrefabArrived=25,
        
        // need to check for prefab arrival events


        DeleteColorPrefabArrived=26,
        UpdateObjectMovespeed=27,
    }
}