// Load an assetbundle which contains Scenes.
// When the user clicks a button the first Scene in the assetbundle is
// loaded and replaces the current Scene.

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	private AssetBundle myLoadedAssetBundle;
	private string[] scenePaths;

	// Use this for initialization
	void Start()
	{
//		myLoadedAssetBundle = BundleSingleton.Instance.LoadBundle("scenes");
		//myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/Scenes");
//		scenePaths = myLoadedAssetBundle.GetAllScenePaths();
	}

	public void LoadMainMenu()
	{
		//		Debug.Log("Scene0 loading: " + scenePaths[0]);
		//		Debug.Log("Scene1 loading: " + scenePaths[1]);
		//		Debug.Log("Scene2 loading: " + scenePaths[2]);
		//		Debug.Log("Scene3 loading: " + scenePaths[3]);
//		SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
		SceneManager.LoadScene ("Assets/CardFramework/AssetBundles/Scenes/MainMenu.unity", LoadSceneMode.Single);
		//SceneManager.LoadScene (scenePaths[1], LoadSceneMode.Single);
	}

	public void LoadTarot()
	{
		//		Debug.Log("Scene0 loading: " + scenePaths[0]);
		//		Debug.Log("Scene1 loading: " + scenePaths[1]);
		//		Debug.Log("Scene2 loading: " + scenePaths[2]);
		//		Debug.Log("Scene3 loading: " + scenePaths[3]);
//		SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
		SceneManager.LoadScene ("Assets/CardFramework/AssetBundles/Scenes/DealTarot.unity", LoadSceneMode.Single);
		//SceneManager.LoadScene (scenePaths[1], LoadSceneMode.Single);
	}
}
