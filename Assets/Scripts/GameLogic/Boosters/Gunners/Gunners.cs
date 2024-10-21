using Core.Managers;
using GameLogic.PlayerRelated;
using GameLogic.PlayerRelated.GameLogic.PlayerRelated;
using PoolTypes;
using UnityEngine;

namespace GameLogic.Boosters.Gunners
{
    public class Gunners : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Shooter gunner1;
        [SerializeField] private Shooter gunner2;

        private TouchInputManager _inputManager;


        public void Init(TouchInputManager inputManager)
        {
            _inputManager = inputManager;
        }
        void Awake()
        {
            gunner1.Init(_inputManager);
            gunner2.Init(_inputManager);
        }
        
    }
}
