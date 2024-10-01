﻿namespace GameLogic.ObstacleGeneration
{
    public class SpecialMovingObstacle : Obstacle
    {
        // movement will be controlled from the obstacle parts.
        public void OnEnable()
        {
            MoveSpeed = 0;
        }

        public override void Update()  // why update needs to eb called? rockets are using Move with 0 ms but still moving!
        {
            
        }
    }
}