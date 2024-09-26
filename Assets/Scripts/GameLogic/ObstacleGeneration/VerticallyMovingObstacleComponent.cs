using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.ObstacleGeneration
{
    public class VerticallyMovingObstacleComponent : ObstacleComponent
    {
        // Start is called before the first frame update
        
        private float direction;
        private float speed;
        private static int OutOfScreenXValue => 10;

        private void Start()
        {
            speed = 1.2f;
        }

        private void OnEnable()
        {
            transform.position = new Vector3(0, transform.position.y, 0);  // reset for vertical movement
            direction = Random.value > 0.5 ? 1 : -1;
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < OutOfScreenXValue)
            {
                transform.position += Vector3.left* (direction * Time.deltaTime * speed);
            }
        }

    }
}
