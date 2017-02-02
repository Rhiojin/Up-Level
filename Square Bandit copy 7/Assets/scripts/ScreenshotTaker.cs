using UnityEngine;
using System.Collections;
using UnityEditor;
[InitializeOnLoad]
public class ScreenshotTaker
{

	static private bool _TakingScreenshots;
	static private int _CurrentResolutionIndex = 0;
	static private string _FolderPath;
	static private EditorWindow _GameView;

	static private Vector2[] ScreenResolutions = {
//		new Vector2(2048, 2732),
		new Vector2(1080, 1920),
		new Vector2(640, 960),	
	};


	private static void CustomUpdate(){
		if(_TakingScreenshots){
			var resolution = ScreenResolutions [_CurrentResolutionIndex];
			Screenshot (resolution, _FolderPath);
			_CurrentResolutionIndex++;
			if (_CurrentResolutionIndex >= ScreenResolutions.Length) {
				_TakingScreenshots = false;
				onScreenshotsTakingDone ();
			}
		}
	}

	private static void onScreenshotsTakingDone(){
//		Application.OpenURL (_FolderPath);
		EditorApplication.update -= CustomUpdate;
	}

	private static void Screenshot (Vector2 resolution, string folderPath)
	{
		//resize game view
		int width = (int)resolution.x;
		int height = (int)resolution.y;
		int gameViewBarHeight = 17;
		var rect = _GameView.position;
		rect.x = 0;
		rect.y = 0;
		rect.width = width;
		rect.height = height + gameViewBarHeight;
		_GameView.position = rect;

		_GameView.minSize = new Vector2 (rect.width, rect.height);
		_GameView.maxSize = new Vector2 (rect.width, rect.height);

		//take screenshot
		int s = PlayerPrefs.GetInt("quickShot",0);
		string fileName = "screenshot_" + s +  ".png";
		s++;
		PlayerPrefs.SetInt("quickShot",s);
		fileName = System.IO.Path.Combine (folderPath, fileName);
		Application.CaptureScreenshot (fileName);
	}

	[MenuItem("Tools/TakeScreenshots")]
	public static void MenuItem_TakeScreenshots()
	{
		//get game view
		System.Type gameViewType = System.Type.GetType("UnityEditor.GameView, UnityEditor");
		_GameView = EditorWindow.GetWindow(gameViewType);
		if (_GameView == null)
			EditorApplication.ExecuteMenuItem("Waindow/Game");

		_GameView = EditorWindow.GetWindow(gameViewType);
		if (_GameView == null)
		{
			Debug.LogError("Game View not find!");
			return;
		}

		_FolderPath = Application.persistentDataPath;
		_FolderPath = Application.dataPath+"/screenshots/";
		_TakingScreenshots = true;
		_CurrentResolutionIndex = 0;
		EditorApplication.update += CustomUpdate;

	}
}