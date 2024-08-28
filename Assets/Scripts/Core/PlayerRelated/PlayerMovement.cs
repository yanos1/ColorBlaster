namespace Core.PlayerRelated
{
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 10f; // Speed of the player's movement
        public float maxYPosition = 5f; // Maximum Y position the player can move to
        public float minYPosition = -5f; // Minimum Y position the player can move to

        
        void Update()
        {
            // Get the vertical input (up and down arrows or W/S keys)
            float verticalInput = Input.GetAxis("Vertical");
        
            // Calculate the movement direction
            Vector3 moveDirection = new Vector3(0, verticalInput, 0);
        
            // Apply the movement
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        }


        public void Move(Vector3 direction)
        {
            // Move the player up
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

            // Clamp the player's position within the bounds
            newPosition.y = Mathf.Clamp(newPosition.y, minYPosition, maxYPosition);

            // Update the player's position
            transform.position = newPosition;
        }
    }
    
    
}