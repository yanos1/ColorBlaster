namespace Core.PlayerRelated
{
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 10f; // Speed of the player's movement
        public float maxYPosition = 5f; // Maximum Y position the player can move to
        public float minYPosition = -5f; // Minimum Y position the player can move to

        public void MoveUp()
        {
            Move(Vector3.up);
        }

        public void MoveDown()
        {
            Move(Vector3.down);
        }

        private void Move(Vector3 direction)
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