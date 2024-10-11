using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Managers;
using Extentions;
using GameLogic.ConsumablesGeneration;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoosterUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<BuffUI> buffsUI;

    private Dictionary<Color, BuffUI> buffToUIMap; // this will let us know if a uiBuff is already active for a cetrain buff

    void Start()
    {
        buffToUIMap = new Dictionary<Color, BuffUI>();
    }

    private void OnEnable()
    {
        CoreManager.instance.EventManager.AddListener(EventNames.ColorRushPrefabArrived, ActivateBar);
        CoreManager.instance.EventManager.AddListener(EventNames.ShieldPrefabArrived, ActivateBar);
        CoreManager.instance.EventManager.AddListener(EventNames.GemPrefabArrived, ActivateBar);
        CoreManager.instance.EventManager.AddListener(EventNames.DeleteColorPrefabArrived, ActivateBar);
        CoreManager.instance.EventManager.AddListener(EventNames.DeactivateColorRush, DeactivateBar);
        CoreManager.instance.EventManager.AddListener(EventNames.DeactivateShield, DeactivateBar);
        CoreManager.instance.EventManager.AddListener(EventNames.DeactivateGemRush, DeactivateBar);
        CoreManager.instance.EventManager.AddListener(EventNames.DeactivateDeleteColor, DeactivateBar);
        CoreManager.instance.EventManager.AddListener(EventNames.EndRun, DeactivateUI);

    }

    private void OnDisable()
    {
        CoreManager.instance.EventManager.RemoveListener(EventNames.ColorRushPrefabArrived, ActivateBar);
        CoreManager.instance.EventManager.RemoveListener(EventNames.ShieldPrefabArrived, ActivateBar);
        CoreManager.instance.EventManager.RemoveListener(EventNames.GemPrefabArrived, ActivateBar);
        CoreManager.instance.EventManager.RemoveListener(EventNames.DeleteColorPrefabArrived, ActivateBar);
        CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateDeleteColor, DeactivateBar);
        CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateColorRush, DeactivateBar);
        CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateShield, DeactivateBar);
        CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateGemRush, DeactivateBar);
        CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, DeactivateUI);
    }

    private void DeactivateUI(object obj)
    {
        foreach (var buffUI in buffsUI)
        {
            buffUI.gameObject.SetActive(false);
        }
    }

    private void DeactivateBar(object obj)
    {
        if (obj is Color color)
        {
            print("DEACTIVATE BAT!!fff");
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
    }



    private void ActivateBar(object obj)
    {
        if (obj is (Color color, float value, Booster buff))
        {
            foreach (var (_color, buffUI) in buffToUIMap)
            {
                print("EXISTED!");

                if (UtilityFunctions.CompareColors(_color, color))
                {
                    buffUI.IncreaseBuff(value);
                    return;
                }
            }

            Sprite buffSprite = buff.UIImage.sprite;
            foreach (var buffUI in buffsUI)
            {
                if (!buffUI.gameObject.activeInHierarchy)
                {
                    buffUI.gameObject.SetActive(true);
                    buffToUIMap[color] = buffUI;

                    buffUI.InitBuff(buffSprite,color,value);
                    print("DID NOT EXIST");

                    return;
                }
            }
        }
    }
}