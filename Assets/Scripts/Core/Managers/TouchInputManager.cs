using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using GameLogic.PlayerRelated;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;

namespace Core.Managers
{
    public class TouchInputManager : MonoBehaviour
    {
        private float minDistanceToMove = 0.7f;  // Minimum distance to consider a valid move
        private bool isShooting = false;         // To track if shooting has occurred
        private Vector2 touchPosition;
        private Vector2 mousePosition;
        private Vector2 lastTouchPosition;
        private Vector2 swipeDirection;
        private TouchControl currentTouch;

        public Mouse Mouse => mouse;
        public Touchscreen Screen => screen;
        
        private Mouse mouse;
        private Touchscreen screen;

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, ActivateGameTouchControl);
            mouse = Mouse.current;
            screen = Touchscreen.current;
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
                // Handle touch input
                if (Touchscreen.current != null)
                {
                    var touches = GetTouches();
                    if (touches.Count > 0)
                    {
                        currentTouch = touches[0];
                        if (!IsTouchOverUI())  // Only process if not over UI
                        {
                            touchPosition = GetTouchPosition(currentTouch);
                            swipeDirection = DetectSwipeInput(currentTouch);  // Detect swipe for touch input
                        }
                        else
                        {
                            touchPosition = default;
                            currentTouch = default;
                        }
                    }
                }

                // Handle mouse input
                mousePosition = HandleMouseInput();
                if (Mouse.current != null && Mouse.current.leftButton.isPressed)
                {
                    if (!IsPointerOverUI())  // Only process if not over UI
                    {
                        swipeDirection = DetectMouseSwipe();  // Detect swipe for mouse input
                    }
                }

                yield return null;
            }
        }

        // Swipe detection for touch input
        public Vector2 DetectSwipeInput(TouchControl touch)
        {
            // Track the swipe phase for touch input
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                lastTouchPosition = GetTouchPosition(touch);  // Start of the swipe
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 currentTouchPosition = GetTouchPosition(touch);
                Vector2 swipeDelta = currentTouchPosition - lastTouchPosition;

                if (Mathf.Abs(swipeDelta.y) > minDistanceToMove)  // Check for vertical movement
                {
                    return swipeDelta.y > 0 ? Vector2.up : Vector2.down;
                }
            }

            return default;
        }

        // Swipe detection for mouse input
        private Vector2 DetectMouseSwipe()
        {
            // Handle mouse swipe detection
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                lastTouchPosition = HandleMouseInput();  // Register the start of the swipe
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                Vector2 currentMousePosition = HandleMouseInput();
                Vector2 swipeDelta = currentMousePosition - lastTouchPosition;

                if (Mathf.Abs(swipeDelta.y) > minDistanceToMove)  // Check for vertical movement
                {
                    return swipeDelta.y > 0 ? Vector2.up : Vector2.down;
                }
            }

            return default;
        }

        // Convert touch position to world coordinates
        private Vector2 GetTouchPosition(TouchControl touch)
        {
            Vector2 primaryTouchPosition = touch.position.ReadValue();
            Vector3 positionNearClip = new Vector3(primaryTouchPosition.x, primaryTouchPosition.y,
                CameraManager.instance.MainCamera.nearClipPlane);
            return CameraManager.instance.MainCamera.ScreenToWorldPoint(positionNearClip);
        }

        // Get valid touch inputs
        private static List<TouchControl> GetTouches()
        {
            return Touchscreen.current.touches
                .Where(touch => touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began ||
                                touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved ||
                                touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Stationary)
                .ToList();
        }

        // Handle mouse input for position
        private Vector2 HandleMouseInput()
        {
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                Vector2 currentMousePosition = Mouse.current.position.ReadValue();
                Vector3 positionNearClip = new Vector3(currentMousePosition.x, currentMousePosition.y,
                    CameraManager.instance.MainCamera.nearClipPlane);
                return CameraManager.instance.MainCamera.ScreenToWorldPoint(positionNearClip);
            }
            else
            {
                return default;
            }
        }

        // Public function to get touch position
        public Vector2? GetTouchPosition()
        {
            return touchPosition != default ? touchPosition : null;
        }

        // Public function to get mouse position
        public Vector2? GetMousePosition()
        {
            return mousePosition != default ? mousePosition : null;
        }

        // Public function to get swipe direction
        public Vector2? GetSwipePosition()
        {
            return swipeDirection != default ? swipeDirection : null;
        }

        // Check if touch is over a UI element
        public bool IsTouchOverUI()
        {
            // Ensure currentTouch is valid
            if (currentTouch != null && EventSystem.current != null)
            {
                if (EventSystem.current.IsPointerOverGameObject(currentTouch.touchId.ReadValue()))
                {
                    return true;  // Touch is over a UI element
                }
            }
            return false;  // Touch is not over a UI element
        }

        // Check if mouse pointer is over a UI element
        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}
