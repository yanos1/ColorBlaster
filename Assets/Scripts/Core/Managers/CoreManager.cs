using System;
using System.Collections.Generic;
using Extentions;
using GameLogic.ConsumablesGeneration;
using GameLogic.ObstacleGeneration;
using GameLogic.PlayerRelated;
using LoaderLogic;
using ScriptableObjects;
using UnityEngine;

namespace Core.Managers
{
    public class CoreManager
    {
        public static CoreManager instance;
        public ControlPanelManager ControlPanelManager { get; private set; }
        public GameManager GameManager { get; private set; }
        public EventManager EventManager { get; private set; }
        public SaveManager SaveManager { get; private set; }
        public UserDataManager UserDataManager { get; private set; }
        public TimeManager TimeManager { get; private set; }
        public ObjectPoolManager PoolManager { get; private set; }
        public StyleManager StyleManager { get; private set; }
        public ColorsManager ColorsManager { get; private set; }
        public BuffManager BuffManager { get; private set; }
        public ObstacleManager ObstacleManager { get; private set; }
        public ItemCostManager CostManager { get; private set; }
        public Player Player { get; set; }
        public MonoRunner MonoRunner { get; private set; }

        public CoreManager(GameLoaderUI loaderUI)
        {
            if (instance != null)
            {
                throw new InvalidOperationException("CoreManager instance already exists.");
            }
            instance = this;
            
            // Initilize CoreManagers
            loaderUI.AddProgress(10);
            ControlPanelManager = new ControlPanelManager();
            loaderUI.AddProgress(20);
            EventManager = new EventManager();
            loaderUI.AddProgress(30);
            SaveManager = new SaveManager();
            loaderUI.AddProgress(30);
            UserDataManager = new UserDataManager(SystemInfo.deviceUniqueIdentifier);
            loaderUI.AddProgress(10);
            MonoRunner = new GameObject("CoreManagerRunner").AddComponent<MonoRunner>();
        }

        public void InitializeManagers(TextAsset itemCosts, Style[] styles,List<ColorTheme> colorThemes, Obstacle[] obstacles,Obstacle[] bossObstacles, PoolEntry[] poolEntries, TreasureChestBuff[] rewards, Action onComplete)
        {
            // Initialize all the managers here
            
            GameManager = new GameManager();
            TimeManager = new TimeManager();

            StyleManager = new StyleManager(styles);

            ColorsManager = new ColorsManager(colorThemes);

            BuffManager = new BuffManager(rewards);

            ObstacleManager = new ObstacleManager(obstacles, bossObstacles);
            PoolManager = new ObjectPoolManager(poolEntries);
            CostManager = new ItemCostManager(itemCosts);
            
            

            // Notify that initialization is complete
            onComplete?.Invoke();
        }
    }
}