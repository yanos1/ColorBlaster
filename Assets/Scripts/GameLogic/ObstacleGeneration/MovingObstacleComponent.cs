using System;
using System.Collections;
using Core.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.ObstacleGeneration
{
    public class MovingObstaclePart : ObstaclePart
    {
        [SerializeField] private int direction;
        private float speed;
        private float distanceBetweenCenterAndEdge;

        // Start is called before the first frame update
        void Start()
        {
            speed = CoreManager.instance.ControlPanelManager.movingObstaclesMovespeed;
            distanceBetweenCenterAndEdge = GetComponent<Collider2D>().bounds.max.x - transform.position.x;
            StartCoroutine(CheckIfOutOfBounds());
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += Vector3.left * (direction * Time.deltaTime *
                                                  CoreManager.instance.ControlPanelManager.GetObstacleSpeedMuliplier(
                                                      speed));
        }

        private IEnumerator CheckIfOutOfBounds()
        {
            while (isActiveAndEnabled)
            {
                print(
                    $"edge position right : {transform.position.x + distanceBetweenCenterAndEdge}" +
                    $"edge position left : {transform.position.x - distanceBetweenCenterAndEdge}" +
                    $"  camera edge Right: {CameraManager.instance.ReturnRightMostPosition().x} " +
                    $"camera edge left: {CameraManager.instance.ReturnLeftMostPosition().x}");
                if (transform.position.x + distanceBetweenCenterAndEdge >
                    CameraManager.instance.ReturnRightMostPosition().x ||
                    transform.position.x - distanceBetweenCenterAndEdge <
                    CameraManager.instance.ReturnLeftMostPosition().x)
                {
                    print("SWAP DIRECTION!");
                    direction *= -1;
                }

                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}