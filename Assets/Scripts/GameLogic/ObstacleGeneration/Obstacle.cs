using System.Collections.Generic;
using Core.Managers;
using Extentions;
using GameLogic.StyleRelated;
using Interfaces;
using PoolTypes;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class Obstacle : StyleableObject, IResettable
    {
        public Vector3 RightMostPosition => rightMostPosition.position;

        public List<ObstacleComponent> ObstacleComponents => obstacleComponents;

        public int Difficulty
        {
            get => difficulty;
            private set => difficulty = value;
        }

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
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

        private float moveSpeed;
        private bool stop;

        public void ResetGameObject()
        {
            foreach (var part in obstacleComponents)
            {
                part.ResetGameObject();
            }
        }

        public virtual void OnEnable()
        {
            print("STARTEDDDDDDDDDDD");
            moveSpeed = CoreManager.instance.GameManager.CurrentObjectsSpeed;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            Move();
        }

        public override void ChangeStyle()
        {
            foreach (var part in obstacleComponents)
            {
                part.ChangeStyle();
            }
        }

        public void Move()
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }

        public void ChangeColors()
        {
            Color[] shuffledColors = UtilityFunctions.ShuffleArray(CoreManager.instance.ColorsManager.CurrentColors);
            int currentColorIndex = 0;
            for (int i = 0; i < obstacleComponents.Count; ++i)
            {
                currentColorIndex = obstacleComponents[i].SetColor(shuffledColors, currentColorIndex);
            }
        }
    }
}