using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class FootStepPlayer : MonoBehaviour
{
    public string[] footStepEvents;


    public void PlayFootSteps()
    {
        int i = Random.Range(0, footStepEvents.Length);
        FMODUnity.RuntimeManager.PlayOneShot(footStepEvents[i], transform.position);
    }
}
