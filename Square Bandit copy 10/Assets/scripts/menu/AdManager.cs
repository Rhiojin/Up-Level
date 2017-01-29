using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

	public delegate void adMobReadyDelegate();
	public static event adMobReadyDelegate _AdMob_BannerReady;

	public delegate void adColonyAdChangeDelegate(bool avail, string adZ);
	public static event adColonyAdChangeDelegate _Adcolony_adColonyAdChange;

	public delegate void rewardVideoDelegate(int coins);
	public static event rewardVideoDelegate _Adcolony_didFinishVideo;
	private static bool gotReward = false;

	public static BannerView bannerView;

	[Header("Ad Networks - AdMob")]
	public string _iOS_adUnitId = "";
	public static string iOS_adUnitId = "";

	public string _Android_adUnitId = "";
	public static string Android_adUnitId = "";
	public static bool AdmobBannerReady = false;
	public bool _AdmobShowOnAwake = false;
	public static bool AdmobShowOnAwake = false;

	[Header("Ad Networks - AdColony")]

	public string _iOSAppID = "";
	public string _iOSVidZoneID = "";

	public string _AmazonAppID = "";
	public string _AmazonVidZoneID = "";

	public string _GooglePlayAppID = "";
	public string _GooglePlayVidZoneID = "";

	public static string ios_appID = "";
	public static string ios_vidZoneID = "";
	public static string amazon_appID = "";
	public static string amazon_vidZoneID = "";
	public static string GP_appID = "";
	public static string GP_vidZoneID = "";

	public static AdManager Instance;

	public static bool AmazonPlatform = false;

	void Awake()
	{
		if(Instance != null)
		{
			Destroy(gameObject);
			return;

		}
		Instance = this;
		DontDestroyOnLoad(gameObject);


	}

	void Start () 
	{
		iOS_adUnitId = _iOS_adUnitId;
		Android_adUnitId = _Android_adUnitId;

		ios_appID = _iOSAppID;
		ios_vidZoneID = _iOSVidZoneID;
		amazon_appID = _AmazonAppID;
		amazon_vidZoneID = _AmazonVidZoneID;
		GP_appID = _GooglePlayAppID;
		GP_vidZoneID = _GooglePlayVidZoneID;

		AdmobShowOnAwake = _AdmobShowOnAwake;

		AdmobSetup();
		AdcolonySetup();

	}


	private static void AdcolonySetup()
	{
		#if UNITY_IOS

		AdColony.Configure
		(
		"1.0",
		ios_appID,
		ios_vidZoneID
		);
		#elif UNITY_ANDROID

		if(AmazonPlatform)
		{
			//UNDERGROUND ***** shouldnt be used but just in case ****

			AdColony.Configure
			(
				"1.0",
				amazon_appID,
				amazon_vidZoneID
			);
		}

		else{
			AdColony.Configure
			(
				"1.0",
				GP_appID,
				GP_vidZoneID
			);
		}

		#endif
		AdColony.OnAdAvailabilityChange = _didGetNewAd;
		AdColony.OnV4VCResult = OnV4VCResult;
	}

	public static bool AdColony_AdAvailable()
	{
		string vidID = ios_vidZoneID;

		#if UNITY_ANDROID
		if(AmazonPlatform) vidID = amazon_vidZoneID;
		else vidID = GP_vidZoneID;
		#endif

		if(AdColony.IsVideoAvailable(vidID))
		{
			print ("Video ad available");
			return true;

		}
		else
		{
			print ("Video ad NOT available");
			return false;
		}

	}


	public static void AdColony_PlayVideoAd(Button button = null)
	{


		string vidID = ios_vidZoneID;

		#if UNITY_ANDROID
		if(AmazonPlatform) vidID = amazon_vidZoneID;
		else vidID = GP_vidZoneID;
		#endif
		if(AdColony.IsVideoAvailable(vidID))
		{
			Debug.Log("Play AdColony Video");
			// Call AdColony.ShowVideoAd with that zone to play an interstitial video.
			// Note that you should also pause your game here (audio, etc.) AdColony will not
			// pause your app for you.
			if(soundManager.instance)
			{
				soundManager.instance.ToggleSFX();
			}
			gotReward = false;
			AdColony.ShowV4VC(false,vidID);

		}
		else
		{
			Debug.Log("Video Not Available");
			if(button)button.interactable = false;
		}
	}

	public void OnVideoStarted()
	{
		print ("started AdColony vid");
	}

	public void OnVideoFinished(bool success)
	{

		if(success)
		{
			Debug.Log("video ad play success");
		}
		else
		{
			Debug.Log("video ad play failed");


		}

		Camera.main.Render();

	}

	public static void OnV4VCResult(bool success, string name, int amount)
	{
		if(success)
		{
			Debug.Log("V4VC success");
			Debug.Log("watched "+name+" for "+amount);

			_didWatchVideo();

		}
		else
		{
			Debug.Log("V4VC Failed");
		}

		Camera.main.Render();
		if(soundManager.instance)
		{
//			soundManager.instance.ToggleBGM();
			soundManager.instance.ToggleSFX();
		}
	}


	public static void _didWatchVideo()
	{
		if(!gotReward)
		{
			gotReward = true;
			if(_Adcolony_didFinishVideo != null)
			{
				_Adcolony_didFinishVideo(70);
			}
		}
	}

	public static void _didGetNewAd(bool available, string adZone)
	{
		if(_Adcolony_adColonyAdChange != null)
		{
			_Adcolony_adColonyAdChange(available, adZone);
		}
	}

	void AdmobSetup()
	{
		#if UNITY_ANDROID

		bannerView = new BannerView(Android_adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
//		bannerView.LoadAd(new AdRequest.Builder().AddTestDevice("41B8C5DD78D0159E2AA5D050D852F3D1").Build());
		bannerView.LoadAd(new AdRequest.Builder().Build());
		//if(showBannerOnAwake)bannerView.Show();


		#elif UNITY_IPHONE

		bannerView = new BannerView(iOS_adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
		bannerView.LoadAd(new AdRequest.Builder().Build());
		//if(showBannerOnAwake)bannerView.Show();



		#else
		string X_adUnitId = "unexpected_platform";

		bannerView = new BannerView(X_adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
		bannerView.LoadAd(new AdRequest.Builder().Build());
		//if(showBannerOnAwake)bannerView.Show();


		#endif

		if(AdmobShowOnAwake) Admob_ShowBannerAd();
//		else Admob_HideABannerAd();
		AdmobBannerReady = true;



		if(_AdMob_BannerReady != null)
		{
		_AdMob_BannerReady();
		}
	}

		public static void Admob_ShowBannerAd()
		{
		int b = PlayerPrefs.GetInt("removedAds",0);
//		print("showing banner ad "+b.ToString());
		if(b == 0) bannerView.Show();
		}

		public static void Admob_HideABannerAd()
		{
		int b = PlayerPrefs.GetInt("removedAds",0);
//		print("hiding banner ad "+b.ToString());
		bannerView.Hide();
		} 

		public static void Admob_DestroyBannerAd()
		{
		bannerView.Destroy();
		} 
}
