using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlexibleUIImage : FlexibleUI

{
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        base.Initialize();
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();
        image.color = flexibleUIData.imageColor;
    }



}
