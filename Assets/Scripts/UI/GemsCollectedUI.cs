using System;
using System.Collections;
using Core.Managers;
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
        [SerializeField] private Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.2f); // Scale target for the bounce effect

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
            gemsCollectedText.text = (int.Parse(gemsCollectedText.text) + 1).ToString();

            // Scale the image with a quick bounce effect
            StartCoroutine(ScaleImage());

            // Multiply the image color by the color passed in the object
            if (obj is Color gemColor)
            {
                gemsImage.color += 30*gemColor; // Multiply current image color by gem color
                print(gemsImage.color);
            }
        }

        private IEnumerator ScaleImage()
        {
            Vector3 originalScale = gemsImage.transform.localScale;

            // Scale up to the target scale
            float elapsedTime = 0f;
            while (elapsedTime < scaleDuration)
            {
                gemsImage.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Scale back down to the original scale
            elapsedTime = 0f;
            while (elapsedTime < scaleDuration)
            {
                gemsImage.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the scale is reset exactly to the original
            gemsImage.transform.localScale = originalScale;
        }
    }
}
