using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;

public class Manager : MonoBehaviour 
{
    public Text StateDescriptionText;
    public Image StateImage;
    public string CurrentEvent;
    public GameObject OptionButtonsPanel;
    public GameObject OptionButton;

	void Start () 
    {
        LoadEvent(CurrentEvent);
	}
	
	void Update () 
    {
	
	}

    public void LoadEvent(string eventName)
    {
        LoadState("START");
        /*TextAsset textAsset = (TextAsset) Resources.Load("Xmls/"+eventName);  
        if (textAsset == null)
            Debug.Log("ERROR: " + eventName + ".xml is missing...");
        XmlDocument xmldoc = new XmlDocument ();
        xmldoc.LoadXml ( textAsset.text );

        XmlNodeList elemList = xmldoc.GetElementsByTagName("State");
        XmlNode introElement = elemList[0];

        // set state description
        StateDescriptionText.text = introElement["Text"].InnerText;

        // set state image
        string imageName = introElement["Image"].InnerText;
        Sprite imageSprite = Resources.Load<Sprite>("Images/"+imageName);
        if (imageSprite == null)
            Debug.Log("ERROR: " + imageName + ".png is missing...");
        StateImage.sprite = imageSprite;

        // add option buttons
        XmlNode optionsNode = introElement["Options"];
        XmlNodeList optionsNodes = optionsNode.SelectNodes("Option");
        foreach (XmlNode node in optionsNodes)
        {
            GameObject button = Instantiate(OptionButton);
            button.transform.GetChild(0).GetComponent<Text>().text = node["Text"].InnerText;
            button.GetComponent<OptionButton>().MoveTo = node["MoveTo"].InnerText;
            button.transform.SetParent(OptionButtonsPanel.transform, false);
        }*/
    }

    public void LoadState(string stateId)
    {
        TextAsset textAsset = (TextAsset) Resources.Load("Xmls/"+CurrentEvent);  
        if (textAsset == null)
            Debug.Log("ERROR: " + CurrentEvent + ".xml is missing...");
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
