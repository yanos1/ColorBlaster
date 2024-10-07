using System.Collections.Generic;
using Core.GameData;
using UnityEngine;

namespace Core.Managers
{
    [System.Serializable]
    public class ItemCost
    {
        public Item item; // Enum instead of string
        public int cost;
    }

    [System.Serializable]
    public class ItemCostList
    {
        public List<ItemCost> itemCosts; // Wrapper for deserialization
    }

    public class ItemCostManager
    {
        public TextAsset ItemCostJson;

        private Dictionary<Item, int> itemCosts = new Dictionary<Item, int>();

        public ItemCostManager(TextAsset itemCostJson)
        {
            ItemCostJson = itemCostJson;
            LoadItemCosts();
        }

        private void LoadItemCosts()
        {
            ItemCostList itemList = JsonUtility.FromJson<ItemCostList>(ItemCostJson.text);
            foreach (var item in itemList.itemCosts)
            {
                itemCosts[item.item] = item.cost;
            }
        }

        public int GetItemCost(Item itemName) // Changed parameter to ItemType
        {
            if (itemCosts.TryGetValue(itemName, out int cost))
            {
                return cost;
            }
            else
            {
                Debug.LogError("Item cost not found!");
                return -1; // Or throw exception / use Nullable<int>
            }
        }
    }
}