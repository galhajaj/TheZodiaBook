using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

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

        // get list of all random events of that area
        var allRandomEvents = Resources.LoadAll("Xmls/" + areaName + "RandomEvents");
        List<string> allRandomEventsList = new List<string>();
        for (int i = 0; i < allRandomEvents.Length; ++i)
            allRandomEventsList.Add(allRandomEvents[i].name);

        //get list of all resources in current area path
        var objects = Resources.LoadAll("Xmls/" + areaName); // var its Object[]
        for (int i = objects.Length - 1; i >= 0; --i)
        {
            string eventName = objects[i].name;

            if (eventName.Split('_')[1] == "RANDOM")
            {
                int numberOfRandomEvents = Convert.ToInt32(eventName.Split('_')[2]);
                for(int ri = 0; ri < numberOfRandomEvents; ++ri)
                {
                    int randomEventIndex = UnityEngine.Random.Range(0, allRandomEventsList.Count);
                    string randomEvent = allRandomEventsList[randomEventIndex];
                    allRandomEventsList.RemoveAt(randomEventIndex);
                    _events.Insert(0, areaName + "RandomEvents/" + randomEvent);
                }
            }
            else
            {
                _events.Insert(0, areaName + "/" + eventName);
            }
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
            button.transform.GetChild(0).GetComponent<Text>().text = node.InnerText;
            button.GetComponent<OptionButton>().MoveTo = node.Attributes["to"].Value;
            button.transform.SetParent(OptionButtonsPanel.transform, false);
        }
    }
}
