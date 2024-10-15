using Core.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NonConsumableShopItem : ShopItem
    {
        public bool IsOwned => isOwned;
        public Button EquipButton => equipButton;
        
        [SerializeField] private Button equipButton;
        [SerializeField] private GameObject equippedImage;

        private bool isOwned;
        public override void Start()
        {
            base.Start();
        }

        public void SetOwnedAndEquipped()
        {
            isOwned = true;
            equipButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);

            equippedImage.SetActive(true);
            equippedImage.transform.position = transform.position;
            Debug.Log("enable equip4 111");

        }

        public void ActivateEquipIfOwned()
        {
            Debug.Log("enable equip3 111");

            if (isOwned)
            {
                equipButton.gameObject.SetActive(true);
            }
        }

        public void BuyItem()
        {
            Debug.Log("111 bought item");
            isOwned = true;
            equipButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }

        public void EquipItem()
        {
            CoreManager.instance.UserDataManager.EquipItem(itemType, firebasePath);
            SetOwnedAndEquipped();
        }

        public void SetNotOwned()
        {
            buyButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
        }

        public void SetOwned()
        {
            Debug.Log("enable equip2 111");

            isOwned = true;
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);   // has item nad is not equipped
        }
        
        public void EnableEquipButton()
        {
            Debug.Log("enable equip 111");
            equipButton.gameObject.SetActive(true);
        }
    }
}