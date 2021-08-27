using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;
using FMOD;

public class Interactable : MonoBehaviour
{
    private GameManager GM;
    private Rigidbody2D rb2D;
    public bool isDamaged;
    [HideInInspector]public bool beingRepaired;



    public PowerUp powerUpPrefab;

    [Header("FMOD SFX")]
    public string SFX = "event:/";

    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        rb2D = GetComponent<Rigidbody2D>();
    }


    public void Knockback(float kbForce, Transform playerPos )
    {
        // rb2D.AddForce(new Vector2(1, .8f) * kbForce, ForceMode2D.Impulse);
        Vector2 posDelta = transform.position - playerPos.position;
        posDelta.Normalize();

        transform.DOLocalMove(posDelta * kbForce, 1f, false);
        
    }

    //Called by employee units
    public void RepairObject()
    {
        UnityEngine.Debug.Log("Employee repaired object");
        GM.destroyedInteractables--;
        isDamaged = false;
        //Swap sprites here. Play audio here.
    }

    //Called on collision with something player tackle
    public void DestroyInteractable()
    {
        if (!isDamaged)
        {
            GM.destroyedInteractables++;
            PowerUp powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);

            Vector2 kbDirection = transform.position - GM.playerRef.transform.position;
            kbDirection.Normalize();

            powerUp.GetComponent<Rigidbody2D>().AddForce(kbDirection * 10f, ForceMode2D.Impulse);
            isDamaged = true;

            //Add audio sprite change logic here;
            FMODUnity.RuntimeManager.PlayOneShot(SFX, transform.position);
        }

    }
}
