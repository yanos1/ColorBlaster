// using System.Collections.Generic;
// using UnityEngine;
//
// namespace GameLogic.ConsumablesGeneration
// {
//     public class PowerUpManager : MonoBehaviour
//     {
//         [SerializeField] private List<TreasureChestBuff> rewards;
//         private bool isPowerActive;
//
//
//         public void OpenChest(int strength)
//         {
//             TreasureChestBuff buff = GenerateReward();
//             if (buff != null)
//             {
//                 // Give the player the reward (coins or buff)
//                 buff.buffManager.ApplyBuff(strength);
//             }
//         }
//
//         private TreasureChestBuff GenerateReward()
//         {
//             float totalWeight = 0f;
//
//             // Calculate the total weight (sum of all drop chances)
//             foreach (var reward in rewards)
//             {
//                 totalWeight += reward.dropChance;
//             }
//
//             // Pick a random value based on the total weight
//             float randomValue = Random.Range(0f, totalWeight);
//             float cumulativeWeight = 0f;
//
//             // Find which reward corresponds to the random value
//             foreach (var reward in rewards)
//             {
//                 cumulativeWeight += reward.dropChance;
//                 if (randomValue <= cumulativeWeight)
//                 {
//                     return reward; // Return the chosen reward
//                 }
//             }
//
//             return null;  // Default return (should not happen if weights are set correctly)
//         }
//
//   
//     }
// }
