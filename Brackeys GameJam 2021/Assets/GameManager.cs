using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD;
using FMODUnity;
using UnityEngine.UI;
using TMPro;

using Cinemachine;


public class GameManager : MonoBehaviour
{

    public List<Interactable> allInteractables;

    public int interactablesToWin;
    public int score;
    private float timer;

    public int maxTime;
    public int timePerBoost;

    [HideInInspector]public Player playerRef;

    [HideInInspector] public int destroyedInteractables;

    [Header("Camera Shake Parameters")]
    public CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin noise;
    public float shakeAmplitude;
    public float shakeFrequency;
    public float shakeTime;
    private float shakeTimer;
    private float startingAmplitude;


    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("FMOD Parameters")]
    public string bgmEvent = "event:/";
    FMOD.Studio.EventInstance bgm;
    FMOD.Studio.PARAMETER_ID bgmTimerID;

    public string[] announcementEvent;
    public float announcementTimer;

    [Header("Debug Parameters")]
    public int seconds;

    private void Start()
    {
        announcementTimer = 10;

        timer = maxTime;

        playerRef = FindObjectOfType<Player>();

        allInteractables = new List<Interactable>();
        allInteractables.AddRange(FindObjectsOfType<Interactable>());


        //FMOD Initializing
        bgm = FMODUnity.RuntimeManager.CreateInstance(bgmEvent);

        bgm.start();


        FMOD.Studio.EventDescription bgmTimerDescription;
        bgm.getDescription(out bgmTimerDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION timerParameterDescrition;
        bgmTimerDescription.getParameterDescriptionByName("Timer", out timerParameterDescrition);
        bgmTimerID = timerParameterDescrition.id;


        //Cinemachine rigging
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    public void ShakeCam()
    {
        startingAmplitude = shakeAmplitude;
        noise.m_AmplitudeGain = shakeAmplitude;
        noise.m_FrequencyGain = shakeFrequency;
        shakeTimer = shakeTime;

    }


    private void Update()
    {

        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            noise.m_AmplitudeGain = Mathf.Lerp(startingAmplitude, 0f, 1 - (shakeTimer / shakeTime));
            
        }


        if (Input.GetKey(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(1);
        }

        timer -= Time.deltaTime;
        seconds = (int)timer;

        timerText.text = "Time: " +seconds;
        scoreText.text = "Score: " + score;
        
        //Updating FMOD Timer
        bgm.setParameterByID(bgmTimerID, (float)seconds/maxTime);


        if(seconds <= 0)
        {
            EndGame();
        }

        announcementTimer -= Time.deltaTime;

        if(announcementTimer <= 0)
        {
            int i = Random.Range(0, announcementEvent.Length);
            FMODUnity.RuntimeManager.PlayOneShot(announcementEvent[i], transform.position);
            announcementTimer = 30f;
        }
    }

    public void AddScore(int i)
    {
        score += i;

    }
    public void AddTime()
    {
        timer += timePerBoost;
    }

    public void CheckWinCondition()
    {
        if (destroyedInteractables >= interactablesToWin)
        {
            UnityEngine.Debug.Log("Player has won, load next scene");
            SceneManager.LoadScene(1);
        }

    }

    public void EndGame()
    {
        bgm.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(2);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        bgm.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene(0);
    }


}
