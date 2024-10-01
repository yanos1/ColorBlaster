using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Managers
{
    public class ProgressManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI distanceUI;
        [SerializeField] private TextMeshProUGUI highScoreUI;
        private int _currentGemsPicked;
        private int _currentDistanceTraveled;
        private int _bestDistanceTraveled;
        private float _timeBetweenUpdates = 0.17f;

        public void Start()
        {
            _currentDistanceTraveled = 0;
            _currentGemsPicked = 0;
            InitializeHighScore();
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, StartMeasuringDistance);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, StartMeasuringDistance);
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, StopMeasuring);
            CoreManager.instance.EventManager.AddListener(EventNames.GemPrefabArrived, AddCoin);
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, UpdateStatsIfNeeded);
            CoreManager.instance.EventManager.AddListener(EventNames.LevelUp, DecreaseTimeBetweenUpdates);
        }


        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, StartMeasuringDistance);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, StartMeasuringDistance);
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, StopMeasuring);
            CoreManager.instance.EventManager.RemoveListener(EventNames.GemPrefabArrived, AddCoin);
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, UpdateStatsIfNeeded);
            CoreManager.instance.EventManager.RemoveListener(EventNames.LevelUp, DecreaseTimeBetweenUpdates);

        }

        private void DecreaseTimeBetweenUpdates(object obj)
        {
            _timeBetweenUpdates = Mathf.Max(_timeBetweenUpdates-0.01f, 0.05f);
        }

        private async void InitializeHighScore()
        {
            _bestDistanceTraveled = await CoreManager.instance.UserDataManager.GetHighScore();
            highScoreUI.text = "best: " + _bestDistanceTraveled;

        }

        private void UpdateStatsIfNeeded(object obj)
        {
            if (_bestDistanceTraveled < _currentDistanceTraveled)
            {
                CoreManager.instance.UserDataManager.SetNewHighScore(_currentDistanceTraveled);
                highScoreUI.text = "best: " + _currentDistanceTraveled;
                _currentDistanceTraveled = 0;
            }

            CoreManager.instance.UserDataManager.UpdateGemsOwned(_currentGemsPicked);
        }

        public int GetCoins()
        {
            return _currentGemsPicked;
        }

        private void AddCoin(object obj)
        {
            _currentGemsPicked++;
        }

        private void StartMeasuringDistance(object obj)
        {
            StartCoroutine(SelfUpdate());
        }

        private void StopMeasuring(object obj)
        {
            StopAllCoroutines();
        }

        public IEnumerator SelfUpdate()
        {
            while (CoreManager.instance.GameManager.IsGameActive)
            {
                _currentDistanceTraveled += 1;
                distanceUI.text = _currentDistanceTraveled + "m";
                yield return new WaitForSeconds(_timeBetweenUpdates);
            }
        }
    }
}