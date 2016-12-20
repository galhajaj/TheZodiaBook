using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerDownHandler
{
    public GameLoop GameLoopScript;
    public SnakeDirection Direction;

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
        Time.timeScale = 1.0F;
        GameLoopScript.Direction = Direction;
    }

}
