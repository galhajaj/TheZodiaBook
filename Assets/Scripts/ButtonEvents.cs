using UnityEngine;
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
        _managerScript.LoadState(moveTo);
    }

}
