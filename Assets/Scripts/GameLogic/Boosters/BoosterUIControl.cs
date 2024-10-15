using Core.Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.Boosters
{
    public class BoosterButtonController : MonoBehaviour
    {
        public Booster Booster => booster;
        public Button BoosterButton => button;
        public TextMeshProUGUI NumbersOwnedText => numbersOwnedText;
        
        [SerializeField] private Booster booster;

        [SerializeField] private Button button;

        [SerializeField] private TextMeshProUGUI numbersOwnedText;

        // Start is called before the first frame update

        // Update is called once per frame
        public void UpdateUI()
        {
            // Update numbers owned text based on the data in the ScriptableObject
            numbersOwnedText.text = CoreManager.instance.UserDataManager
                .GetBoosterAmount(booster.boosterType, booster.firebasePath).ToString();
        }
    }
}