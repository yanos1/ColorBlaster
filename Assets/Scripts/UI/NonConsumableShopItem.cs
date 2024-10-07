using Core.Managers;
using UnityEngine.UI;

namespace UI
{
    public class NonConsumableShopItem : ShopItem
    {
        public Button equipButton;
        private bool isOwned;

        public override void Start()
        {
            base.Start();
        }

        public override bool BuyItemIfPossible()
        {
            if (base.BuyItemIfPossible())
            {
                CoreManager.instance.UserDataManager.AddItem(itemType, firebasePath);
                SetUnequipped();
            }

            return true;
        }

        public void SetEquipped()
        {
            equipButton.gameObject.SetActive(false);
            CoreManager.instance.UserDataManager.EquipItem(itemType, firebasePath);
        }

        public void SetUnequipped()
        {
            equipButton.gameObject.SetActive(true);
        }
    }
}