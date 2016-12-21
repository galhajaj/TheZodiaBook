using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class EscapeToMainScene : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*if (GameLoopScript.Score > PlayerPrefs.GetInt("BestScore"))
                PlayerPrefs.SetInt("BestScore", GameLoopScript.Score);

            SceneManager.LoadScene("mainScene");*/
        }
	
	}
}
