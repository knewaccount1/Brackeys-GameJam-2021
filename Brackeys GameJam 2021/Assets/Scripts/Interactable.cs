using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour
{
    private GameManager GM;
    private Rigidbody2D rb2D;
    public bool isDamaged;

    public AudioClip destroyAudio;
    

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

    public void RepairObject()
    {
        Debug.Log("Employee is trying to repair object");
        GM.destroyedInteractables--;
    }

    public void DestroyInteractable()
    {

        GM.destroyedInteractables++;
       
    }
}
