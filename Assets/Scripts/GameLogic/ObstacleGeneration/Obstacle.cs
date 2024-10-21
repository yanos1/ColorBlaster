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
    public class Obstacle : MoveableObject, IObstacleElement
    {
    
        public List<ObstacleComponent> ObstacleComponents => obstacleComponents;
        public Vector3 RightMostPosition => rightMostPosition.position;

        public int Difficulty
        {
            get => difficulty;
            private set => difficulty = value;
        }

        public bool CanMove
        {
            get => _canMove;
            set => _canMove = value;
        }
        public bool Crossed
        {
            get => crossed;
            set => crossed = value;
        }

        public ObstacleType ObstacleType => obstacleType;

        [SerializeField] private ObstacleType obstacleType;
        [SerializeField] private int difficulty;
        [SerializeField] private List<ObstacleComponent> obstacleComponents;
        [SerializeField] private Transform rightMostPosition;

        private bool _canMove;
        private bool crossed;

        public virtual void Awake()
        {
            _canMove = true;
            crossed = false;
            SetParent();
        }
        public override void Update()
        {
            if (_canMove)
            {
                base.Update();
            }
        }

        public virtual void ResetObstacle()
        {
            crossed = false;
            foreach (var part in obstacleComponents)
            {
                part.ResetObstacle();
            }
            
      
        }


        public int ChangeColors(Color[] shuffledColors, int index)
        {
            int currentColorIndex = 0;
            for (int i = 0; i < obstacleComponents.Count; ++i)
            {
                currentColorIndex = obstacleComponents[i].ChangeColors(shuffledColors, currentColorIndex);
            }

            return 0;
        }

        public List<StyleableObject> ExtractObstacleParts()
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

        private void SetParent()
        {
            foreach (var component in obstacleComponents)
            {
                component.SetParent(this);
            }
        }

        public void OnComponentChange()
        {
            print("ALL GONE !");
            foreach (var part in ExtractObstacleParts())
            {
                if (part.gameObject.activeInHierarchy)
                {
                    return;
                }
            }
            CoreManager.instance.EventManager.InvokeEvent(EventNames.ObstacleCrossed, null);
            crossed = true;
        }
    }
}