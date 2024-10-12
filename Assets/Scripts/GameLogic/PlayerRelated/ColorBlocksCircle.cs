using System;
using System.Collections;
using System.Linq;
using Core.Managers;
using Extentions;
using GameLogic.Boosters;
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
            SetColorWheelColors(null);

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
            CoreManager.instance.EventManager.AddListener(EventNames.SessionUp, SetColorWheelColors);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.Revive, RestoreBlocks);
            CoreManager.instance.EventManager.RemoveListener(EventNames.Shoot, SetIsShooting);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateColorRush, OnColorRushPickUp);
            CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateColorRush, OnColorRushEnd);
            CoreManager.instance.EventManager.RemoveListener(EventNames.SessionUp, SetColorWheelColors);

        }

        private void SetColorWheelColors(object obj)
        {
            Color[] currentColors = CoreManager.instance.ColorsManager.CurrentColors;

            for (int i = 0; i < blocks.Length; ++i)
            {
                blocks[i].SetColor(currentColors[i%currentColors.Length]);
            }
        }
        private void OnColorRushPickUp(object obj)
        {
            if (obj is (Color color, float duration, BoosterButtonController buff))
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
                block.SetColor(currentColors[i++%currentColors.Length]);
                block.Renderer.sharedMaterials[1] = currentStyle.Material;
                print("11");
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
                        : () => StartCoroutine(SetPlayeAliveAfterDelay(maxReviveDuration)) // Call the method
                ));
            }
        }


        private IEnumerator SetPlayeAliveAfterDelay(float delay)
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