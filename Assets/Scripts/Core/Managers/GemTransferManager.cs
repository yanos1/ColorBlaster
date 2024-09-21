using Extentions;
using UI;

namespace Core.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using Core.Managers;
    using PoolTypes;
    using TMPro;
    using UnityEngine;

    public class GemTransferManager : MonoBehaviour
    {
        [SerializeField] private Canvas gameUICanvas;

        public void TransferGemsText(TextMeshProUGUI collectedText, TextMeshProUGUI ownedText, List<Color> colors,
            PoolType gemType, float duration)
        {
            StartCoroutine(TransferGemsTextOverTime(collectedText, ownedText, colors, gemType, duration));
        }

        private IEnumerator TransferGemsTextOverTime(TextMeshProUGUI collectedText, TextMeshProUGUI ownedText,
            List<Color> colors, PoolType gemType, float duration)
        {
            float durationForOneGem = duration / colors.Count;
            foreach (var color in colors)
            {
                GameObject gem = CoreManager.instance.PoolManager.GetFromPool(gemType);
                SetGemInitialProperties(gem, collectedText, color);

                // Move gems to the final position
                Vector3 ownedTextWorldPosition = UIUtilityFunctions.GetWorldPositionFromUI(ownedText.rectTransform,
                    gameUICanvas, CameraManager.instance.MainCamera);
                StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem, gem.transform.position, Quaternion.identity,
                    ownedTextWorldPosition, Quaternion.identity, durationForOneGem, () =>
                    {
                        CoreManager.instance.PoolManager.ReturnToPool(gemType, gem);
                        // TODO: Play earn sound if needed
                    }));

                yield return new WaitForSeconds(durationForOneGem/2);
            }
            
        }

        private void SetGemInitialProperties(GameObject gem, TextMeshProUGUI collectedText, Color color)
        {
            Vector3 gemWorldPosition = UIUtilityFunctions.GetWorldPositionFromUI(collectedText.rectTransform,
                gameUICanvas, CameraManager.instance.MainCamera);
            gemWorldPosition.z = 0f; // Ensure Z position is reset to 0
            gem.transform.position = gemWorldPosition;

            gem.GetComponent<SpriteRenderer>().color = color;
            gem.GetComponent<TrailRenderer>().startColor = color;
            UtilityFunctions.MoveObjectInRandomDirection(gem.transform,0.5f);
        }
    }
}