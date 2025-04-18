using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.SceneManagement;

public class IAPManager : MonoBehaviour, IDetailedStoreListener {

    public static IAPManager iapManager;
    public CoinTotal coinTotal;

    private void Awake() {
        if (iapManager == null) {
            DontDestroyOnLoad(gameObject);
            iapManager = this;
        }
        else if (iapManager != this) {
            Destroy(gameObject);
        }
    }

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public string Gold_500 = "gold_500";
    public string Gold_1500 = "gold_1500";
    public string Gold_5000 = "gold_5000";
    //public static string kProductIDNonConsumable = "nonconsumable";


    void Start() {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null) {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        GetCoinTotalGameObject();
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GetCoinTotalGameObject();
    }

    private void GetCoinTotalGameObject() {
        coinTotal = FindObjectOfType(typeof(CoinTotal)) as CoinTotal;
    }

    public void InitializePurchasing() {
        // If we have already connected to Purchasing ...
        if (IsInitialized()) {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        builder.AddProduct(Gold_500, ProductType.Consumable);
        builder.AddProduct(Gold_1500, ProductType.Consumable);
        builder.AddProduct(Gold_5000, ProductType.Consumable);
        // Continue adding the non-consumable product.
        //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    public bool IsInitialized() {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void Buy_Gold_500() {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(Gold_500);
    }
    public void Buy_Gold_1500() {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(Gold_1500);
    }
    public void Buy_Gold_5000() {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(Gold_5000);
    }

    public string GetProductPriceFromStore(string id) {
        if (m_StoreController!=null && m_StoreController.products!=null) {
            Debug.Log(m_StoreController.products.WithID(id).metadata.localizedPriceString);
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;
        }
        else {
            return "";
        }
    }

    /*
    public void BuyNonConsumable() {
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(kProductIDNonConsumable);
    }
    */

    void BuyProductID(string productId) {
        // If Purchasing has been initialized ...
        if (IsInitialized()) {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases() {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized()) {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error) {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, Gold_500, StringComparison.Ordinal)) {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameDataControl.gdControl.AddCoins(500);
            FindObjectOfType<SoundManager>().PlayOneShotSound("coinSFX");
            coinTotal.UpdateCoinsTotalTextAndFlyingTextAnim(500);
            SetNoAds();
            TrackGold500Purchases();
            GameDataControl.gdControl.SavePlayerData();
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, Gold_1500, StringComparison.Ordinal)) {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameDataControl.gdControl.AddCoins(1500);
            FindObjectOfType<SoundManager>().PlayOneShotSound("coinSFX");
            coinTotal.UpdateCoinsTotalTextAndFlyingTextAnim(1500);
            SetNoAds();
            TrackGold1500Purchases();
            GameDataControl.gdControl.SavePlayerData();
        }
        // Or ... a subscription product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, Gold_5000, StringComparison.Ordinal)) {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameDataControl.gdControl.AddCoins(5000);
            FindObjectOfType<SoundManager>().PlayOneShotSound("coinSFX");
            coinTotal.UpdateCoinsTotalTextAndFlyingTextAnim(5000);
            SetNoAds();
            TrackGold5000Purchases();
            GameDataControl.gdControl.SavePlayerData();
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

    private static void SetNoAds() {
        PlayerPrefs.SetInt("no_ads", 1);
    }

    private static void TrackGold500Purchases() {
        int purchaseCounter = PlayerPrefs.GetInt("Times_Purchased_Gold_500", 0);
        purchaseCounter++;
        PlayerPrefs.SetInt("Times_Purchased_Gold_500", purchaseCounter);
    }

    private static void TrackGold1500Purchases() {
        int purchaseCounter = PlayerPrefs.GetInt("Times_Purchased_Gold_1500", 0);
        purchaseCounter++;
        PlayerPrefs.SetInt("Times_Purchased_Gold_1500", purchaseCounter);
    }

    private static void TrackGold5000Purchases() {
        int purchaseCounter = PlayerPrefs.GetInt("Times_Purchased_Gold_5000", 0);
        purchaseCounter++;
        PlayerPrefs.SetInt("Times_Purchased_Gold_5000", purchaseCounter);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription) {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureDescription));
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message) {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }
}