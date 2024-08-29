using System;
using System.Collections.Generic;
using Core.Managers;
using Core.ObstacleGeneration;
using Extentions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace ObstacleGeneration
{
    public class Obstacle : MonoBehaviour, Resetable
    {
        public Vector3 RightMostPosition => rightMostPosition.position;
        public int Difficulty
        {
            get => difficulty;
            private set => difficulty = value;
        }
        
        public PoolType PoolType
        {
            get => type;
            private set => type = value;
        }
        [SerializeField] private int difficulty;
        [SerializeField] private PoolType type;
        [SerializeField] private List<ObstacleComponent> obstacleComponents;
        [SerializeField] private Transform rightMostPosition;
        
        public void ResetGameObject()
        {
            foreach (var part in obstacleComponents)
            {
                part.ResetGameObject();
            }
        }
        
        void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {
            Move();
        }

        public void ChangeStyle()
        {
            foreach (var part in obstacleComponents)
            {
                part.ChangeStyle();
            }
        }

        public void Move()
        {
            transform.position -= new Vector3(CoreManager.instance.ObstacleManager.BaseSpeed * Time.deltaTime, 0, 0);
        }

        public void ChangeColor()
        {
            Color[] shuffledColors = UtilityFunctions.ShuffleArray(CoreManager.instance.StyleManager.GetStyle().ColorPalette);
            int currentColorIndex = 0;
            for (int i = 0; i < obstacleComponents.Count; ++i)
            {
                currentColorIndex = obstacleComponents[i].SetColor(shuffledColors, currentColorIndex);
            }
        }
    }
}
