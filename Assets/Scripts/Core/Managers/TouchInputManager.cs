using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using GameLogic.PlayerRelated;
using UnityEngine.InputSystem.Controls;

namespace Core.Managers
{
    public class TouchInputManager : MonoBehaviour
    {
        private Player player; // Reference to the player's Player script
        private float minDistanceToMove = 0.2f; // Minimum distance to consider a valid move
        private bool isShooting = false; // To track if shooting has occurred

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, ActivateGameTouchControl);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, ActivateGameTouchControl);
        }

        private void ActivateGameTouchControl(object obj)
        {
            StartCoroutine(SelfUpdate());
        }

        private IEnumerator SelfUpdate()
        {
            while (CoreManager.instance.GameManager.IsGameActive)
            {
                List<TouchControl> touches = new();

                if (Touchscreen.current != null)
                {
                    // Filter touches and only consider the first valid touch
                    touches = Touchscreen.current.touches
                        .Where(touch => touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began ||
                                        touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved ||
                                        touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Stationary)
                        .ToList();

                    if (touches.Count > 0)
                    {
                        // Only consider the first touch
                        Vector2 primaryTouchPosition = touches[0].position.ReadValue();

                        // Convert touch position to world coordinates
                        Vector3 touchPosition = new Vector3(primaryTouchPosition.x, primaryTouchPosition.y, Camera.main.nearClipPlane);
                        Vector3 worldTouchPosition = CameraManager.instance.MainCamera.ScreenToWorldPoint(touchPosition);

                        HandleSingleTouch(worldTouchPosition);
                    }
                    else
                    {
                        // No active touches, reset movement and shoot
                        HandleNoTouch();
                    }
                }

                yield return null;
            }
        }

        private void HandleSingleTouch(Vector3 worldTouchPosition)
        {
            // Calculate the horizontal distance between the touch position and the player's position
            float distanceToTouch = Mathf.Abs(worldTouchPosition.x - player.transform.position.x);

            if (distanceToTouch > minDistanceToMove)
            {
                if (worldTouchPosition.x < player.transform.position.x)
                {
                    // Move left if the touch is to the left of the player
                    player.PlayerMovement.Move(Vector3.left);
                    isShooting = false;
                }
                else if (worldTouchPosition.x > player.transform.position.x)
                {
                    // Move right if the touch is to the right of the player
                    player.PlayerMovement.Move(Vector3.right);
                    isShooting = false;
                }
            }
        }

        private void HandleNoTouch()
        {
            // If no touch is detected and player hasn't shot yet, shoot
            if (!isShooting)
            {
                isShooting = true;
                player.Shooter.Shoot();
            }
        }
    }
}
