using Core.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.ObstacleGeneration
{
    public class OffsettingObstacleComponent : ObstacleComponent
    {
        // Start is called before the first frame update

        private float direction;
        private static float OutOfScreenYValue => 5.4f;


        private void OnEnable()
        {
            transform.position = new Vector3(0, transform.position.y, 0); // reset for vertical movement
            direction = Random.value > 0.5 ? 1 : -1;
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < OutOfScreenYValue)
            {
                transform.position += Vector3.left * (direction * Time.deltaTime *
                                                      CoreManager.instance.ControlPanelManager.GetObstacleMovespeed(
                                                          CoreManager.instance.ControlPanelManager
                                                              .offsettingobstaclesMovespeed));
            }
        }
    }
}