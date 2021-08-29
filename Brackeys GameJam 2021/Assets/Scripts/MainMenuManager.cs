using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD;
using FMODUnity;

public class MainMenuManager : MonoBehaviour
{

    public string bgmEvent = "event:/";
    FMOD.Studio.EventInstance bgm;


    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        
    }
}
