using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EventSystemChecker : MonoBehaviour
{
    public static EventSystem menuEventSystem;

    // Any static function tagged with RuntimeInitializeOnLoadMethod will be called only a single time when the game load
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static public void InitSceneCallback()
    {
        SceneManager.sceneLoaded += SceneWasLoaded;
    }

    // This used to be OnLevelWasLoaded (now deprecated). It is registered as a callback on Scene Load in the
    // SceneManager in the function above.
    static public void SceneWasLoaded(Scene scene, LoadSceneMode mode)
	{

       //Find the EventSystem by type
        menuEventSystem = FindObjectOfType<EventSystem>() as EventSystem;

        //If there is no EventSystem (needed for UI interactivity) present
        if (menuEventSystem == null)
        {
            //The following code instantiates a new object called EventSystem
            GameObject obj = new GameObject("EventSystem");

            //And adds the required components, while storing a reference in the menuSystem variable, which is static and accessible from other scripts
            menuEventSystem = obj.AddComponent<EventSystem>();
            obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;

        }



    }

}
