using System.Collections.Generic;
using Extentions;
using GameLogic.Boosters;
using GameLogic.ConsumablesGeneration;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Managers
{
    public class BoosterUIManager : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private List<BuffUI> buffsUI;

        private Dictionary<Color, BuffUI>
            buffToUIMap; // this will let us know if a uiBuff is already active for a cetrain buff

        void Start()
        {
            buffToUIMap = new Dictionary<Color, BuffUI>();
        }


        public void DeactivateUI()
        {
            foreach (var buffUI in buffsUI)
            {
                buffUI.gameObject.SetActive(false);
            }
        }

        public void DeactivateBoosterUI(Color color)
        {
            foreach (var (_color, buffUI) in buffToUIMap)
            {
                if (UtilityFunctions.CompareColors(_color, color))
                {
                    buffUI.gameObject.SetActive(false);
                    buffToUIMap.Remove(_color);
                    return;
                }
            }
        }


        public void ActivateBooster(BoosterButtonController boosterButtonController, Color color)
        {
            foreach (var (_color, buffUI) in buffToUIMap)
            {
                if (UtilityFunctions.CompareColors(_color, color))
                {
                    buffUI.IncreaseBuff(boosterButtonController.Booster.duration);
                    return;
                }
            }

            Sprite boosterSprite = boosterButtonController.GetComponent<Image>().sprite;
            foreach (var buffUI in buffsUI)
            {
                if (!buffUI.gameObject.activeInHierarchy)
                {
                    buffUI.gameObject.SetActive(true);
                    buffToUIMap[color] = buffUI;

                    buffUI.InitBuff(boosterSprite, color, boosterButtonController.Booster.duration);
                    print("DID NOT EXIST");

                    return;
                }
            }
        }
    }
}