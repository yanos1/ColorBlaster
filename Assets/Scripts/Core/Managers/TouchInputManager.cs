using System;
using Core.PlayerRelated;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Managers
{
    public class TouchInputManager : MonoBehaviour
    {
        private Player player; // Reference to the player's Player script
        private float minDistanceBetweenPlayerAndTouch = 0.2f;
        private bool hasShot = false; // Flag to track if the shot has been triggered
        private bool moveTouch = false;
        private bool shootTouch = false;

        private void Start()
        {
            player = CoreManager.instance.Player;
        }

        private void Update()
        {
            // Check if the touchscreen is available and if there are any active touches
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                // Get the touch position in screen coordinates
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

                // Convert screen touch position to world position
                Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
                touchWorldPosition.z = 0; // Ensure touch position is on the 2D plane

                if (touchWorldPosition.x < player.transform.position.x && Mathf.Abs(touchWorldPosition.y - player.transform.position.y) > minDistanceBetweenPlayerAndTouch)
                {
                    // Determine movement direction based on touch position
                    moveTouch = true;
                    if (Touchscreen.current.touches.Count == 1)
                    {
                        shootTouch = false;
                    }
                    Vector3 direction = touchWorldPosition.y > player.transform.position.y ? Vector3.up : Vector3.down;
                    player.PlayerMovement.Move(direction);
                }
                else
                {
                    moveTouch = false;
                }
                if (!shootTouch)
                {
                    shootTouch = true;
                    // Trigger shooting event if touch position is not to the right of the player and has not shot yet
                    player.Shooter.Shoot();
                }
            }
            else
            {
                // Reset the hasShot flag when touch is released
                moveTouch = false;
                shootTouch = false;
            }
        }
    }
}
