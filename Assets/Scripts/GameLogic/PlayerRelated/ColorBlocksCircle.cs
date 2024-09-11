﻿using System.Collections;
using System.Linq;
using Core.Managers;
using Extentions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.PlayerRelated
{
    // this manager styles the circle of colors aorund the plyer based on the current style
    public class ColorBlocksCircle : MonoBehaviour
    {
        [SerializeField] private ColorBlock[] blocks;
        [SerializeField] float rotationSpeed; // Speed of rotation
        [SerializeField] float rotationSpeedWhileShooting; // Speed of rotation
        [SerializeField] private GameObject rotationAxis;

        private bool invincible;
        private bool playerDead;
        private bool isShooting;
        private Coroutine shootingCoroutine;

        
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
            CoreManager.instance.EventManager.AddListener(EventNames.KillPlayer, ShatterColorBlocks);
            CoreManager.instance.EventManager.AddListener(EventNames.IncreaseGameDifficulty, RotateFaster);
            CoreManager.instance.EventManager.AddListener(EventNames.Shoot, SetIsShooting);
            

        }
        
        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.Revive, RestoreBlocks);
            CoreManager.instance.EventManager.RemoveListener(EventNames.KillPlayer, ShatterColorBlocks);
            CoreManager.instance.EventManager.RemoveListener(EventNames.IncreaseGameDifficulty, RotateFaster);
            CoreManager.instance.EventManager.RemoveListener(EventNames.Shoot, SetIsShooting);


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
            float maxReviveDuration = 0;
            float baseReviveDuration = 0.5f;
            for (int i=0; i < blocks.Length ; ++i)
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
                    curReviveDuration, i == blocks.Length-1 ? null : () => StartCoroutine(SetPlayerDeadAfterDelay(maxReviveDuration)) // Call the method
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
        private void ShatterColorBlocks(object obj)
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