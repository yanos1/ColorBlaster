using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        private float time;
        private float ChangeDifficultyInterval = 18f;


        private void Start()
        {
            time = Time.time;
        }

        private void Update()
        {
            if (Time.time > time + ChangeDifficultyInterval)
            {
                // CoreManager.instance.EventManager.InvokeEvent(EventNames.IncreaseGameDifficulty, null);
            }
        }
    }
}