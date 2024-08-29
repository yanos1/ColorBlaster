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
<<<<<<< Updated upstream
        [SerializeField] private List<ObstacleComponent> obstacleComponents;
=======
        [SerializeField] private List<ObstacleComponent> obstacleParts;
>>>>>>> Stashed changes
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

        public void ApplyStyle()
        {
            foreach (var part in obstacleComponents)
            {
                part.ApplyStyle();
            }
        }

        public void Move()
        {
            transform.position -= new Vector3(CoreManager.instance.ObstacleManager.BaseSpeed * Time.deltaTime, 0, 0);
        }

        public void ChangeColors()
        {
            Color[] shuffledColors = UtilityFunctions.ShuffleArray(CoreManager.instance.StyleManager.GetStyle().ColorPalette);
<<<<<<< Updated upstream
            int currentColorIndex = 0;
            for (int i = 0; i < obstacleComponents.Count; ++i)
            {
                currentColorIndex = obstacleComponents[i].SetColor(shuffledColors, currentColorIndex);
=======
            int curIndex = 0;
            for (int i = 0; i < obstacleParts.Count; ++i)
            {
               curIndex =  obstacleParts[i].SetColor(shuffledColors, curIndex);
                
>>>>>>> Stashed changes
            }
        }
    }
}
