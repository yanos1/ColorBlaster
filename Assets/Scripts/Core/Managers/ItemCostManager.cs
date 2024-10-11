using System;
using System.Collections.Generic;
using Core.GameData;
using UnityEngine;

namespace Core.Managers
{
    [System.Serializable]
    public class ItemCost
    {
        public string item; // Store the item as a string during deserialization
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
            
            
            foreach (var itemCost in itemList.itemCosts)
            {
                // Try to parse the string to the enum
                if (Enum.TryParse(itemCost.item, out Item parsedItem))
                {
                    itemCosts[parsedItem] = itemCost.cost;
                }
                else
                {
                    Debug.Log($"Item '{itemCost.item}' could not be parsed to enum.");
                }
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