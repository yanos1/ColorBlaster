using System;
using System.Collections.Generic;
using Core.Managers;
using Extentions;
using GameLogic.StyleRelated;
using Interfaces;
using PoolTypes;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameLogic.ObstacleGeneration
{
    public class Obstacle : MoveableObject, IResettable
    {
        public Vector3 RightMostPosition => rightMostPosition.position;

        public int Difficulty
        {
            get => difficulty;
            private set => difficulty = value;
        }


        [SerializeField] private int difficulty;
        [SerializeField] private List<ObstacleComponent> obstacleComponents;
        [SerializeField] private Transform rightMostPosition;


        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void Update()
        {
            base.Update();
        }

        public void ResetGameObject()
        {
            foreach (var part in obstacleComponents)
            {
                part.ResetGameObject();
            }
        }


        // Update is called once per frame

        public void ChangeColors()
        {
            Color[] shuffledColors = UtilityFunctions.ShuffleArray(CoreManager.instance.ColorsManager.CurrentColors);
            int currentColorIndex = 0;
            for (int i = 0; i < obstacleComponents.Count; ++i)
            {
                currentColorIndex = obstacleComponents[i].SetColor(shuffledColors, currentColorIndex);
            }
        }

        public List<StyleableObject> ExtractStyleableObjects()
        {
            List<StyleableObject> allObstacleParts = new();
            foreach (var component in obstacleComponents)
            {
                foreach (var part in component.obstacleParts)
                {
                    allObstacleParts.Add(part);
                }
            }

            return allObstacleParts;
        }
    }
}