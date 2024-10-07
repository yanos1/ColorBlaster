using Core.Managers;
using TMPro;

namespace UI
{
    public class ConsumableShopItem : ShopItem
    {
        public TextMeshProUGUI numberOwned;

        public override void Start()
        {
            base.Start();
        }

        public override bool BuyItemIfPossible()
        {
            if (base.BuyItemIfPossible())
            {
                CoreManager.instance.UserDataManager.AddBooster(itemType, 1);
                numberOwned.text = "Owned: " + (int.Parse(numberOwned.text) + 1);
            }

            return true;
        }
    }
}