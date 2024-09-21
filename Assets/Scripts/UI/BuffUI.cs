using Extentions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuffUI : MonoBehaviour
    {
        public Image Bar => bar;
        public Image Icon => icon;

        [SerializeField] private Image bar;
        [SerializeField] private Image icon;

        private Coroutine _fillRoutine;
        private float _maxDuration = 35f;

        // Update is called once per frame

        public void InitBuff(Sprite sprite, Color color, float seconds)
        {
            bar.color = color;
            icon.sprite = sprite;
            icon.color = color;
            bar.fillAmount = seconds / _maxDuration;
            print(seconds);
            print(bar.fillAmount);
            _fillRoutine = StartCoroutine(UtilityFunctions.ReduceFillRoutine(bar, bar.fillAmount, seconds));
        }

        public void IncreaseBuff(float seconds)
        {
            StopCoroutine(_fillRoutine);

            _fillRoutine = null;
            bar.fillAmount += seconds / _maxDuration;
            print(seconds);
            print(bar.fillAmount);
            _fillRoutine = StartCoroutine(UtilityFunctions.ReduceFillRoutine(bar, bar.fillAmount,
                seconds + bar.fillAmount * _maxDuration));
        }
    }
}