using System.Collections.Generic;
using GameLogic.StyleRelated;
using Interfaces;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class ObstacleComponent : StyleableObject, IResettable
    {
        [SerializeField] private List<ObstaclePart> parts;

        [SerializeField] private bool isSingleColorComponent;

        

        public void ResetGameObject()
        {
            foreach (var part in parts)
            {
                part.ResetGameObject();
            }
        }

        public override void ChangeStyle()
        {
            foreach (var part in parts)
            {
                part.ApplyStyle();
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