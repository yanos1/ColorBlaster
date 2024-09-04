﻿using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    }
}