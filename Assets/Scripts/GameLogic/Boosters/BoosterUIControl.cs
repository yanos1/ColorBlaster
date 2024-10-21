using System;
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
        public TextMeshProUGUI NumberObstaclesToOpen => numberObstaclesToOpen;

        [SerializeField] private Booster booster;

        [SerializeField] private Button button;

        [SerializeField] private TextMeshProUGUI numbersOwnedText;
        [SerializeField] private TextMeshProUGUI numberObstaclesToOpen;

        // Start is called before the first frame update

        // Update is called once per frame

        public void UpdateNumbersOwned()
        {
            // Update numbers owned text based on the data in the ScriptableObject
            numbersOwnedText.text = CoreManager.instance.UserDataManager
                .GetBoosterAmount(booster.boosterType, booster.firebasePath).ToString();
        }

        public void UpdateObstaclesToOpen()
        {
            Activate();
            int newNumber = int.Parse(numberObstaclesToOpen.text) - 1;
            numberObstaclesToOpen.text = newNumber.ToString();
        }


        public void UpdateObstaclesToOpen(int amount)
        {
            Activate();
            numberObstaclesToOpen.text = amount.ToString();
        }

        private void Activate()
        {
            if (!numberObstaclesToOpen.gameObject.activeInHierarchy)
            {
                numberObstaclesToOpen.gameObject.SetActive(true);
            }
        }
    }
}