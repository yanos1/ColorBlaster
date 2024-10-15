using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        #region INIT

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
                booster.buyButton.onClick.AddListener(() => BuyItemIfPossible(booster));

                _dataManager.BoostersOwned.TryGetValue(booster.itemType, out int amount);
                booster.numberOwned.text = "Owned " + amount;
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
                item.buyButton.onClick.AddListener(() => BuyItemIfPossible(item));
                item.EquipButton.onClick.AddListener(() => EquipNewItem(item));

                ConfigButton(item);
            }
        }

        private void ConfigButton(NonConsumableShopItem item)
        {
            bool hasItem = _dataManager.HasItem(item.itemType, item.firebasePath);

            bool itemEquipped = _dataManager.IsItemEquipped(item.itemType, item.firebasePath);


            if (!hasItem)
            {
                print($"{item} is not owned and not equipped");

                item.SetNotOwned();
            }

            else if (itemEquipped)
            {
                print($"{item} is owned and equipped");
                item.SetOwnedAndEquipped();
            }

            else
            {
                print($"{item} is owned but not equipped");
                item.SetOwned();
            }
        }

        #endregion

        public async virtual void BuyItemIfPossible<T>(T itemToBuy) where T : ShopItem
        {
            // Check if the user has enough gems
            if (CoreManager.instance.UserDataManager.GemsOwned >= itemToBuy.Price)
            {
                // Deduct the gems
                CoreManager.instance.UserDataManager.AddGems(-itemToBuy.Price);

                // Handle the specific item type
                if (itemToBuy is ConsumableShopItem consumable)
                {
                    // Add the consumable booster and update UI
                    CoreManager.instance.UserDataManager.AddBooster(consumable.itemType, 1,
                        () => consumable.numberOwned.text =
                            "Owned: " + (int.Parse(Regex.Match(consumable.numberOwned.text, @"\d+").ToString()) + 1)
                    );
                }
                else if (itemToBuy is NonConsumableShopItem nonConsumable)
                {
                    // Add the non-consumable item
                    await CoreManager.instance.UserDataManager.AddItem(nonConsumable.itemType,
                        nonConsumable.firebasePath);
                    nonConsumable.BuyItem();
                    // foreach (var item in GetShopItemsList(nonConsumable))
                    // {
                    //     item.ActivateEquipIfOwned();
                    // }
                }

                // Invoke the bought item event
                CoreManager.instance.EventManager.InvokeEvent(EventNames.BoughtItem, itemToBuy.Price);
            }
            else
            {
                // TODO: Add can't afford juice
            }
        }

        public void EquipNewItem(NonConsumableShopItem itemToEquip)
        {
            List<NonConsumableShopItem> items = GetShopItemsList(itemToEquip);
            foreach (var item in items)
            {
                if (item == itemToEquip)
                {
                    item.EquipItem();
                    continue;
                }

                if (item.IsOwned)
                {
                    item.EnableEquipButton();
                }
            }
        }


        private List<NonConsumableShopItem> GetShopItemsList(NonConsumableShopItem itemToBuy)
        {
            if (itemToBuy.firebasePath == UserDataManager.FirebasePath.avatarsOwned)
            {
                return avatars;
            }
            else
            {
                return colorThemes;
            }
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}


public abstract class ShopItem : MonoBehaviour
{
    public int Price => price;
    public Item itemType;
    [FormerlySerializedAs("BuyButton")] public Button buyButton;
    public Sprite sprite;
    private int price;
    public TextMeshProUGUI priceText;
    public UserDataManager.FirebasePath firebasePath;


    public virtual void Start()
    {
        price = CoreManager.instance.CostManager.GetItemCost(itemType);
        priceText.text = price.ToString();
    }
}