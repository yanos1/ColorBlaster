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

        public CoreManager()
        {
            if (instance != null)
            {
                throw new InvalidOperationException("CoreManager instance already exists.");
            }
            instance = this;
            
            // Initilize CoreManagers
            ControlPanelManager = new ControlPanelManager();
            EventManager = new EventManager();
            SaveManager = new SaveManager();
            UserDataManager = new UserDataManager(SystemInfo.deviceUniqueIdentifier);
            MonoRunner = new GameObject("CoreManagerRunner").AddComponent<MonoRunner>();
        }

        public void InitializeManagers(TextAsset itemCosts, Style[] styles,List<ColorTheme> colorThemes, Obstacle[] obstacles, PoolEntry[] poolEntries, TreasureChestBuff[] rewards, float baseObjectSpeed, Action onComplete, GameLoaderUI loaderUI)
        {
            // Initialize all the managers here
            
            GameManager = new GameManager();
            loaderUI.AddProgress(10);
            TimeManager = new TimeManager();
            loaderUI.AddProgress(10);

            StyleManager = new StyleManager(styles);
            loaderUI.AddProgress(10);

            ColorsManager = new ColorsManager(colorThemes);
            loaderUI.AddProgress(10);

            BuffManager = new BuffManager(rewards);
            loaderUI.AddProgress(10);

            ObstacleManager = new ObstacleManager(obstacles);
            PoolManager = new ObjectPoolManager(poolEntries);
            CostManager = new ItemCostManager(itemCosts);
            
            

            // Notify that initialization is complete
            onComplete?.Invoke();
        }
    }
}