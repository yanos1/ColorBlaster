using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class RotatingObstacleComponent : ObstacleComponent
    {
        [SerializeField] private Vector3 RotationSpeed;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(RotationSpeed * Time.deltaTime);
        }
    }
}
