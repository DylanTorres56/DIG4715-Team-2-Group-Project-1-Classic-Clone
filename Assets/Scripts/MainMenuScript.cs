using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript: MonoBehaviour
{
    [SerializeField] private AudioClip menuMusicClip;
    
    public void Awake()
    {
        Time.timeScale = 1;
        

    }

    private void Start()
    {
        SoundFXManager.Instance.PlayMusic(menuMusicClip, transform, 0.5f);
    }
    // Start is called before the first frame update
    public void LevelOne()
    {
        Debug.Log("Loading a new scene.");
        SceneManager.LoadSceneAsync(2);
    }

    public void LevelTwo()
    {
        Debug.Log("Loading a new scene.");
        SceneManager.LoadSceneAsync(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
