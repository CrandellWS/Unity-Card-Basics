using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartOptions : MonoBehaviour {


    public MenuSettings menuSettingsData;
	public int sceneToStart = 1;										//Index number in build settings of scene to load if changeScenes is true
	public bool changeScenes;											//If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	public bool changeMusicOnStart;										//Choose whether to continue playing menu music or start a new music clip
	public CanvasGroup fadeOutImageCanvasGroup;                         //Canvas group used to fade alpha of image which fades in before changing scenes
	public Image fadeImage;                                             //Reference to image used to fade out before changing scenes
	public CanvasGroup BackgroundImageCanvasGroup;                         //Canvas group used to fade alpha of image which fades in before changing scenes
	public Image BackgroundImage;                                             //Reference to image used to fade out before changing scenes

	[HideInInspector] public bool inMainMenu = true;					//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
	[HideInInspector] public AnimationClip fadeAlphaAnimationClip;		//Animation clip fading out UI elements alpha


	private PlayMusic playMusic;										//Reference to PlayMusic script
	private float fastFadeIn = .01f;									//Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
	private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels
    private CanvasGroup menuCanvasGroup;



	public static StartOptions instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);


		//Get a reference to ShowPanels attached to UI object
		showPanels = GetComponent<ShowPanels> ();

		//Get a reference to PlayMusic attached to UI object
		playMusic = GetComponent<PlayMusic> ();

        //Get a reference to the CanvasGroup attached to the main menu so that we can fade it's alpha
        menuCanvasGroup = GetComponent<CanvasGroup>();

        fadeImage.color = menuSettingsData.sceneChangeFadeColor;

	}


	public void StartButtonClicked()
	{
		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic
		//To change fade time, change length of animation "FadeToColor"
		if (menuSettingsData.musicLoopToChangeTo != null) 
		{
			playMusic.FadeDown(menuSettingsData.menuFadeTime);
		}

		//If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
		if (menuSettingsData.nextSceneIndex != 0) 
		{
			//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
			Invoke ("LoadDelayed", menuSettingsData.menuFadeTime);

			StartCoroutine(FadeCanvasGroupAlphaHideMenu(0f, 1f, fadeOutImageCanvasGroup));


        } 

		//If changeScenes is false, call StartGameInScene
		else 
		{
			//Call the StartGameInScene function to start game without loading a new scene.
			StartGameInScene();
		}

	}


	public void MainMenuButtonClicked()
	{
		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic
		//To change fade time, change length of animation "FadeToColor"
		if (menuSettingsData.musicLoopToChangeTo != null) 
		{
			playMusic.FadeDown(menuSettingsData.menuFadeTime);
		}

		//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
		Invoke ("LoadDelayedMainMenu", menuSettingsData.menuFadeTime);

		StartCoroutine(FadeCanvasGroupAlpha(0f, 1f, fadeOutImageCanvasGroup));

	}


	public void OptionsButtonClicked()
	{
		//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
		Invoke ("LoadDelayedOptionsMenu", menuSettingsData.menuFadeTime);

		StartCoroutine(FadeCanvasGroupAlpha(0f, 1f, fadeOutImageCanvasGroup));

	}


	public void OptionsBackButtonClicked()
	{
		//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
		Invoke ("LoadDelayedOptionsMenuHide", menuSettingsData.menuFadeTime);

		StartCoroutine(FadeCanvasGroupAlpha(0f, 1f, fadeOutImageCanvasGroup));

	}


    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneWasLoaded;

    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneWasLoaded;
    }

	bool sceneInit = false;

    //Once the level has loaded, check if we want to call PlayLevelMusic
    void SceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
		//if changeMusicOnStart is true, call the PlayLevelMusic function of playMusic
		if (menuSettingsData.musicLoopToChangeTo != null)
		{
			playMusic.PlayLevelMusic ();
		}
		Debug.Log("OnSceneLoaded: " + scene.name);
		Debug.Log(mode);

		if (scene.name != "MainMenu") {
			Debug.Log("OnSceneLoaded: not MainMenu is " + scene.name);
			StartCoroutine (FadeCanvasGroupAlphaHideMenu (1f, 0f, fadeOutImageCanvasGroup));
			StartCoroutine (FadeCanvasGroupAlphaHideMenu (1f, 0f, BackgroundImageCanvasGroup));
		} else {
			Debug.Log("OnSceneLoaded: so MainMenu is " + scene.name);
			showPanels.ShowMenu ();
			StartCoroutine (FadeCanvasGroupAlpha (0f, 1f, BackgroundImageCanvasGroup));
		}
	}

	public void LoadDelayed()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//Hide the main menu UI element
		showPanels.HideMenu ();

		//Load the selected scene, by scene index number in build settings
		SceneManager.LoadScene (sceneToStart);
	}

	public void LoadDelayedMainMenu()
	{
		//Pause button now does not work if escape is pressed since we are going into the Main menu.
		inMainMenu = true;

		//show the main menu UI element

		StartCoroutine (FadeCanvasGroupAlpha (1f, 0f, fadeOutImageCanvasGroup));

		SceneManager.LoadScene ("Assets/CardFramework/AssetBundles/Scenes/MainMenu.unity", LoadSceneMode.Single);

		Debug.Log("LoadDelayedMainMenu");
	}


	public void LoadDelayedOptionsMenu()
	{

		//show the main menu UI element

		StartCoroutine (FadeCanvasGroupAlpha (1f, 0f, fadeOutImageCanvasGroup));

		showPanels.ShowOptionsPanel();

		Debug.Log("LoadDelayedOptionsMenu");
	}


	public void LoadDelayedOptionsMenuHide()
	{

		//show the main menu UI element

		StartCoroutine (FadeCanvasGroupAlpha (1f, 0f, fadeOutImageCanvasGroup));

		showPanels.HideOptionsPanel();

		Debug.Log("LoadDelayedOptionsMenuHide");
	}

	public void HideDelayed()
	{
		//Hide the main menu UI element after fading out menu for start game in scene
		showPanels.HideMenu();
	}

	public void StartGameInScene()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//If there is a second music clip in MenuSettings, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic 
		if (menuSettingsData.musicLoopToChangeTo != null) 
		{
			//Wait until game has started, then play new music
			Invoke ("PlayNewMusic", menuSettingsData.menuFadeTime);
		}
        
		StartCoroutine(FadeCanvasGroupAlphaHideMenu(1f,0f, menuCanvasGroup));
	}

	public IEnumerator FadeCanvasGroupAlpha(float startAlpha, float endAlpha, CanvasGroup canvasGroupToFadeAlpha)
	{

		float elapsedTime = 0f;
		float totalDuration = menuSettingsData.menuFadeTime;

		while (elapsedTime < totalDuration)
		{
			elapsedTime += Time.deltaTime;
			float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);
			canvasGroupToFadeAlpha.alpha = currentAlpha;
			yield return null;
		}

		Debug.Log("Fade Image Coroutine done.");
	}

	public IEnumerator FadeCanvasGroupAlphaHideMenu(float startAlpha, float endAlpha, CanvasGroup canvasGroupToFadeAlpha)
	{

		float elapsedTime = 0f;
		float totalDuration = menuSettingsData.menuFadeTime;

		while (elapsedTime < totalDuration)
		{
			elapsedTime += Time.deltaTime;
			float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);
			canvasGroupToFadeAlpha.alpha = currentAlpha;
			yield return null;
		}

		HideDelayed();
		Debug.Log("Fade Image and hide menu Coroutine done.");
	}


    public void PlayNewMusic()
	{
		//Fade up music nearly instantly without a click 
		playMusic.FadeUp (fastFadeIn);
		//Play second music clip from MenuSettings
		playMusic.PlaySelectedMusic (menuSettingsData.musicLoopToChangeTo);
	}

}
