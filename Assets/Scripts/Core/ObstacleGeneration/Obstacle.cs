using System;
using System.Collections.Generic;
using Core.Managers;
using Core.ObstacleGeneration;
using Extentions;
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
        [SerializeField] private List<ObstaclePart> obstacleParts;
        [SerializeField] private Transform rightMostPosition;
        
        private List<ObstaclePart> inactiveParts;

        public void ResetGameObject()
        {
            foreach (var part in obstacleParts)
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
            foreach (var part in obstacleParts)
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
            Color[] colors = CoreManager.instance.StyleManager.GetStyle().ColorPalette;
            UtilityFunctions.ShuffleArray(colors);
     

            for (int i = 0; i < obstacleParts.Count; ++i)
            {
                obstacleParts[i].Renderer.material.color = colors[i% colors.Length];
            }
        }
    }
}
