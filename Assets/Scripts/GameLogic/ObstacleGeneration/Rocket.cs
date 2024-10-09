using System.Collections;
using Core.Managers;
using Extentions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.ObstacleGeneration
{
    public class Rocket : ObstaclePart
    {
        [SerializeField] private Vector2 spawnYPositionRange;
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private SpriteRenderer _alertRenderer;
        [SerializeField] private float frequency; // Frequency of the sine wave motion
        [SerializeField] private float amplitude; // Amplitude of the sine wave motion

        private float imageFadeDuration;
        private int numOfAlerts;
        private int startDirection;
        private float speedMultiplier; // Multiplier of the baseobstacleSpeed.


        public override void Awake()
        {
            base.Awake();
            trail = GetComponent<TrailRenderer>();
            imageFadeDuration = 0.5f;
            numOfAlerts = 3;
            startDirection = Random.value > 0.5 ? 1: -1;
            speedMultiplier = 8f;
        }

        
        private IEnumerator AlertThenLaunch()
        { 
            for (int i = 0; i < numOfAlerts; i++)
            {
                yield return StartCoroutine(UtilityFunctions.FadeImage(_alertRenderer, 0, 1, imageFadeDuration));

                yield return StartCoroutine(UtilityFunctions.FadeImage(_alertRenderer, 1, 0, imageFadeDuration));

            }

            yield return StartCoroutine(Launch());
        }

        private IEnumerator Launch()
        {
            while (gameObject.activeInHierarchy)
            {
                Vector3 forwardMovement = Vector3.down * (CoreManager.instance.ControlPanelManager.GetGameMoveSpeed() * speedMultiplier * Time.deltaTime);
                // Vector3 sineWaveMotion = Vector3.left * (Mathf.Sin(Time.deltaTime * frequency*startDirection) * amplitude);

                transform.position += forwardMovement;

                yield return null;
            }
        }

        public override void ResetGameObject()
        {
            base.ResetGameObject();
            StopAllCoroutines();
            transform.position = new Vector3(Random.Range(spawnYPositionRange.x, spawnYPositionRange.y),
                transform.parent.parent.position.y, 0);   // I DONT LIKE THIS AT ALL
            trail.startColor = GetColor();
            startDirection = Random.value > 0.5 ? 1 : -1;
            StartCoroutine(AlertThenLaunch());
        }
    }
}