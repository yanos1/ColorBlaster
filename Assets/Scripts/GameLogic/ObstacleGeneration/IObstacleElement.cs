using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public interface IObstacleElement
    {
        public void ResetObstacle();
        public int ChangeColors(Color[] colors, int index);
    }
}