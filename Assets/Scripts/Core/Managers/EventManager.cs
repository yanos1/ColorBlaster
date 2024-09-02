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
    }
}