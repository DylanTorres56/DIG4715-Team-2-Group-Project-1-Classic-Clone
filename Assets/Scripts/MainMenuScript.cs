using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript: MonoBehaviour
{
    public void Awake()
    {
        Time.timeScale = 1;
    }
    
    // Start is called before the first frame update
    public void NewGame()
    {
        Debug.Log("Loading a new scene.");
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
