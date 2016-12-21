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
        TextAsset textAsset = (TextAsset) Resources.Load("Xmls/"+eventName);  
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
            Debug.Log("iteration inside foreach");
            GameObject button = Instantiate(OptionButton);
            if (button == null)
                Debug.Log("button is null");
            button.transform.GetChild(0).GetComponent<Text>().text = node["Text"].InnerText;
            button.GetComponent<OptionButton>().MoveTo = node["MoveTo"].InnerText;

            button.transform.SetParent(OptionButtonsPanel.transform, false);
        }
    }
}
