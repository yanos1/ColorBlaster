using UnityEngine;
using UnityEngine.UI;

namespace LoaderLogic
{
    public class GameLoaderUI : MonoBehaviour
    {
        [SerializeField] Image loaderFG;

        private int _progress;
        private int _targetProgress;

        public void Init(int target)
        {
            _targetProgress = target;
            _progress = 0;
            UpdateUI();
        }

        public void AddProgress(int progress)
        {
            SetProgress(_progress + progress);
        }

        public void SetProgress(int progress)
        {
            _progress = progress;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var percent = (float)_progress / _targetProgress;
            var percentClamp = Mathf.Clamp01(percent);
            if (loaderFG) loaderFG.fillAmount = percentClamp;
        }

        private void Reset()
        {
            loaderFG = GetComponent<Image>();
        }
    }
}