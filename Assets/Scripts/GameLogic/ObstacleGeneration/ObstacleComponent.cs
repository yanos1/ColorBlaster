using System.Collections.Generic;
using GameLogic.StyleRelated;
using Interfaces;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class ObstacleComponent :MonoBehaviour, IResettable
    {
        public List<ObstaclePart> obstacleParts => parts;
        
        [SerializeField] private List<ObstaclePart> parts;

        [SerializeField] private bool isSingleColorComponent;
        

        

        public void ResetGameObject()
        {
            foreach (var part in parts)
            {
                part.ResetGameObject();
            }
        }
        
        public int SetColor(Color[] colors, int index)
        {
            foreach (var part in parts)
            {
                part.SetColor(colors[index % colors.Length]);
                if (!isSingleColorComponent)
                {
                    index++;
                } 
            }

            return isSingleColorComponent ? index+1 : index;
        }

        
        
        
    }
}