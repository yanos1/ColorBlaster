// namespace Core.Managers
// {
// using System;
// using UnityEngine;
// using UnityEngine.Purchasing;
//
// public class IAPManager : MonoBehaviour, IStoreListener
// {
//     private static IAPManager instance;
//     private static IStoreController storeController;
//     private static IExtensionProvider storeExtensionProvider;
//
//     // Product identifiers for all the products to be handled.
//     // These identifiers should match the product IDs you set up in the Unity Dashboard.
//     public static string PRODUCT_100_COINS = "100_coins";
//     public static string PRODUCT_500_COINS = "500_coins";
//     public static string PRODUCT_NO_ADS = "no_ads";
//
//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
//
//     private void Start()
//     {
//         InitializePurchasing();
//     }
//
//     public void InitializePurchasing()
//     {
//         if (IsInitialized())
//         {
//             return;
//         }
//
//         var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
//
//         // Add products to the builder
//         builder.AddProduct(PRODUCT_100_COINS, ProductType.Consumable);
//         builder.AddProduct(PRODUCT_500_COINS, ProductType.Consumable);
//         builder.AddProduct(PRODUCT_NO_ADS, ProductType.NonConsumable);
//
//         UnityPurchasing.Initialize(this, builder);
//     }
//
//     private bool IsInitialized()
//     {
//         return storeController != null && storeExtensionProvider != null;
//     }
//
//     public void BuyProduct(string productId)
//     {
//         if (IsInitialized())
//         {
//             Product product = storeController.products.WithID(productId);
//
//             if (product != null && product.availableToPurchase)
//             {
//                 storeController.InitiatePurchase(product);
//             }
//             else
//             {
//                 Debug.LogError("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase.");
//             }
//         }
//         else
//         {
//             Debug.LogError("BuyProductID FAIL. Not initialized.");
//         }
//     }
//
//     public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//     {
//         storeController = controller;
//         storeExtensionProvider = extensions;
//     }
//
//     public void OnInitializeFailed(InitializationFailureReason error)
//     {
//         Debug.LogError($"OnInitializeFailed InitializationFailureReason:{error}");
//     }
//
//     public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//     {
//         if (string.Equals(args.purchasedProduct.definition.id, PRODUCT_100_COINS, StringComparison.Ordinal))
//         {
//             GrantCoins(100);
//         }
//         else if (string.Equals(args.purchasedProduct.definition.id, PRODUCT_500_COINS, StringComparison.Ordinal))
//         {
//             GrantCoins(500);
//         }
//         else if (string.Equals(args.purchasedProduct.definition.id, PRODUCT_NO_ADS, StringComparison.Ordinal))
//         {
//             DisableAds();
//         }
//         else
//         {
//             Debug.LogWarning($"ProcessPurchase: FAIL. Unrecognized product: {args.purchasedProduct.definition.id}");
//         }
//
//         return PurchaseProcessingResult.Complete;
//     }
//
//     public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//     {
//         Debug.LogError($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
//     }
//
//     private void GrantCoins(int amount)
//     {
//         // Add coins to the user's balance
//         Debug.Log($"Granted {amount} coins");
//         // Example:
//         // PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + amount);
//     }
//
//     private void DisableAds()
//     {
//         // Disable ads in the game
//         Debug.Log("Ads disabled");
//         // Example:
//         // PlayerPrefs.SetInt("NoAds", 1);
//     }
// }
//
// }