using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FlexibleUIText : FlexibleUI

{
    private Text text;
    public bool useMainFont = true;

    void Awake()
    {
        text = GetComponent<Text>();
        base.Initialize();
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();
        text.color = flexibleUIData.textColor;
        if (useMainFont)
        {
            text.font = flexibleUIData.font;
        }
        
    }



}
