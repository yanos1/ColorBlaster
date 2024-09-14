using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Extentions;
using PoolTypes;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas gameUICanvas;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject oneMoreAttemptPanel;
        [SerializeField] private TextMeshProUGUI gemsCollectedText;
        [SerializeField] private TextMeshProUGUI gemsOwnedText;
        [SerializeField] private GameObject playAgainButton;
        [SerializeField] private PoolType gemType;

        [SerializeField] private PanelManager panelManager; // Reference to the new PanelManager
        [SerializeField] private GemTransferManager gemTransferManager; // Reference to the new GemTransferManager

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, ShowGameOverPanel);
            CoreManager.instance.EventManager.AddListener(EventNames.BroadcastGemsPicked, TransferGems);
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, ShowOneMoreAttemptMenu);
        }


        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, ShowGameOverPanel);
            CoreManager.instance.EventManager.RemoveListener(EventNames.BroadcastGemsPicked, TransferGems);
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, ShowOneMoreAttemptMenu);
        }


        private void TransferGems(object obj)
        {
            string gemsOwned = CoreManager.instance.UserDataManager.GemsOwned.ToString();
            gemsOwnedText.text = gemsOwned;
            print(gemsOwnedText.text);
            print(")))))))");

            if (obj is List<Color> colorsList)
            {
                float duration = 1.7f;
                int amount = int.Parse(gemsCollectedText.text);
                gemTransferManager.TransferGems(gemsCollectedText, gemsOwnedText, colorsList, gemType, duration);
                StartCoroutine(UIUtilityFunctions.TransferNumberCoroutine(gemsCollectedText, gemsOwnedText,
                    amount, duration, () => { CoreManager.instance.UserDataManager.AddGems(amount); }));
                StartCoroutine(ActivatePlayAgainButtonAfterDelay(duration));
            }
        }

        private void ShowGameOverPanel(object obj)
        {
            panelManager.ShowPanelAfterDelay(gameOverPanel, 0.7f,
                () => { CoreManager.instance.EventManager.InvokeEvent(EventNames.GameOverPanelActive, null); });
        }

        private void ShowOneMoreAttemptMenu(object obj)
        {
            panelManager.ShowPanelAfterDelay(oneMoreAttemptPanel, 1.2f);
        }

        private IEnumerator ActivatePlayAgainButtonAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            playAgainButton.SetActive(true);
        }
    }
}