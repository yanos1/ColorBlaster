using System.Collections;
using System.Linq;
using Core.Managers;
using Extentions;
using GameLogic.Consumables;
using GameLogic.ConsumablesGeneration;
using GameLogic.ObstacleGeneration;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.PlayerRelated
{
    // this manager styles the circle of colors aorund the plyer based on the current style
    public class ColorBlocksCircle : MonoBehaviour
    {
        [SerializeField] private ColorBlock[] blocks;
        [SerializeField] private Style colorRushStyle;
        [SerializeField] float rotationSpeed; // Speed of rotation
        [SerializeField] float rotationSpeedWhileShooting; // Speed of rotation
        [SerializeField] private GameObject rotationAxis;

        private bool invincible;
        private bool playerDead;
        private bool isShooting;
        private Coroutine shootingCoroutine;
        private Coroutine colorRushCoroutine;


        private void Awake()
        {
            Color[] currentColors = CoreManager.instance.ColorsManager.CurrentColors;
            Debug.Log(currentColors.First());

            if (blocks.Length != currentColors.Length)
            {
                Debug.Log("Mismatch between style colors and color blocks");
            }

            for (int i = 0; i < currentColors.Length; ++i)
            {
                blocks[i].SetColor(currentColors[i]);
            }

            invincible = false;
            playerDead = false;
            isShooting = false;
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.Revive, RestoreBlocks);
            CoreManager.instance.EventManager.AddListener(EventNames.IncreaseGameDifficulty, RotateFaster);
            CoreManager.instance.EventManager.AddListener(EventNames.Shoot, SetIsShooting);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateColorRush, ActicateColorRush);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.Revive, RestoreBlocks);
            CoreManager.instance.EventManager.RemoveListener(EventNames.IncreaseGameDifficulty, RotateFaster);
            CoreManager.instance.EventManager.RemoveListener(EventNames.Shoot, SetIsShooting);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateColorRush, ActicateColorRush);
        }

        private void ActicateColorRush(object obj)
        {
            colorRushCoroutine = StartCoroutine(ActivateColorRushForDuration());
        }

        private IEnumerator ActivateColorRushForDuration()
        {
            foreach (var block in blocks)
            {
                block.Renderer.color = Color.white;
                block.Renderer.material = colorRushStyle.Material;
                block.Renderer.material.shader = colorRushStyle.Shader;
            }

            yield return new WaitForSeconds(10f);
            DeactivateColorRush();
        }

        private void DeactivateColorRush()
        {
            int i = 0;
            foreach (var block in blocks)
            {
                Style currentStyle = CoreManager.instance.StyleManager.GetStyle();
                Color[] currentColors = CoreManager.instance.ColorsManager.CurrentColors;
                block.Renderer.color = currentColors[i++];
                block.Renderer.material = currentStyle.Material;
                block.Renderer.material.shader = currentStyle.Shader;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ObstaclePart part = other.GetComponent<ObstaclePart>();
            if (part is not null && !CoreManager.instance.Player.IsDead)
            {
                ShatterColorBlocks();
                CoreManager.instance.EventManager.InvokeEvent(EventNames.KillPlayer, null);
            }

            Consumable consumable = other.GetComponent<Consumable>();
            if (consumable is not null)
            {
                consumable.Consume();
            }
        }

        private void SetIsShooting(object obj)
        {
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(SetIsShootingForShortDuration());
            }
            else
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = StartCoroutine(SetIsShootingForShortDuration());
            }
        }

        private IEnumerator SetIsShootingForShortDuration()
        {
            isShooting = true;
            yield return new WaitForSeconds(0.2f);
            isShooting = false;
        }


        private void RotateFaster(object obj)
        {
            rotationSpeed += 16;
        }


        private void RestoreBlocks(object obj)
        {
            if (colorRushCoroutine is not null)
            {
                DeactivateColorRush();
            }
            float maxReviveDuration = 0;
            float baseReviveDuration = 0.5f;
            for (int i = 0; i < blocks.Length; ++i)
            {
                float curReviveDuration = Random.value + baseReviveDuration;
                maxReviveDuration = Mathf.Max(curReviveDuration, maxReviveDuration);
                blocks[i].gameObject.SetActive(true);
                StartCoroutine(UtilityFunctions.MoveObjectOverTime(
                    blocks[i].gameObject,
                    blocks[i].transform.position,
                    blocks[i].transform.rotation,
                    blocks[i].StartingPosition,
                    blocks[i].StartingRotation,
                    curReviveDuration,
                    i == blocks.Length - 1
                        ? null
                        : () => StartCoroutine(SetPlayerDeadAfterDelay(maxReviveDuration)) // Call the method
                ));
            }
        }


        private IEnumerator SetPlayerDeadAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            CoreManager.instance.EventManager.InvokeEvent(EventNames.FinishedReviving, null);
            playerDead = false;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ShatterColorBlocks()
        {
            playerDead = true;
            foreach (var block in blocks)
            {
                block.Shatter();
                block.transform.position = block.EndGamePosition;
            }
        }


        private void Update()
        {
            if (playerDead) return;
            RotateAroundPlayer();
        }


        private void RotateAroundPlayer()
        {
            foreach (var colorBlock in blocks)
            {
                if (colorBlock != null)
                {
                    colorBlock.transform.RotateAround(rotationAxis.transform.position, Vector3.back,
                        (isShooting ? rotationSpeedWhileShooting : rotationSpeed) * Time.deltaTime);
                }
            }
        }
    }
}