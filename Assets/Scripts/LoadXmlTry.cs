using UnityEngine;
using System.Collections;
using System.Xml;

public class LoadXmlTry : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
        TextAsset textAsset = (TextAsset) Resources.Load("Xmls/lala");  
        if (textAsset == null)
            Debug.Log("xml file is null...");
        XmlDocument xmldoc = new XmlDocument ();
        xmldoc.LoadXml ( textAsset.text );

        XmlNodeList elemList = xmldoc.GetElementsByTagName("Event");
        for (int i=0; i < elemList.Count; i++)
        {   
            Debug.Log(elemList[i].InnerXml);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
