using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

public class Tester : MonoBehaviour 
{
    public bool IsActive = true;

    private List<string> _areas = new List<string>();
    private List<string> _states = new List<string>();

	// Use this for initialization
	void Start () 
    {
        if (!IsActive)
            return;

        addTests();
        testAll();
	}
	
    private void addTests()
    {
        _areas.Add("LittleTown");
        _areas.Add("TheDarkForest");
        _areas.Add("KingKolecCastle");
        _areas.Add("TheMountains");
    }

	private void testAll() 
    {
        foreach (string area in _areas)
        {
            test(area);
        }
	}

    private void test(string areaName)
    {
        int counter = 0;

        Object[] objects = Resources.LoadAll("Xmls/" + areaName);
        for (int i = objects.Length - 1; i >= 0; --i)
        {
            testEvent(areaName, objects[i].name);
            counter++;
        }

        Debug.Log(areaName + ": " + counter.ToString() + " events");
    }

    private void testEvent(string areaName, string eventName)
    {
        string fullEventName = areaName + "/" + eventName;
        TextAsset textAsset = (TextAsset)Resources.Load("Xmls/" + fullEventName); 
        if (textAsset == null)
            Debug.LogError(fullEventName + ".xml is missing...");
        XmlDocument xmldoc = new XmlDocument ();
        xmldoc.LoadXml ( textAsset.text );

        XmlNodeList elemList = xmldoc.GetElementsByTagName("State");

        _states.Clear();
        foreach (XmlNode n in elemList)
        {
            _states.Add(n.Attributes["id"].Value);
        }
        var duplicateStates = _states.GroupBy(a => a).SelectMany(ab => ab.Skip(1).Take(1)).ToList();
        foreach (string stt in duplicateStates)
        {
            Debug.LogError("Duplicate state " + stt + " in " + fullEventName);
        }

        // check if the first state is START
        if (_states[0] != "START")
            Debug.LogError("First state is not START in " + fullEventName);

        foreach (XmlNode n in elemList)
        {
            string currentState = n.Attributes["id"].Value;

            XmlNode optionsNode = n["Options"];
            XmlNodeList optionsNodes = optionsNode.SelectNodes("Option");

            List<string> optionsList = new List<string>();
            foreach (XmlNode node in optionsNodes)
            {
                optionsList.Add(node.InnerText);    
            }
            var duplicateOptions = optionsList.GroupBy(a => a).SelectMany(ab => ab.Skip(1).Take(1)).ToList();
            foreach (string opt in duplicateOptions)
            {
                Debug.LogError("Duplicate option " + opt + " in " + fullEventName + "/" + currentState);
            }

            foreach (XmlNode node in optionsNodes)
            {
                XmlAttribute toAtrr = node.Attributes["to"];
                if (toAtrr == null)
                {
                    Debug.LogError("The [to] attribute is missing in " + fullEventName + "/" + currentState);
                    return;
                }

                string to = toAtrr.Value;
                string first = to.Split(':')[0];
                string second = to.Split(':')[1];

                if (first == "event")
                {
                    if (second != "next")
                        Debug.LogError("The [to] attribute " + to + " in " + fullEventName + "/" + currentState + " is not allowed");
                }
                else if (first == "state")
                {
                    if (!_states.Contains(second))
                        Debug.LogError("The state " + second + " is not found in " + fullEventName);

                    if (currentState == second)
                        Debug.LogError("The state " + second + " in " + fullEventName + "/" + currentState + " has option that direct itself");
                }
                else if (first == "area" || first == "subarea")
                {
                    if (!_areas.Contains(second))
                        Debug.LogError("The area " + second + " is not found for " + fullEventName + "/" + currentState);
                }
                else
                {
                    Debug.LogError("The [to] attribute " + to + " in " + fullEventName + "/" + currentState + " is not allowed");
                }
            }
        }
    }
}

