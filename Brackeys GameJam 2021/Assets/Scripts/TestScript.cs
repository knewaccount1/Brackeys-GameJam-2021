using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class TestScript : MonoBehaviour
{

    private FMOD.Studio.EventInstance instance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            instance = FMODUnity.RuntimeManager.CreateInstance("event:/Kick");
            instance.start();
            instance.release();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            instance = FMODUnity.RuntimeManager.CreateInstance("event:/Sample BG Music");
            instance.start();
        }

    }
}
