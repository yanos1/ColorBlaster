using System.Collections.Generic;
using Core.ObstacleGeneration;
using UnityEngine;

namespace ObstacleGeneration
{
    public abstract class Obstacle : MonoBehaviour, Resetable
    {
        // Start is called before the first frame update
        [SerializeField] private PoolType type;
        [SerializeField] private List<ObstaclePart> parts;
        public int Difficulty { get; private set; }
        
        
        public abstract void Reset();

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}
