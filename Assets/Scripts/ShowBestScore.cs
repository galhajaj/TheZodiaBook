using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowBestScore : MonoBehaviour 
{
    public Text ScoreText;
	// Use this for initialization
	void Start () 
    {
        ScoreText.text = "Best Score: " + PlayerPrefs.GetInt("BestScore").ToString();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
