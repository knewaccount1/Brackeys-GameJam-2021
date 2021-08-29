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

    public string selectEvent;
    public string backEvent;

    public void PlaySelectSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(selectEvent, transform.position);
    }
    public void PlayBackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(backEvent, transform.position);
    }

    public void PlayButton()
    {
        PlaySelectSound();
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
