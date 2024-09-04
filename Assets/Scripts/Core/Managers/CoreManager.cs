using System;
using Core.PlayerRelated;
using Extentions;
using ObstacleGeneration;
using ScriptableObjects;
using UnityEngine;

namespace Core.Managers
{
    public class CoreManager
    {
        public static CoreManager instance;
        public GameManager GameManager { get; private set; }
        public EventManager EventManager { get; private set; }
        public SaveManager SaveManager { get; private set; }
        public TimeManager TimeManager { get; private set; }
        public ObjectPoolManager PoolManager { get; private set; }
        public StyleManager StyleManager { get; private set; }
        public CurrencyManager CurrencyManager { get; private set; }
        public ObstacleManager ObstacleManager { get; private set; }
        public ItemCostManager CostManager { get; private set; }
        public Player Player { get; set; }
        public MonoRunner MonoRunner { get; private set; }

        public CoreManager()
        {
            if (instance != null)
            {
                throw new InvalidOperationException("CoreManager instance already exists.");
            }
            instance = this;
            
            // Initilize CoreManagers
            EventManager = new EventManager();
            SaveManager = new SaveManager();
            MonoRunner = new GameObject("CoreManagerRunner").AddComponent<MonoRunner>();
        }

        public void InitializeManagers(TextAsset itemCosts, Style[] styles, Obstacle[] obstacles, PoolEntry[] poolEntries, float baseObstacleSpeed, Action onComplete)
        {
            // Initialize all the managers here
            GameManager = new GameManager();
            TimeManager = new TimeManager();
            StyleManager = new StyleManager(styles);
            CurrencyManager = new CurrencyManager();
            PoolManager = new ObjectPoolManager(poolEntries);
            ObstacleManager = new ObstacleManager(obstacles, baseObstacleSpeed);
            CostManager = new ItemCostManager(itemCosts);
            
            

            // Notify that initialization is complete
            onComplete?.Invoke();
        }
    }
}