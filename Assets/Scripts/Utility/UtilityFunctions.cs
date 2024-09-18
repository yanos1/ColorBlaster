using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Extentions
{
    public static class UtilityFunctions
    {
        public static T[] ShuffleArray<T>(T[] array)
        {
            T[] copy = (T[])array.Clone();
            System.Random random = new System.Random();
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (copy[i], copy[j]) = (copy[j], copy[i]);
            }

            return copy;
        }

        public static IEnumerator FadeImage(Renderer renderer, float startValue, float endValue,
            float imageFadeDuration)
        {
            float elapsedTime = 0f;

            Color color = renderer.material.color;

            while (elapsedTime < imageFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / imageFadeDuration);

                color.a = currentAlpha;

                renderer.material.color = color;

                yield return null;
            }

            color.a = endValue;
            renderer.material.color = color;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static IEnumerator MoveObjectOverTime(GameObject obj, Vector3 startingPos, Quaternion startingRotation,
            Vector3 endingPos, Quaternion endingRotation, float duration, Action onComplete = null)
        {
            float timeElapsed = 0;
            float percentageCompleted = 0;
            while (percentageCompleted < 1)
            {
                timeElapsed += Time.deltaTime;
                percentageCompleted = timeElapsed / duration;
                obj.transform.position = Vector3.Lerp(startingPos, endingPos, percentageCompleted);
                obj.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, percentageCompleted);
                yield return null;
            }

            obj.transform.position = endingPos;
            obj.transform.rotation = endingRotation;

            // Invoke the callback if provided
            onComplete?.Invoke();
        }

        public static IEnumerator MoveObjectOverTime(GameObject obj, Vector3 startingPos, Quaternion startingRotation,
            Transform endingPos, Quaternion endingRotation, float duration, Action onComplete = null)
        {
            float timeElapsed = 0;
            float percentageCompleted = 0;
            while (percentageCompleted < 1)
            {
                timeElapsed += Time.deltaTime;
                percentageCompleted = timeElapsed / duration;
                obj.transform.position = Vector3.Lerp(startingPos, endingPos.position, percentageCompleted);
                obj.transform.rotation = Quaternion.Lerp(startingRotation, endingRotation, percentageCompleted);
                yield return null;
            }

            obj.transform.position = endingPos.position;
            obj.transform.rotation = endingRotation;

            // Invoke the callback if provided
            onComplete?.Invoke();
        }

        public static void MoveObjectInRandomDirection(Transform obj, float magnitude = 1f)
        {
            float xAddition = Random.Range(-magnitude, magnitude);
            float yAddition = Random.Range(-magnitude, magnitude);
            obj.position += new Vector3(xAddition, yAddition, 0);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static IEnumerator WaitAndInvokeAction(float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            onComplete?.Invoke();
        }

        public static IEnumerator ScaleObjectOverTime(GameObject objToScale, Vector3 targetScale, float duration)
        {
            Vector3 originalScale = objToScale.transform.localScale;

            // Scale up to the target scale
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                objToScale.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            objToScale.transform.localScale = targetScale;
        }


        public static void StopAndStartCoroutine(this MonoBehaviour monoBehaviour, ref Coroutine coroutine,
            IEnumerator routine)
        {
            if (coroutine != null)
            {
                monoBehaviour.StopCoroutine(coroutine);
            }

            coroutine = monoBehaviour.StartCoroutine(routine);
        }


        public static bool CompareColors(Color color1, Color color2, int decimalPlaces = 2)
        {
            // Round each component to the specified number of decimal places
            float color1R = (float)Math.Round(color1.r, decimalPlaces);
            float color1G = (float)Math.Round(color1.g, decimalPlaces);
            float color1B = (float)Math.Round(color1.b, decimalPlaces);
            float color1A = (float)Math.Round(color1.a, decimalPlaces);

            float color2R = (float)Math.Round(color2.r, decimalPlaces);
            float color2G = (float)Math.Round(color2.g, decimalPlaces);
            float color2B = (float)Math.Round(color2.b, decimalPlaces);
            float color2A = (float)Math.Round(color2.a, decimalPlaces);

            // Compare rounded values directly
            bool redEqual = color1R == color2R;
            bool greenEqual = color1G == color2G;
            bool blueEqual = color1B == color2B;
            bool alphaEqual = color1A == color2A;

            // Print results for each color channel
            Debug.Log($"Red Comparison: {color1R} vs {color2R} -> {redEqual}");
            Debug.Log($"Green Comparison: {color1G} vs {color2G} -> {greenEqual}");
            Debug.Log($"Blue Comparison: {color1B} vs {color2B} -> {blueEqual}");
            Debug.Log($"Alpha Comparison: {color1A} vs {color2A} -> {alphaEqual}");

            // Return true only if all channels are equal
            return redEqual && greenEqual && blueEqual && alphaEqual;
        }
    }
}