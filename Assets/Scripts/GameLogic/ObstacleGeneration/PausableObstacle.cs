using System;
using Core.Managers;
using Unity.VisualScripting;

namespace GameLogic.ObstacleGeneration
{
    public class PausableObstacle : Obstacle
    {

        public override void Awake()
        {
            CanMove = false;
        }
        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.MoveObstacle, OnMoveObstacle);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.MoveObstacle, OnMoveObstacle);
        }

        public void OnMoveObstacle(object obj)
        {
            CanMove = true;
        }

        public override void ResetObstacle()
        {
            base.ResetObstacle();
            CanMove = false;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Move()
        {
            base.Move();
            base.Move();
            base.Move();
            base.Move();
        }
    }
}