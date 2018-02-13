using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuObject : MonoBehaviour {

    //Drag the object which you want to be automatically selected by the keyboard or gamepad when this panel becomes active
    public GameObject firstSelectedObject;

    public void SetFirstSelected()
    {
        //Tell the EventSystem to select this object
        EventSystemChecker.menuEventSystem.SetSelectedGameObject(firstSelectedObject);
    }

    public void OnEnable()
    {
        //Check if we have an event system present
        if (EventSystemChecker.menuEventSystem != null)
        {
            //If we do, select the specified object
            SetFirstSelected();
        }
        
    }

}
