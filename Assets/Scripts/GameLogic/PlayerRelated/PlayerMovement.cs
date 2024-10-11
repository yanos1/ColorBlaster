using System;
using Core.Managers;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class PlayerMovement : MonoBehaviour
    {
        private float maxYPosition; // Maximum Y position the player can move to
        private float minYPosition; // Minimum Y position the player can move to

        private float minMoveDistance = 0.3f; // Minimum distance to move
        private TouchInputManager _inputManager;


        private void Start()
        {
            maxYPosition = transform.position.y + minMoveDistance;
            minYPosition = transform.position.y - minMoveDistance;
        }

        void Update()
        {
            if (CoreManager.instance.Player.IsDead) return;
            if (_inputManager.IsTouchOverUI()) return;
            // Check for touch input first
            Vector2? inputPosition = GetInputPosition();

            if (inputPosition != null)
            {
                HandleHorizontalMovement((Vector2)inputPosition); // Move based on touch or mouse input
                HandleVerticalMovement();
            }
            else
            {
                DetectKeyBoardInput(); // Fallback to keyboard input if no touch or mouse input
            }
        }

// Consolidate touch and mouse input checks into a single method
        private Vector2? GetInputPosition()
        {
            Vector2? touchPosition = _inputManager.GetTouchPosition();

            if (touchPosition != null)
            {
                print("returned touch postion");
                return touchPosition; // Return touch position if detected
            }

            Vector2? mousePosition = _inputManager.GetMousePosition();
            print($"returned mouse pos {mousePosition}");
            return mousePosition; // Return mouse position if detected, otherwise null
        }

        private void HandleVerticalMovement()
        {
            Vector2? swipePosition = _inputManager.GetSwipePosition();
            if (swipePosition != null)
            {
                // Get the swipe direction
                Vector2 swipeDirection = (Vector2)swipePosition;

                // Calculate the target position based on the swipe direction

                float targetYPosition = transform.position.y + (swipeDirection.y);
                print($"target y {targetYPosition} min y {minYPosition} max y {maxYPosition}");
                targetYPosition = Mathf.Clamp(targetYPosition, minYPosition, maxYPosition);
                print($"target y : {targetYPosition}");
                if (Mathf.Approximately(targetYPosition, transform.position.y))
                {
                    return;   // we are already at edge position
                }
                Vector3 targetPosition = transform.position + (Vector3)(minMoveDistance * swipeDirection);
                print($"try to swipe to {targetPosition} from {transform.position}");
                // Move the player to the target position (you may want to clamp or smooth this movement)
                transform.position = Vector3.MoveTowards(transform.position, targetPosition,
                    CoreManager.instance.ControlPanelManager.playerMovementSpeed);
            }
        }

        // Handle player movement using touch input
        // private void HandleHorizontalMovement(Vector2 touchPosition)
        // {
        //     // Check if touch is far enough from the player
        //     if (Mathf.Abs(touchPosition.x - transform.position.x) < minMoveDistance) return;
        //
        //     // Move towards touch position (horizontal movement)
        //     Vector3 targetPosition = new Vector3(touchPosition.x, transform.position.y, 0);
        //     // if (Math.Abs(targetPosition.x - transform.position.x) < 0.2f)
        //     // {
        //     //     print("TELEPORTING");
        //     //     transform.position = targetPosition;
        //     // }
        //     // {
        //     //     print("teleporting !!!");
        //     //     transform.position = targetPosition;
        //     // }
        //
        //
        //     transform.position = Vector3.MoveTowards(transform.position, targetPosition,
        //         CoreManager.instance.ControlPanelManager.playerMovementSpeed * Time.deltaTime);
        // }
        private void HandleHorizontalMovement(Vector2 touchPosition)
        {
            // Check if touch is far enough from the player
            if (Mathf.Abs(touchPosition.x - transform.position.x) < minMoveDistance) return;

            // Target position with horizontal movement only
            Vector3 targetPosition = new Vector3(touchPosition.x, transform.position.y, 0);

            // Calculate distance to target and adapt speed based on distance
            float distance = Vector3.Distance(transform.position, targetPosition);
            float adaptiveSpeed = CoreManager.instance.ControlPanelManager.playerMovementSpeed * distance;

            // Move player smoothly towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, adaptiveSpeed * Time.deltaTime);
        }


        // Calculate the new movement direction based on touch position
        private Vector3 GetNewPosition(Vector2 touchPosition)
        {
            // Determine direction based on the X-axis
            int direction = touchPosition.x > transform.position.x ? 1 : -1;
            // Move right for positive direction, left for negative

            return Vector3.right * (direction * CoreManager.instance.ControlPanelManager.playerMovementSpeed *
                                    Time.deltaTime);
        }

        // Handle player movement using keyboard input (horizontal movement)
        private void DetectKeyBoardInput()
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            // Calculate the movement direction
            Vector3 moveDirection = new Vector3(horizontalInput, 0, 0);

            // Apply the movement
            transform.Translate(moveDirection *
                                (CoreManager.instance.ControlPanelManager.playerMovementSpeed * Time.deltaTime));
        }

        // Clamp player's vertical movement (for possible future use, currently unused)
        public void Move(Vector3 direction)
        {
            // Move the player based on the provided direction vector
            Vector3 newPosition = transform.position +
                                  direction * (CoreManager.instance.ControlPanelManager.playerMovementSpeed *
                                               Time.deltaTime);

            // Clamp the player's Y position within the specified bounds
            newPosition.y = Mathf.Clamp(newPosition.y, minYPosition, maxYPosition);

            // Update the player's position
            transform.position = newPosition;
        }

        // Initialize the input manager
        public void Init(TouchInputManager inputManager)
        {
            _inputManager = inputManager;
        }
    }
}