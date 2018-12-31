using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]

public class FlexibleUI : MonoBehaviour {

    public FlexibleUIData flexibleUIData;

    protected virtual void OnSkinUI(){}

    public void Initialize()
    {
        OnSkinUI();
    }

    public virtual void Update()
    {
        if (Application.isEditor)
        {
            OnSkinUI();
        }
    }
}
