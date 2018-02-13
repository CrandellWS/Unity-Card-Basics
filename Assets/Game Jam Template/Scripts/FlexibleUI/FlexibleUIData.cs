using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIData : ScriptableObject
{

    [Header("Button Colors")]
    public ColorBlock buttonColorBlock;

    [Header("Panel Color")]
    public Color imageColor;

    [Header("Slider Colors")]
    public ColorBlock sliderColorBlock;

    [Header("Text Attributes")]
    public Font font;
    public Color textColor;
}
