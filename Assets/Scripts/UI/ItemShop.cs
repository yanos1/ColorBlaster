using System;
using System.Collections.Generic;
using Core.GameData;
using Core.Managers;
using Extensions;
using Extentions;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class ItemShop : MonoBehaviour
    {
        [SerializeField] private SerializableTuple<Button, GameObject>[] buttonToPanelMap;

        // Start is called before the first frame update
        [SerializeField] private List<ConsumableShopItem> boosters;
        [SerializeField] private List<NonConsumableShopItem> avatars;
        [SerializeField] private List<NonConsumableShopItem> colorThemes;

        private UserDataManager _dataManager;
        // private Dictionary<int, NonConsumableShopItem>

        void Start()
        {
            _dataManager = CoreManager.instance.UserDataManager;

            InitializeButtonToPanelMap();
            InitializeNonConsumablesItems();
            InitializeConsumablesItems();
        }

        private void InitializeButtonToPanelMap()
        {
            foreach (SerializableTuple<Button, GameObject> kvp in buttonToPanelMap)
            {
                kvp.first.onClick.AddListener(delegate
                {
                    kvp.second.SetActive(true);
                    foreach (SerializableTuple<Button, GameObject> buttonToPanel in buttonToPanelMap)
                    {
                        if (kvp.first == buttonToPanel.first)
                        {
                            continue;
                        }

                        buttonToPanel.second.SetActive(false);
                    }
                });
            }

            buttonToPanelMap[0].first.onClick.Invoke();
        }

        private void InitializeConsumablesItems()
        {
            foreach (var booster in boosters)
            {
                print(booster.itemType);
                _dataManager.BoostersOwned.TryGetValue(booster.itemType, out int amount);
                
                booster.numberOwned.text = "Owned " +  amount;
            }
        }

        private void InitializeNonConsumablesItems()
        {
            InitItemType(avatars);
            InitItemType(colorThemes);
        }

        private void InitItemType(List<NonConsumableShopItem> shopItems)
        {
            foreach (var item in shopItems)
            {
                if (_dataManager.IsItemEquipped(item.itemType, item.firebasePath))
                {
                    item.SetEquipped();
                }

                if (_dataManager.HasItem(item.itemType, item.firebasePath))
                {
                    item.equipButton.gameObject.SetActive(true);
                }
            }
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}


public abstract class ShopItem : MonoBehaviour, IPurchasable
{
    public Item itemType;
    public Image image;
    public Button BuyButton;
    public int price;
    public TextMeshProUGUI priceText;
    public FirebasePath firebasePath;

    public virtual bool BuyItemIfPossible()

    {
        if (CoreManager.instance.UserDataManager.GemsOwned >= price)
        {
            
            CoreManager.instance.UserDataManager.AddGems(-price);
            
            CoreManager.instance.EventManager.InvokeEvent(EventNames.BoughtItem, price);
            return true;
        }
        else
        {
            //TODO add cant affoard juice
            return false;
        }
    }

    public virtual void Start()
    {
        BuyButton.onClick.AddListener(() => BuyItemIfPossible());
        price = CoreManager.instance.CostManager.GetItemCost(itemType);
        priceText.text = price.ToString();
    }
}
