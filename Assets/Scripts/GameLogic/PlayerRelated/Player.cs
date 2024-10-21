using Core.Managers;
using Extentions;
using GameLogic.Boosters;
using GameLogic.Boosters.Gunners;
using GameLogic.ConsumablesGeneration;
using GameLogic.ObstacleGeneration;
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

        [SerializeField] private PlayerAvatarHandler avatarHandler;
        [SerializeField] public TouchInputManager inputManager;

        [SerializeField] private ColorWheel colorWheel;
        [SerializeField] private Shooter shooter;
        [SerializeField] private PlayerMovement playerMovement;

        //boosters
        [SerializeField] private Shield shieldBuff;
        [SerializeField] private GameObject machineGun;
        [SerializeField] private Gunners gunners;

        private Vector3 startPos;
        private bool isDead;
        private static float fallMagnitude => 2f;
        private static float outOfBounds => -6.5f;
        private static float reviveDuration => 0.5f; // time for player to return to position after death


        private Rigidbody2D rb;


        private void Awake()
        {
            CoreManager.instance.Player = this;
            avatarHandler.SetPlayerAvatar();
            rb = GetComponent<Rigidbody2D>();
            startPos = transform.position;
            isDead = false;
            gunners.Init(inputManager);
            playerMovement.Init(inputManager);
            shooter.Init(inputManager);
        }


        #region EVENTS

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, MakeAlive);
            CoreManager.instance.EventManager.AddListener(EventNames.Revive, ResetGameObject);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateShield, OnActivateShield);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateColorRush, OnActivateColorRush);
            CoreManager.instance.EventManager.AddListener(EventNames.DeactivateShield, OnDeactivateShield);
            CoreManager.instance.EventManager.AddListener(EventNames.DeactivateColorRush, OnDeactivateColorRush);
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateGunners, OnActivateGunners);
            CoreManager.instance.EventManager.AddListener(EventNames.DeactivateGunners, OnDeactivateGunners);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, MakeAlive);
            CoreManager.instance.EventManager.RemoveListener(EventNames.Revive, ResetGameObject);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateShield, OnActivateShield);
            CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateShield, OnDeactivateShield);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateColorRush, OnActivateColorRush);
            CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateColorRush, OnDeactivateColorRush);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateGunners, OnActivateGunners);
            CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateGunners, OnDeactivateGunners);
        }

        #endregion

        private void OnDeactivateGunners(object obj)
        {
            gunners.gameObject.SetActive(false);
        }

        private void OnActivateGunners(object obj)
        {
            gunners.gameObject.SetActive(true);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            // if (invincible) return;
            ObstaclePart part = other.GetComponent<ObstaclePart>();
            if (part is not null && !CoreManager.instance.Player.IsDead)
            {
                colorWheel.ShatterColorBlocks();
                Die();
                CoreManager.instance.EventManager.InvokeEvent(EventNames.KillPlayer, null);
            }

            Consumable consumable = other.GetComponent<Consumable>();
            if (consumable is not null)
            {
                consumable.Consume();
            }
        }

        private void OnDeactivateColorRush(object obj)
        {
            machineGun.gameObject.SetActive(false);
            colorWheel.gameObject.SetActive(true);
        }

        private void OnActivateColorRush(object obj)
        {
            machineGun.gameObject.SetActive(true);
            colorWheel.gameObject.SetActive(false);
        }

        private void OnDeactivateShield(object obj)
        {
            shieldBuff.gameObject.SetActive(false);
        }

        private void OnActivateShield(object obj)
        {
            print("ATTEMPT TO ACTIVE SHIELD 222");
            if (obj is (Color color, float duration, BoosterButtonController buff))
            {
                print("SET SHIELD ACTIVE222");
                shieldBuff.gameObject.SetActive(true);
                shieldBuff.ChangeColor(color);
            }
        }


        private void Die()
        {
            isDead = true;
            StartCoroutine(UtilityFunctions.MoveObjectOverTime(
                gameObject,
                transform.position,
                Quaternion.identity,
                transform.position + Vector3.up * GetFallDistance(),
                Quaternion.identity,
                reviveDuration,
                () => { CoreManager.instance.EventManager.InvokeEvent(EventNames.EndRun, null); }
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