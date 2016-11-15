using UnityEngine;
using System.Collections;
using System.IO;

using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms.GameCenter;

public class nativeAPImanager : MonoBehaviour {

	public static nativeAPImanager instance;

	[DllImport ("__Internal")]
	private static extern void _ShowAlert(string msg);

	[DllImport ("__Internal")]
	private static extern void _VisitWebpage(string address);

	[DllImport ("__Internal")]
	private static extern void _DownloadGame(string gameId);

	[DllImport ("__Internal")]
	private static extern void _MoreGames();

	[DllImport ("__Internal")]
	private static extern void _RateUs(string appId);

	[DllImport ("__Internal")]
	private static extern void _ShareToFacebook(string _prefillmsg);

	[DllImport ("__Internal")]
	private static extern void _ShareToFacebookCard(string _prefillmsg);

	[DllImport ("__Internal")]
	private static extern void _ShareToTwitter(string _prefillmsg);

	[DllImport ("__Internal")]
	private static extern void _ShareToTwitterCard(string _prefillmsg);

	public bool AmazonPlatform = false;
	public string GooglePlay_leaderboardID;
	string androidPath = "";


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

	void Start () 
	{
		GameCenter_Authenticate();
		if(AmazonPlatform) PlayerPrefs.SetInt("amazonPlatform",1);
		else PlayerPrefs.SetInt("amazonPlatform",0);
	}

	public void VisitSocialNetwork(string network)
	{
		#if UNITY_IOS
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_ShowAlert("loading..");
			_VisitWebpage(network);
		}
		#elif UNITY_ANDROID
		string link = "";
		switch(network)
		{
		case("facebook"):
			{
				link = "https://www.facebook.com/nerdagencygames";
			}
			break;
		case("twitter"):
			{
				link = "https://www.twitter.com/nerdagencygames";
			}
			break;
		case("instagram"):
			{
				link = "https://www.instagram.com/nerdagencygames";
			}
			break;
		}

