using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Manager : MonoBehaviour 
{
    public Text StateDescriptionText;
    public Image StateImage;
    public string FirstArea;
    public GameObject OptionButtonsPanel;
    public GameObject OptionButton;

    private List<string> _events = new List<string>();
    public string _currentEvent = string.Empty;

	void Start () 
    {
        LoadArea(FirstArea);
	}
	
	void Update () 
    {
	
	}

    public void LoadArea(string areaName, bool isSubArea = false)
    {
        // clear events
        if (!isSubArea)
            _events.Clear();

        //get list of all resources in current area path
        Object[] objects = Resources.LoadAll("Xmls/" + areaName);
        for (int i = objects.Length - 1; i >= 0; --i)
        {
            _events.Insert(0, areaName + "/" + objects[i].name);
        }

        // load first event
        LoadNextEvent();
    }

    public void LoadNextEvent()
    {
        _currentEvent = _events[0];

        _events.RemoveAt(0);

        LoadState("START");
    }

    public void LoadState(string stateId)
    {
        TextAsset textAsset = (TextAsset) Resources.Load("Xmls/" + _currentEvent);  
        if (textAsset == null)
            Debug.Log("ERROR: " + _currentEvent + ".xml is missing...");
        XmlDocument xmldoc = new XmlDocument ();
        xmldoc.LoadXml ( textAsset.text );

        XmlNodeList elemList = xmldoc.GetElementsByTagName("State");

        // find the state by name
        XmlNode stateNode = null;
        if (stateId == "START")
        {
            stateNode = elemList[0];
        }
        else
        {
            foreach (XmlNode n in elemList)
            {
                if (n.Attributes["id"].Value == stateId)
                    stateNode = n;
            }
        }

        if (stateNode == null)
        {
            Debug.Log("ERROR: " + stateId + " state is missing...");
            return;
        }

        // set state description
        StateDescriptionText.text = stateNode["Text"].InnerText;

        // set state image
        string imageName = stateNode["Image"].InnerText;
        Sprite imageSprite = Resources.Load<Sprite>("Images/"+imageName);
        if (imageSprite == null)
            Debug.Log("ERROR: " + imageName + ".png is missing...");
        StateImage.sprite = imageSprite;

        // delete all current options buttons
        for (int i = OptionButtonsPanel.transform.childCount - 1; i >= 0; --i)
        {
            Destroy(OptionButtonsPanel.transform.GetChild(i).gameObject);
        }

        // add option buttons
        XmlNode optionsNode = stateNode["Options"];
        XmlNodeList optionsNodes = optionsNode.SelectNodes("Option");
        foreach (XmlNode node in optionsNodes)
        {
            GameObject button = Instantiate(OptionButton);
            button.transform.GetChild(0).GetComponent<Text>().text = node["Text"].InnerText;
            button.GetComponent<OptionButton>().MoveTo = node["MoveTo"].InnerText;
            button.transform.SetParent(OptionButtonsPanel.transform, false);
        }
    }
}
