namespace Core.Managers
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class PanelManager : MonoBehaviour
    {
        public void ShowPanelAfterDelay(GameObject panel, float delay, Action onComplete = null)
        {
            StartCoroutine(ShowPanelCoroutine(panel, delay, onComplete));
        }

        private IEnumerator ShowPanelCoroutine(GameObject panel, float delay, Action onComplete = null)
        {
            yield return new WaitForSeconds(delay);
            panel.SetActive(true);
            onComplete?.Invoke();
        }
    }

}