using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour 
{
    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetLevelType(string levelType)
    {
        PlayerPrefs.SetString("LevelType", levelType);
    }
}
