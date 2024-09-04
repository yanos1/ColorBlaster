using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

namespace Core.Managers
{
    public enum ItemType
    {
        Revive,
        Style1,
        Style2,
        Style3,
        Colors1,
        Colors2,
        Colors3,
        Character1,
        Character2,
        Character3,
        Boost1,
        Boost2,
        
        
        // Add other items here
    }

    [System.Serializable]
    public class ItemCost
    {
        public ItemType item; // Enum instead of string
        public int cost;
    }

    [System.Serializable]
    public class ItemCostList
    {
        public List<ItemCost> items;
    }

    public class ItemCostManager
    {
        public TextAsset ItemCostJson;

        private Dictionary<ItemType, int> itemCosts = new Dictionary<ItemType, int>();

        public ItemCostManager(TextAsset itemCostJson)
        {
            ItemCostJson = itemCostJson;
            LoadItemCosts();
        }

        private void LoadItemCosts()
        {
            ItemCostList itemList = JsonUtility.FromJson<ItemCostList>(ItemCostJson.text);
            foreach (var item in itemList.items)
            {
                itemCosts[item.item] = item.cost;
            }
        }

        public int GetItemCost(ItemType itemName) // Changed parameter to ItemType
        {
            if (itemCosts.TryGetValue(itemName, out int cost))
            {
                return cost;
            }
            else
            {
                Debug.LogError("Item cost not found!");
                return -1;
            }
        }
    }
}