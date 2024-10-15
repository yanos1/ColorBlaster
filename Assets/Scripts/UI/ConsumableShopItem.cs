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
        
    }
}