		AndroidAlertViewer("Loading...");
		if(AmazonPlatform)
		{
			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject appAct = jc.GetStatic<AndroidJavaObject>("currentActivity");

				using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.nerd.simpleplugin.SocialFunctions"))
				{
					bridgeClass.CallStatic("VisitSocialNetwork", appAct, link);
				}
			}
		}
		else
		{
			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
			AndroidJavaObject appAct = jc.GetStatic<AndroidJavaObject>("currentActivity");

				using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.nerd.simpleplugin.SocialFunctions"))
				{
					bridgeClass.CallStatic("VisitSocialNetwork", appAct, link);
				}
			}
		}
		#endif
	}


	public void ShowAlert(string msg)
	{
		#if UNITY_IOS
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_ShowAlert(msg);
		}
		#elif UNITY_ANDROID
		AndroidAlertViewer(msg);
		if(AmazonPlatform)
		{


		}
		else
		{


		}
		#endif
	}

	private static void AndroidAlertViewer(string msg)
	{
		using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject appCtx = jo.Call<AndroidJavaObject>("getApplicationContext");
			using (AndroidJavaClass mHumbleAssistantClass = new AndroidJavaClass("com.nerd.simpleplugin.Alerts"))
			{
				mHumbleAssistantClass.CallStatic("ShowAlert", jo,appCtx, msg);
			}
		}
	}


	public void CrossPromo_DownloadGame(string iOS_appId, string GooglePlay_appID, string Amazon_appID)
	{
		#if UNITY_IOS
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{

			iOS_appId = "id"+iOS_appId;

			//appId example: "id1024672326";

			_ShowAlert("loading..");
			_DownloadGame(iOS_appId);
		}
		#elif UNITY_ANDROID
		AndroidAlertViewer("Loading...");
		if(AmazonPlatform)
		{

			//appId example: "com.Nerd.RasherRush.underground");

			string bundle = Amazon_appID;
			string directPath = "amzn://apps/android?p="+bundle;
			string broadPath = "http://www.amazon.com/gp/mas/dl/android?p="+bundle;

			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject appAct = jc.GetStatic<AndroidJavaObject>("currentActivity");

				using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.nerd.simpleplugin.SocialFunctions"))
				{
					bridgeClass.CallStatic("DownloadGame", appAct, directPath, broadPath);
				}
			}
		}
		else
		{

			//appId example: "com.Nerd.RasherRush");

			string bundle = GooglePlay_appID;
			string directPath = "market://details?id="+bundle;
			string broadPath = "http://play.google.com/store/apps/details?id="+bundle;

			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject appAct = jc.GetStatic<AndroidJavaObject>("currentActivity");

				using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.nerd.simpleplugin.SocialFunctions"))
				{
					bridgeClass.CallStatic("DownloadGame", appAct, directPath, broadPath);
				}
			}
		}
		#endif
	}

	public void ShareTrophyRoom(string network)
	{
		print("starting save");
		StartCoroutine( SaveShot(network) );

	}

	void FinishImageShare(string network)
	{
		string msg = "Check out my awesome stats in #kickballlegend!";
		#if UNITY_EDITOR
		#endif
		#if UNITY_IOS
		if(network == "facebook")
		{
			_ShareToFacebook("Check out my awesome stats in #kickballlegend!");
		}
		else
		{
			_ShareToTwitter("Check out my awesome stats in #kickballlegend!");
		}
		#elif UNITY_ANDROID
		using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject appAct = jc.GetStatic<AndroidJavaObject>("currentActivity");

			using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.nerd.simpleplugin.SocialFunctions"))
			{
				bridgeClass.CallStatic("ShareGame", appAct, androidPath, msg);
			}
		}
		#endif
	}

	public void ShareScore(string network, int score)
	{
		
		string league = PlayerPrefs.GetString("myLeague","Jumpers for GoalPosts" );
		string msg = "I just scored "+score.ToString()+" in #kickballlegend "+league+" league! Beat that!";
		#if UNITY_IOS
		if(network == "facebook")
		{
			_ShareToFacebook("I just scored "+score.ToString()+"in #kickballlegend "+league+" league! Beat that!");
		}
		else
		{
			_ShareToTwitter("I just scored "+score.ToString()+"in #kickballlegend "+league+" league! Beat that!");
		}
		#elif UNITY_ANDROID
		using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject appAct = jc.GetStatic<AndroidJavaObject>("currentActivity");

			using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.nerd.simpleplugin.SocialFunctions"))
			{
				bridgeClass.CallStatic("SimpleSocialPost", appAct, msg);
			}
		}
		#endif
	}

	IEnumerator SaveShot(string network)
	{

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		int width = Screen.width;
		int height = Screen.height;


		Texture2D tex = new Texture2D(width,height,TextureFormat.RGB24,false);

		tex.ReadPixels(new Rect(0,0,width,height),0,0);

		byte[] texBytes = tex.EncodeToPNG();
		Destroy(tex);

		string imageName = "imageShare";
		int v = PlayerPrefs.GetInt("selfieCounter",0);
		string path = string.Empty;


		#if UNITY_EDITOR
		imageName += ".png";
		v++;
		PlayerPrefs.SetInt("selfieCounter",v);
		File.WriteAllBytes(Application.dataPath+"/screenshots/"+imageName,texBytes);

		#elif UNITY_IOS

		//#endif

		imageName += ".png";
		v++;
		PlayerPrefs.SetInt("selfieCounter",v);

		path = Application.persistentDataPath.Substring( 0, Application.persistentDataPath.Length - 5 );
		path = path.Substring( 0, path.LastIndexOf( '/' ) );
		path = (Path.Combine( Path.Combine( path, "Documents/" ), imageName ));

		PlayerPrefs.SetString("shotToSave",path);

		//print (path+"*********************");

		File.WriteAllBytes(path,texBytes);


		#endif

		#if UNITY_ANDROID
		imageName += v.ToString()+".png";
		v++;
		PlayerPrefs.SetInt("selfieCounter",v);
		print("attempting Android Save");

		path = Application.persistentDataPath;
		path = (Path.Combine( path, imageName ));

		PlayerPrefs.SetString("shotToSave",path);

		File.WriteAllBytes(path,texBytes);


		//Selfie_Save(path,imageName,selfieName);

		#endif
		androidPath = path;

		//*-------------------
		//Tell user that image is saved to album
		print("shot saved");

		FinishImageShare(network);
	}

	void DeleteImage()
	{
		File.Delete(PlayerPrefs.GetString("shotToSave"));
	}

	public void RateGame(string id)
	{
		#if UNITY_IOS
		_RateUs(id);
		#endif
		#if UNITY_ANDROID
		if(AmazonPlatform)
		{
			string bundle = id;
			string directPath = "amzn://apps/android?p="+bundle;
			string broadPath = "http://www.amazon.com/gp/mas/dl/android?p="+bundle;

			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject appAct = jc.GetStatic<AndroidJavaObject>("currentActivity");

				using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.nerd.simpleplugin.SocialFunctions"))
				{
					bridgeClass.CallStatic("RateUs", appAct, directPath, broadPath);
				}
			}
		}
		else
		{
			string bundle = id;
			string directPath = "market://details?id="+bundle;
			string broadPath = "http://play.google.com/store/apps/details?id="+bundle;

			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject appAct = jc.GetStatic<AndroidJavaObject>("currentActivity");

				using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.nerd.simpleplugin.SocialFunctions"))
				{
					bridgeClass.CallStatic("RateUs", appAct, directPath, broadPath);
				}
			}
		}
		#endif
	}

	private void GameCenter_Authenticate() 
	{	
		#if UNITY_IOS
		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		Social.localUser.Authenticate(ProcessAuthentication);
		#endif
		#if UNITY_ANDROID
		if(AmazonPlatform)
		{

		}
		else
		{
		Social.localUser.Authenticate(ProcessAuthentication);
		}
		#endif
	}

	private void Re_AuthenticateGameCenter() 
	{	
		#if UNITY_IOS
		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		Social.localUser.Authenticate(ProcessAuthentication);
		#endif
		#if UNITY_ANDROID
		if(AmazonPlatform)
		{
		bool usesLeaderboards = true;
		bool usesAchievements = true;
		bool usesWhispersync = false;

		//AGSClient.Init (usesLeaderboards, usesAchievements, usesWhispersync);

		}
		else
		{
		Social.localUser.Authenticate(ProcessAuthentication);
		}
		#endif
	}

	void ProcessAuthentication(bool auth_success)
	{
		if(auth_success)
		{
			PlayerPrefs.SetInt("gameCenterAuth",1);
			Debug.Log ("GC Authentication successful");

		}

		else
		{
			PlayerPrefs.SetInt("gameCenterAuth",0);
			Debug.Log ("Failed to authenticate GC");
		}


	}

	public void GameCenter_Show()
	{
		int gc = PlayerPrefs.GetInt("gameCenterAuth",0);
		if(gc==0)
		{
			Re_AuthenticateGameCenter();

			int gc2 = PlayerPrefs.GetInt("gameCenterAuth",0);
			if(gc2==1)
			{
				#if UNITY_IOS
				Social.ShowLeaderboardUI();
				#elif UNITY_ANDROID
				if(AmazonPlatform)
				{

//				if(AGSClient.IsServiceReady()) {
//				AGSLeaderboardsClient.ShowLeaderboardsOverlay();
//				}
//
//				else
//				{
//				AndroidAlertViewer("Game Circle is currently unavailable");
//				}
//
//				}
//				else
//				{
//				PlayGamesPlatform.Instance.ShowLeaderboardUI(GooglePlay_leaderboardID);
				}

				#endif
			}
			else
			{
				#if UNITY_IOS
				_ShowAlert("Game Center is currently Unavailable");
				#elif UNITY_ANDROID
				if(AmazonPlatform) AndroidAlertViewer("Game Circle is currently unavailable");
				else AndroidAlertViewer("Google Play Games is currently unavailable");
				#endif
			}
		}
		else
		{
			#if UNITY_IOS
			Social.ShowLeaderboardUI();
			#elif UNITY_ANDROID
			if(AmazonPlatform)
			{
//
//			if(AGSClient.IsServiceReady()) {
//			AGSLeaderboardsClient.ShowLeaderboardsOverlay();
//			}
//			else
//			{
//			AndroidAlertViewer("Game Circle is currently unavailable");
//			}
//
//			}
//			else
//			{
//			Social.ShowLeaderboardUI();
			}

			#endif
		}
	}
}
