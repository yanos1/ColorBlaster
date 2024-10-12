using Core.Managers;
using Extentions;
using GameLogic.Boosters;
using GameLogic.ConsumablesGeneration;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class Player : MonoBehaviour
    {
        public Shooter Shooter => shooter;
        public PlayerMovement PlayerMovement => playerMovement;
        

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        [SerializeField] public TouchInputManager inputManager;
        [SerializeField] private Shooter shooter;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Shield shieldBuff;

        private Vector3 startPos;
        private bool isDead;
        private static float fallMagnitude => 2f;
        private static float outOfBounds => -6.5f;
        private static float reviveDuration => 0.5f; // time for player to return to position after death


        private Rigidbody2D rb;


        private void Awake()
        {
            CoreManager.instance.Player = this;
            rb = GetComponent<Rigidbody2D>();
            startPos = transform.position;
            isDead = false;
            shooter.Init(inputManager);
            playerMovement.Init(inputManager);
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.KillPlayer, Fall);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, MakeAlive);
            CoreManager.instance.EventManager.AddListener(EventNames.Revive, ResetGameObject);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateShield, ActivateShield);
            CoreManager.instance.EventManager.AddListener(EventNames.DeactivateShield, DeactivateShield);

        }
 
        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.KillPlayer, Fall);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, MakeAlive);
            CoreManager.instance.EventManager.RemoveListener(EventNames.Revive, ResetGameObject);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateShield, ActivateShield);
            CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateShield, DeactivateShield);

        }

        private void DeactivateShield(object obj)
        {
            shieldBuff.gameObject.SetActive(false);
        }

        private void ActivateShield(object obj)
        {
            print("ATTEMPT TO ACTIVE SHIELD 222");
            if (obj is (Color color, float duration, BoosterButtonController buff))
            {
                print("SET SHIELD ACTIVE222" );
                shieldBuff.gameObject.SetActive(true);
                shieldBuff.SetColor(color);
            }
        }
        
        

        private void Fall(object obj)
        {
            isDead = true;
            StartCoroutine(UtilityFunctions.MoveObjectOverTime(
                gameObject, 
                transform.position, 
                Quaternion.identity,
                transform.position + Vector3.up * GetFallDistance(), 
                Quaternion.identity, 
                reviveDuration, 
                () => 
                {
                    CoreManager.instance.EventManager.InvokeEvent(EventNames.EndRun, null);
                }
            ));
        }

        private float GetFallDistance()
        {
            return outOfBounds - transform.position.y;
        }


        public void ResetGameObject(object obj)
        {
            StartCoroutine(UtilityFunctions.MoveObjectOverTime(gameObject, transform.position, Quaternion.identity,
                startPos, Quaternion.identity, reviveDuration));
        }

        private void MakeAlive(object obj)
        {
            isDead = false;
        }
    }
}