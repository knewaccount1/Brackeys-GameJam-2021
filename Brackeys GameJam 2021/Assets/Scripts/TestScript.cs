using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class TestScript : MonoBehaviour
{
    [Header("Persisting Sounds ")]
    public List<BGMSounds> BGMList;
    public string BGM1Event = "";
    public string BGM2Event = "";
    public string BGM3Event = "";
    public string hubMusicEvent = "";


    private FMOD.Studio.EventInstance bgm1;
    private FMOD.Studio.EventInstance bgm2;
    private FMOD.Studio.EventInstance bgm3;
    private FMOD.Studio.EventInstance hubMusic;

    [Header("Finite Sounds (SFX)")]
    public string SFX = "";


    [Header("PlayOneShot But tracked in code")]
    public string whisperEvent = "";
    FMOD.Studio.EventInstance whisper;

    private void Start()
    {
        bgm1 = FMODUnity.RuntimeManager.CreateInstance(BGM1Event);
        bgm2 = FMODUnity.RuntimeManager.CreateInstance(BGM2Event);
        bgm1.start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FMODUnity.RuntimeManager.PlayOneShot(SFX, transform.position);

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bgm1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            bgm2.start();
        }

    }

    [System.Serializable]
    public class BGMSounds
    {
        public string BGMEvent = "";
        [HideInInspector]public FMOD.Studio.EventInstance bgm;

        public BGMSounds()
        {
            bgm = FMODUnity.RuntimeManager.CreateInstance(BGMEvent);
        }
    }
}
