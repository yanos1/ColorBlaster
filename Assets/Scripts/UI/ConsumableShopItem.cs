using System.Text.RegularExpressions;
using Core.Managers;
using TMPro;
using Unity.VisualScripting;

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
                CoreManager.instance.UserDataManager.AddBooster(itemType, 1,
                        () => numberOwned.text =
                            "Owned: " + (int.Parse(Regex.Match(numberOwned.text, @"\d+").ToString()) + 1)
                    );
            }

            return true;
        }
    }
}