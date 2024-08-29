using System;
using Core.PlayerRelated;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Managers
{
    public class CoreManager : MonoBehaviour
    {
        public static CoreManager instance;

        public GameManager GameManager;

        public EventManager EventManager;
        
        public ObjectPoolManager PoolManager;

        public StyleManager StyleManager;

        public ObstacleManager ObstacleManager;

        public ColorBlockManager ColorBlockManager;
        
        public Player Player;
        //

        //
        // public UIManager uiManager;
        //
        // public SoundManager soundManager;

        private void Awake()
        {
            if (instance != null)
            {
                return;
            }

            EventManager = new EventManager();
            instance = this;
        }
    }
}