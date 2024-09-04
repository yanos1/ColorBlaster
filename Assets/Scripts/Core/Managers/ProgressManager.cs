using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Core.Managers
{
    public class ProgressManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI distanceUI;
        private int _currentDistanceTraveled;
        private int _bestDistanceTraveled;

        public void Start()
        {
            _currentDistanceTraveled = 0;
            // bestDistanceTraveled = GetFromDataBase();
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame,StartMeasuringDistance);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving,StartMeasuringDistance);
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun,StopMeasuring);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame,StartMeasuringDistance);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving,StartMeasuringDistance);
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun,StopMeasuring);

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
                yield return new WaitForSeconds(0.1f);
            }
        }
        
    }
}