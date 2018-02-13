using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MenuSettings")]
public class MenuSettings : ScriptableObject
{

    public float menuFadeTime = .5f;
    public Color sceneChangeFadeColor = Color.black;
    [Header("Leave this at zero to start game in same scene as menu, otherwise set to scene index")]
    public int nextSceneIndex = 0;

    [Header("Add your menu music here")]
    public AudioClip mainMenuMusicLoop;
    [Header("If you want to play new music after Start is pressed, add it here")]
    public AudioClip musicLoopToChangeTo;

}
