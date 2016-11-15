using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;



public class IAPmanager : MonoBehaviour , IStoreListener
{
	public static IAPmanager instance;
	private static IStoreController m_StoreController;                                                                  // Reference to the Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;                                                         // Reference to store-specific Purchasing subsystems.

	// Product identifiers for all products capable of being purchased: "convenience" general identifiers for use with Purchasing, and their store-specific identifier counterparts 
	// for use with and outside of Unity Purchasing. Define store-specific identifiers also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

	private static string kProductIDConsumable =    "consumable";                                                         // General handle for the consumable product.
	private static string kProductIDNonConsumable = "nonconsumable";                                                  // General handle for the non-consumable product.
	private static string kProductIDSubscription =  "subscription";                                                   // General handle for the subscription product.

	private static string kProductNameAppleConsumable =    "com.unity3d.test.services.purchasing.consumable";             // Apple App Store identifier for the consumable product.
	private static string kProductNameAppleNonConsumable = "com.unity3d.test.services.purchasing.nonconsumable";      // Apple App Store identifier for the non-consumable product.
	private static string kProductNameAppleSubscription =  "com.unity3d.test.services.purchasing.subscription";       // Apple App Store identifier for the subscription product.

	private static string kProductNameGooglePlayConsumable =    "com.unity3d.test.services.purchasing.consumable";        // Google Play Store identifier for the consumable product.
	private static string kProductNameGooglePlayNonConsumable = "com.unity3d.test.services.purchasing.nonconsumable";     // Google Play Store identifier for the non-consumable product.
	private static string kProductNameGooglePlaySubscription =  "com.unity3d.test.services.purchasing.subscription";  // Google Play Store identifier for the subscription product.

	private static string product_homer = "homer";
	private static string product_homer_appleID = "kbl_homer";
	private static string product_homer_googleID = "kbl_homer";
	private static string product_homer_amazonID = "kbl_homer";

	private static string product_queensGuard = "queens guard";
	private static string product_queensGuard_appleID = "kbl_queens_guard";
	private static string product_queensGuard_googleID = "kbl_queens_guard";
	private static string product_queensGuard_amazonID = "kbl_queens_guard";

	private static string product_ronaldo = "ronaldo";
	private static string product_ronaldo_appleID = "kbl_ronaldo";
	private static string product_ronaldo_googleID = "kbl_ronaldo";
	private static string product_ronaldo_amazonID = "kbl_ronaldo";

	private static string product_doge = "doge";
	private static string product_doge_appleID = "kbl_doge";
	private static string product_doge_googleID = "kbl_doge";
	private static string product_doge_amazonID = "kbl_doge";

	private static string product_becks = "becks";
	private static string product_becks_appleID = "kbl_becks";
	private static string product_becks_googleID = "kbl_becks";
	private static string product_becks_amazonID = "kbl_becks";

	private static string product_karot = "karot";
	private static string product_karot_appleID = "kbl_karot";
	private static string product_karot_googleID = "kbl_karot";
	private static string product_karot_amazonID = "kbl_karot";

	private static string product_sanik = "sanik";
	private static string product_sanik_appleID = "kbl_sanik";
	private static string product_sanik_googleID = "kbl_sanik";
	private static string product_sanik_amazonID = "kbl_sanik";

	private static string product_hop_hop_ninja = "hop hop ninja";
	private static string product_hop_hop_ninja_appleID = "kbl_hop_hop_ninja";
	private static string product_hop_hop_ninja_googleID = "kbl_hop_hop_ninja";
	private static string product_hop_hop_ninja_amazonID = "kbl_hop_hop_ninja";

	private static string product_merica = "merica";
	private static string product_merica_appleID = "kbl_merica";
	private static string product_merica_googleID = "kbl_merica";
	private static string product_merica_amazonID = "kbl_merica";

	private static string product_pepe = "pepe";
	private static string product_pepe_appleID = "kbl_pepe";
	private static string product_pepe_googleID = "kbl_pepe";
	private static string product_pepe_amazonID = "kbl_pepe";

	private static string product_fullGame = "fullGame";
	private static string product_fullGame_appleID = "kbl_fullGame";
	private static string product_fullGame_googleID = "kbl_fullgame";
	private static string product_fullGame_amazonID = "kbl_fullgame";

	#region DELEGATES

	public delegate void PurchaseFullGameCompleteDelegate();
	public event PurchaseFullGameCompleteDelegate didBuyFullGame;

	public delegate void PurchaseCompleteDelegate(string purchase);
	public event PurchaseCompleteDelegate didCompletePurchase;

	public delegate void GetPriceDelegate(string product, string price);
	public event GetPriceDelegate didGetPrice;

	public delegate void StoreIsReady();
	public event StoreIsReady didReadyStore;

	public delegate void reportAchievementDelegate(bool success, string ID);
	public static event reportAchievementDelegate _GameCenter_didReportAchievment;

	private static bool gotReward = false;


	#endregion

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

	}

	void Start()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

