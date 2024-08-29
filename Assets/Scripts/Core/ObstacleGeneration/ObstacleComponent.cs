using System.Collections.Generic;
using Core.ObstacleGeneration;
using UnityEngine;

namespace ObstacleGeneration
{
    public class ObstacleComponent : MonoBehaviour, Resetable
    {
        [SerializeField] private List<ObstaclePart> parts;

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
                part.Renderer.color = colors[index++ % colors.Length];
            }

            return index;
        }
    }
}