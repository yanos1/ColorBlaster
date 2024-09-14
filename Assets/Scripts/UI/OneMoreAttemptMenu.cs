using System;
using Core.Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class OneMoreAttemptMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currency;

        [SerializeField] private TextMeshProUGUI coinsToRevive;
        [SerializeField] private Color insufficientCoinsColor;
        [SerializeField] private Color sufficientCoinsColor;

        private int costToReviveInt;

        // Start is called before the first frame update
        void Start()
        {
            Int32.TryParse(coinsToRevive.text, out costToReviveInt);
            coinsToRevive.text = CoreManager.instance.CostManager.GetItemCost(ItemType.Revive).ToString();
        }

        private void OnEnable()
        {
            currency.text = CoreManager.instance.UserDataManager.GemsOwned.ToString();
            coinsToRevive.color = CoreManager.instance.UserDataManager.GemsOwned < costToReviveInt
                ? insufficientCoinsColor
                : sufficientCoinsColor;
        }


        // Update is called once per frame
        public void ShowAdThenContinueGame()
        {
            // TODO
            // inset ad here 
            CoreManager.instance.EventManager.InvokeEvent(EventNames.Revive, null);
        }

        public void PayCoinsThenContinueGame()
        {
            if (Int32.TryParse(coinsToRevive.text, out var cost))
            {
                if (CoreManager.instance.UserDataManager.GemsOwned < cost)
                {
                    // TODO
                    // insert juice
                    return;
                }
                CoreManager.instance.TimeManager.ResumeTime();
                CoreManager.instance.UserDataManager.AddGems(-cost);
                CoreManager.instance.EventManager.InvokeEvent(EventNames.Revive, null);
                gameObject.SetActive(false);
            }
        }

        public void EndGame()
        {
            // TODO add close panel animation here
            gameObject.SetActive(false);
            CoreManager.instance.EventManager.InvokeEvent(EventNames.GameOver, null);
        }
    }
}