//	public void Test()
//	{
//		if(didBuyFullGame != null)
//		{
//			didBuyFullGame();
//		}
//	}

	public void InitializePurchasing() 
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		// Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
		builder.AddProduct(product_homer, ProductType.NonConsumable, new IDs(){{ product_homer_appleID, AppleAppStore.Name },{ product_homer_googleID,  GooglePlay.Name },{ product_homer_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_queensGuard, ProductType.NonConsumable, new IDs(){{ product_queensGuard_appleID, AppleAppStore.Name },{ product_queensGuard_googleID,  GooglePlay.Name },{ product_queensGuard_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_ronaldo, ProductType.NonConsumable, new IDs(){{ product_ronaldo_appleID, AppleAppStore.Name },{ product_ronaldo_googleID,  GooglePlay.Name },{ product_ronaldo_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_doge, ProductType.NonConsumable, new IDs(){{ product_doge_appleID, AppleAppStore.Name },{ product_doge_googleID,  GooglePlay.Name },{ product_doge_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_becks, ProductType.NonConsumable, new IDs(){{ product_becks_appleID, AppleAppStore.Name },{ product_becks_googleID,  GooglePlay.Name },{ product_becks_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_karot, ProductType.NonConsumable, new IDs(){{ product_karot_appleID, AppleAppStore.Name },{ product_karot_googleID,  GooglePlay.Name },{ product_karot_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_sanik, ProductType.NonConsumable, new IDs(){{ product_sanik_appleID, AppleAppStore.Name },{ product_sanik_googleID,  GooglePlay.Name },{ product_sanik_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_hop_hop_ninja, ProductType.NonConsumable, new IDs(){{ product_hop_hop_ninja_appleID, AppleAppStore.Name },{ product_hop_hop_ninja_googleID,  GooglePlay.Name },{ product_hop_hop_ninja_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_merica, ProductType.NonConsumable, new IDs(){{ product_merica_appleID, AppleAppStore.Name },{ product_merica_googleID,  GooglePlay.Name },{ product_merica_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_pepe, ProductType.NonConsumable, new IDs(){{ product_pepe_appleID, AppleAppStore.Name },{ product_pepe_googleID,  GooglePlay.Name },{ product_pepe_amazonID,  AmazonApps.Name }});
		builder.AddProduct(product_fullGame, ProductType.NonConsumable, new IDs(){{ product_fullGame_appleID, AppleAppStore.Name },{ product_fullGame_googleID,  GooglePlay.Name },{ product_fullGame_amazonID,  AmazonApps.Name }});




		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize(this, builder);
	}


	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void RequestPrice(string product)
	{
		if(didGetPrice != null)
		{
			didGetPrice(product, m_StoreController.products.WithID(product).metadata.localizedPriceString);
		}

	}

	public void BuyConsumable()
	{
		// Buy the consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID(kProductIDConsumable);
	}


	public void BuyNonConsumable(string product)
	{
		// Buy the non-consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID(product);
		if(nativeAPImanager.instance) nativeAPImanager.instance.ShowAlert("loading...");
	}


	public void BuySubscription()
	{
		// Buy the subscription product using its the general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID(kProductIDSubscription);
	}


	void BuyProductID(string productId)
	{
		// If the stores throw an unexpected exception, use try..catch to protect my logic here.
		try
		{
			// If Purchasing has been initialized ...
			if (IsInitialized())
			{
				// ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
				Product product = m_StoreController.products.WithID(productId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if (product != null && product.availableToPurchase)
				{
					Debug.Log (string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
					m_StoreController.InitiatePurchase(product);
				}
				// Otherwise ...
				else
				{
					// ... report the product look-up failure situation  
					Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			// Otherwise ...
			else
			{
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}
		// Complete the unexpected exception handling ...
		catch (Exception e)
		{
			// ... by reporting any unexpected exception for later diagnosis.
			Debug.Log ("BuyProductID: FAIL. Exception during purchase. " + e);
		}
	}


	// Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
	public void RestorePurchases()
	{
		if(nativeAPImanager.instance) nativeAPImanager.instance.ShowAlert("restoring...");
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer || 
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}


	//  
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;

		if(didReadyStore != null)
		{
			didReadyStore();
		}
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		// A consumable product has been purchased by this user.
		print("product recieved "+args.purchasedProduct.definition.id);

		if(String.Equals(args.purchasedProduct.definition.id, product_fullGame, StringComparison.Ordinal))
		{
			print("sending out full game");
			if(didBuyFullGame != null)
			{
				didBuyFullGame();
			}
		}
		else if(didCompletePurchase != null)
		{
			print("sending out pack: "+args.purchasedProduct.definition.id);
			didCompletePurchase(args.purchasedProduct.definition.id);
		}
		else print("no methods subbed to purchasing delegate");





//		if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
//		{
//			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
//		}
//
//		// Or ... a non-consumable product has been purchased by this user.
//		else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
//		{
//			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//		}// Or ... a subscription product has been purchased by this user.
//		else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
//		{
//			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//		}// Or ... an unknown product has been purchased by this user. Fill in additional products here.
//		else 
//		{
//			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//		}// Return a flag indicating wither this product has completely been received, or if the application needs to be reminded of this purchase at next app launch. Is useful when saving purchased products to the cloud, and when that save is delayed.

		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId, failureReason));
	}

//	string GetProductName(string ID)
//	{
////		switch(ID)
////		{
////		case(""):
////			{
////				return "";
////			}
////			break;
////
////		case(""):
////			{
////				return "";
////			}
////			break;
////
////		case(""):
////			{
////				return "";
////			}
////			break;
////
////		case(""):
////			{
////				return "";
////			}
////			break;
////
////		case(""):
////			{
////				return "";
////			}
////			break;
////
////		case(""):
////			{
////				return "";
////			}
////			break;
////
////
////		case(""):
////			{
////				return "";
////			}
////			break;
////
////		default:
////			return "failed"; 
////			break;
////		}
//	}
}

