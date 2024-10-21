using System.Collections.Generic;
using GameLogic.StyleRelated;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class ObstacleComponent : MonoBehaviour, IObstacleElement
    {
        public List<ObstaclePart> obstacleParts => parts;

        [SerializeField] private List<ObstaclePart> parts;

        [SerializeField] private bool isSingleColorComponent;

        private Obstacle parent;

        public void ResetObstacle()
        {
            foreach (var part in parts)
            {
                part.ResetObstacle();
            }
        }

       

        public int ChangeColors(Color[] colors, int index)
        {
            foreach (var part in parts)
            {
                part.ChangeColor(colors[index % colors.Length]);
                if (!isSingleColorComponent)
                {
                    index++;
                }
            }

            return isSingleColorComponent ? index + 1 : index;
        }

        public void SetParent(Obstacle obstacle)
        {
            parent = obstacle;
            foreach (var part in obstacleParts)
            {
                part.SetParent(this);
            }
        }


        public void NotifyParent()
        {
            parent.OnComponentChange();
        }
    }
}