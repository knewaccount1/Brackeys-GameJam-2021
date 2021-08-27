using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD;
using FMODUnity;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{

    public List<Interactable> allInteractables;

    public int interactablesToWin;
    public int score;
    private float timer;

    public int maxTime;

    [HideInInspector]public Player playerRef;

    [HideInInspector] public int destroyedInteractables;


    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("FMOD Parameters")]
    public string bgmEvent = "event:/";
    FMOD.Studio.EventInstance bgm;
    FMOD.Studio.PARAMETER_ID bgmTimerID;

    [Header("Debug Parameters")]
    public int seconds;

    private void Start()
    {
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

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(1);
        }

        timer -= Time.deltaTime;
        seconds = (int)timer;

        timerText.text = seconds + " sec";
        //Updating FMOD Timer
        bgm.setParameterByID(bgmTimerID, (float)seconds/maxTime);
    }

    public void CheckWinCondition()
    {
        if (destroyedInteractables >= interactablesToWin)
        {
            UnityEngine.Debug.Log("Player has won, load next scene");
            SceneManager.LoadScene(1);
        }

    }


}
