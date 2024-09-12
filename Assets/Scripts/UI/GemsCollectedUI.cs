using System;
using System.Collections;
using Core.Managers;
using Extentions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GemsCollectedUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gemsCollectedText;
        [SerializeField] private Image gemsImage;
        [SerializeField] private float scaleDuration = 0.2f; // Duration for scale animation
        [SerializeField] private Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.2f);
        private Vector3 originalScale = new Vector3(1, 1, 1);
        private Coroutine scaleCoroutine;

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.GemPickup, UpdateGemUI);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.GemPickup, UpdateGemUI);
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
            if (obj is Color gemColor)
            {
                gemsImage.color *= gemColor/10;
            }
        }

        private IEnumerator UpdateUIOverTime()
        {
            yield return StartCoroutine(UtilityFunctions.ScaleObjectOverTime(gemsImage.gameObject, targetScale,scaleDuration));
            yield return StartCoroutine(UtilityFunctions.ScaleObjectOverTime(gemsImage.gameObject, originalScale,scaleDuration));

        }
        
    }
}
