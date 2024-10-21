using UnityEngine;

namespace Interfaces
{
    public interface IObstacleElement
    {
        public void ResetObstacle();
        public int ChangeColors(Color[] colors, int index);
    }
}