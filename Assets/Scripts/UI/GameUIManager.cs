using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Extentions;
using PoolTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas gameUICanvas;

        // Start is called before the first frame update
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject oneMoreAttemptPanel;
        // [SerializeField] private GameManager pauseMenu;

        [SerializeField] private TextMeshProUGUI gemsCollectedText;
        [SerializeField] private TextMeshProUGUI gemsOwnedText;

        [SerializeField] private GameObject playAgainButton;

        [SerializeField] private PoolType gemType;


        void OnEnable()
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
            StartCoroutine(TransferGemsOverTime(obj));
        }

        private IEnumerator TransferGemsOverTime(object obj)
        {
            float duration = 2f;
            StartCoroutine(UtilityUIFunctions.TransferNumberCoroutine(gemsCollectedText, gemsOwnedText,
                int.Parse(gemsCollectedText.text), duration));
            if (obj is List<Color> colorsList)
            {
                float durationForOneGem = duration / colorsList.Count;
                
                foreach (var color in colorsList)
                {
                    GameObject gem = CoreManager.instance.PoolManager.GetFromPool(gemType);


                    Vector3 gemWorldPosition = UtilityUIFunctions.GetWorldPositionFromUI(
                        gemsCollectedText.rectTransform,
                        gameUICanvas,
                        CameraManager.instance.MainCamera);
                    Vector3 gemsOwnedWorldPosition = UtilityUIFunctions.GetWorldPositionFromUI(
                        gemsOwnedText.rectTransform,
                        gameUICanvas,
                        CameraManager.instance.MainCamera);
                    gemWorldPosition.z = 0f;
                    gem.transform.position = gemWorldPosition;
                    UtilityFunctions.MoveObjectInRandomDirection(gem.transform);



                    gem.GetComponent<SpriteRenderer>().color = color;
                    gem.GetComponent<TrailRenderer>().startColor = color;

                    CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem,
                        gem.transform.position, Quaternion.identity,
                        gemsOwnedWorldPosition, Quaternion.identity, durationForOneGem, () =>
                        {
                            CoreManager.instance.PoolManager.ReturnToPool(gemType, gem);
                            // TODO: Play earn sound
                        }));
                    yield return new WaitForSeconds(durationForOneGem);
                }

                yield return new WaitForSeconds(duration);
                // TODO animation
                playAgainButton.SetActive(true);
                
            }
        }

        private void ShowGameOverPanel(object obj)
        {
            print("show panel!");
            float delay = 0.7f;
            StartCoroutine(ShowGamePanelPanelAfterDelay(gameOverPanel, delay, () =>
            {
                CoreManager.instance.EventManager.InvokeEvent(EventNames.GameOverPanelActive, null);

            }));
            
        }

        private void ShowOneMoreAttemptMenu(object obj)
        {
            StartCoroutine(ShowGamePanelPanelAfterDelay(oneMoreAttemptPanel, 1.2f));
        }

        private IEnumerator ShowGamePanelPanelAfterDelay(GameObject panel, float delay, Action onComplete = null)
        {
            yield return new WaitForSeconds(delay);
            panel.SetActive(true);
            onComplete?.Invoke();
            // if (pause)
            // {
            //     CoreManager.instance.TimeManager.PauseGame();
            // }
        }
    }
}