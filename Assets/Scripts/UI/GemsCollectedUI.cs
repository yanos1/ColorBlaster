using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Extentions;
using GameLogic.ConsumablesGeneration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GemsCollectedUI : MonoBehaviour
    {


        public Image GemsCollectedIcon => gemsImage;
        
        [SerializeField] private TextMeshProUGUI gemsCollectedText;
        [SerializeField] private Image gemsImage;
        [SerializeField] private float scaleDuration = 0.2f; // Duration for scale animation
        [SerializeField] private Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.2f);
        private Vector3 originalScale = new Vector3(1, 1, 1);
        private Coroutine scaleCoroutine;
        private List<Color> gemColorsCollected = new();

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.GemPrefabArrived, UpdateGemUI);
            CoreManager.instance.EventManager.AddListener(EventNames.GameOverPanelActive, OnGameOver);
        }


        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.GemPrefabArrived, UpdateGemUI);
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOverPanelActive, OnGameOver);
        }
        
        
        private void OnGameOver(object obj)
        {
            CoreManager.instance.EventManager.InvokeEvent(EventNames.BroadcastGemsPicked, gemColorsCollected);
        }

        private void UpdateGemUI(object obj)
        {
            // Increment gems collected count
            if (scaleCoroutine is not null)
            {
                StopCoroutine(scaleCoroutine);
                scaleCoroutine = null;
            }

            scaleCoroutine = StartCoroutine(UpdateUIOverTime());
            gemsCollectedText.text = (int.Parse(gemsCollectedText.text) + 1).ToString();

            // Scale the image with a quick bounce effect

            // Multiply the image color by the color passed in the object
            if (obj is (Color gemColor, float duration, TreasureChestBuff buff))
            {
                gemsImage.color += gemColor;
                gemColorsCollected.Add(gemColor);  // add gem color for later use (for transfering coins to the wallet)

            }
            
        }

        private IEnumerator UpdateUIOverTime()
        {
            yield return StartCoroutine(UtilityFunctions.ScaleObjectOverTime(gemsImage.gameObject, targetScale,scaleDuration));
            yield return StartCoroutine(UtilityFunctions.ScaleObjectOverTime(gemsImage.gameObject, originalScale,scaleDuration));

        }
        
    }
}
