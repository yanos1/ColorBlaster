using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.PlayerRelated;
using System.Linq;
using UnityEngine.InputSystem.Controls;

namespace Core.Managers
{
    public class TouchInputManager : MonoBehaviour
    {
        private Player player; // Reference to the player's Player script
        private float minDistanceBetweenPlayerAndTouch = 0.2f;
        private bool shootTouch = false;


        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, ActivateGameToucbControl);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, ActivateGameToucbControl);
        }
        private void ActivateGameToucbControl(object obj)
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
                    // Filter out touches that are not in Began, Moved, or Stationary phases
                    touches = Touchscreen.current.touches
                        .Where(touch => touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began ||
                                        touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved ||
                                        touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Stationary)
                        .ToList();


                    if (touches.Count > 0)
                    {
                        // Get the first active touch
                        Vector2 primaryTouchPosition = touches[0].position.ReadValue();

                        // Convert touch position to world coordinates
                        Vector3 touchPosition = new Vector3(primaryTouchPosition.x, primaryTouchPosition.y,
                            Camera.main.nearClipPlane);
                        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);

                        if (touches.Count == 1)
                        {
                            HandleSingleTouch(worldTouchPosition);
                        }
                        else if (touches.Count > 1)
                        {
                            // Handle second touch
                            Vector2 secondTouchPosition = touches[1].position.ReadValue();

                            // Ignore touches at (0, 0)
                            if (secondTouchPosition != Vector2.zero)
                            {
                                Vector3 secondTouchWorldPosition = new Vector3(secondTouchPosition.x,
                                    secondTouchPosition.y, Camera.main.nearClipPlane);
                                secondTouchWorldPosition = Camera.main.ScreenToWorldPoint(secondTouchWorldPosition);

                                HandleMultipleTouches(worldTouchPosition, secondTouchWorldPosition);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("No active touches, resetting shootTouch flag.");
                        shootTouch = false;
                    }
                }
                else
                {
                }

                yield return null;
            }
        }

        private void HandleSingleTouch(Vector3 worldTouchPosition)
        {
            if (worldTouchPosition.x < player.transform.position.x)
            {
                shootTouch = false;
                if (Mathf.Abs(worldTouchPosition.y - player.transform.position.y) > minDistanceBetweenPlayerAndTouch)
                {
                    // Move player
                    Vector3 direction = worldTouchPosition.y > player.transform.position.y ? Vector3.up : Vector3.down;
                    player.PlayerMovement.Move(direction);
                }
            }
            if (worldTouchPosition.x >= player.transform.position.x)
            {
                // Shoot
                print("entered single shot shot for some reason");
                if (!shootTouch)
                {
                    shootTouch = true;
                    player.Shooter.Shoot();
                }
            }
        }

        private void HandleMultipleTouches(Vector3 worldTouchPosition1, Vector3 worldTouchPosition2)
        {
            if (worldTouchPosition1.x < player.transform.position.x &&
                worldTouchPosition2.x < player.transform.position.x &&
                Mathf.Abs(worldTouchPosition1.y - player.transform.position.y) > minDistanceBetweenPlayerAndTouch)
            {
                // Move player
                Vector3 direction = worldTouchPosition1.y > player.transform.position.y ? Vector3.up : Vector3.down;
                player.PlayerMovement.Move(direction);
                shootTouch = false; // Allow shooting in the next frame
            }
            else if (worldTouchPosition1.x >= player.transform.position.x &&
                     worldTouchPosition2.x >= player.transform.position.x)
            {
                // Shoot
                if (!shootTouch)
                {
                    print("entered multy touch  shot by 2 clicks beyong player");

                    shootTouch = true;
                    player.Shooter.Shoot();
                }
            }
            else
            {
                // Both moving and shooting
                print("try to shoot and move together");
                print("entered multy touch  shot by moving and shooting together");

                Vector3 movingPos = worldTouchPosition1.x < player.transform.position.x
                    ? worldTouchPosition1
                    : worldTouchPosition2;

                if (Mathf.Abs(movingPos.y - player.transform.position.y) > minDistanceBetweenPlayerAndTouch)
                {

                    Vector3 direction = movingPos.y > player.transform.position.y + minDistanceBetweenPlayerAndTouch
                        ? Vector3.up
                        : Vector3.down;
                    player.PlayerMovement.Move(direction);
                }
                if (!shootTouch)
                {
                    shootTouch = true;
                    player.Shooter.Shoot();
                }
            }
        }
    }
}