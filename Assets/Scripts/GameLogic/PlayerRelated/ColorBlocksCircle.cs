﻿using System;
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

        [SerializeField] private GameObject rotationAxis;


        private bool invincible;
        private bool playerDead;
        private bool isShooting;
        private Coroutine shootingCoroutine;
        private Coroutine colorRushCoroutine;


        private void Awake()
        {
            Color[] currentColors = CoreManager.instance.ColorsManager.CurrentColors;

            if (blocks.Length != currentColors.Length)
            {
                Debug.Log("Mismatch between style colors and color blocks");
            }

            for (int i = 0; i < currentColors.Length; ++i)
            {
                blocks[i].SetColor(currentColors[i]);
                print(currentColors[i]);
            }

            invincible = true;
            playerDead = false;
            isShooting = false;
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.Revive, RestoreBlocks);
            CoreManager.instance.EventManager.AddListener(EventNames.Shoot, SetIsShooting);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateColorRush, OnColorRushPickUp);
            CoreManager.instance.EventManager.AddListener(EventNames.DeactivateColorRush, OnColorRushEnd);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.Revive, RestoreBlocks);
            CoreManager.instance.EventManager.RemoveListener(EventNames.Shoot, SetIsShooting);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateColorRush, OnColorRushPickUp);
            CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateColorRush, OnColorRushEnd);
        }

        private void OnColorRushPickUp(object obj)
        {
            if (obj is (Color color, float duration, TreasureChestBuff buff))
            {
                foreach (var block in blocks)
                {
                    block.Renderer.color = color;
                    block.Renderer.sharedMaterials[1] = colorRushStyle.Material;
                    block.Renderer.sharedMaterials[1].shader = colorRushStyle.Shader;
                }
            }
        }

        private void OnColorRushEnd(object obj)
        {
            int i = 0;
            print("DEACTIVATE !!!!!!!!!!");
            foreach (var block in blocks)
            {
                Style currentStyle = CoreManager.instance.StyleManager.GetStyle();
                Color[] currentColors = CoreManager.instance.ColorsManager.CurrentColors;
                block.SetColor(currentColors[i++]);
                block.Renderer.sharedMaterials[1] = currentStyle.Material;
                block.Renderer.sharedMaterials[1].shader = currentStyle.Shader;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // if (invincible) return;
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

        /// <summary>
        /// this grants us cooldown of .2 seconds for each bullet
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetIsShootingForShortDuration()
        {
            isShooting = true;
            yield return new WaitForSeconds(0.2f);
            isShooting = false;
        }


        private void RestoreBlocks(object obj)
        {
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
                        (isShooting
                            ? CoreManager.instance.ControlPanelManager.GetWheelRotationSpeedWhileShooting()
                            : CoreManager.instance.ControlPanelManager.GetWheeRotationSpeed()) * Time.deltaTime);
                }
            }
        }
    }
}