﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerDownHandler
{
    private Manager _managerScript = null;
	
    void Awake()
    {
        _managerScript = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public void OnPointerDown(PointerEventData eventData)
    {
        string moveTo = gameObject.GetComponent<OptionButton>().MoveTo;
        string moveToType = moveTo.Split(':')[0];
        string moveToName = moveTo.Split(':')[1];

        if (moveToType.ToLower() == "state")
        {
            _managerScript.LoadState(moveToName);
        }
        else if (moveToType.ToLower() == "event" && moveToName.ToLower() == "next")
        {
            _managerScript.LoadNextEvent();
        }
        else if (moveToType.ToLower() == "area")
        {
            _managerScript.LoadArea(moveToName);
        }
        else if (moveToType.ToLower() == "subarea")
        {
            _managerScript.LoadArea(moveToName, true);
        }
        else
        {
            Debug.Log("ERROR: move to type " + moveTo + " is not identified!");
        }
    }

}
