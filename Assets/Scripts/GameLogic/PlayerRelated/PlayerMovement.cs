using Core.Managers;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class PlayerMovement : MonoBehaviour
    {
        public float maxYPosition = 5f; // Maximum Y position the player can move to
        public float minYPosition = -5f; // Minimum Y position the player can move to

        
        void Update()
        {
            if(CoreManager.instance.Player.IsDead) return;
            
            float horizontalInput = Input.GetAxis("Horizontal");
        
            // Calculate the movement direction
            Vector3 moveDirection = new Vector3(horizontalInput,0, 0);
        
            // Apply the movement
            transform.Translate(moveDirection * (CoreManager.instance.ControlPanelManager.playerMovementSpeed * Time.deltaTime));
        }


        public void Move(Vector3 direction)
        {
            // Move the player up
            Vector3 newPosition = transform.position + direction * (CoreManager.instance.ControlPanelManager.playerMovementSpeed * Time.deltaTime);

            // Clamp the player's position within the bounds
            newPosition.y = Mathf.Clamp(newPosition.y, minYPosition, maxYPosition);

            // Update the player's position
            transform.position = newPosition;
        }
    }
    
    
}