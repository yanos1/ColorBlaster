using System;
using System.Collections.Generic;
using Core.Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.ObstacleGeneration
{
    public class ObstacleComponent : MonoBehaviour, Resetable
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

        public void ApplyStyle()
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