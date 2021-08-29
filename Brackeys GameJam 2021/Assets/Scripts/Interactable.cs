using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;
using FMOD;
public class Interactable : MonoBehaviour
{
    private GameManager GM;
    private SpriteRenderer sr;
    private Rigidbody2D rb2D;
    public bool isDamaged;
    
    [HideInInspector]public bool beingRepaired;

    public ParticleSystem[] destroyParticle;
    public Sprite damagedSprite;
    private Sprite originalSprite;
    public PowerUp powerUpPrefab;
    public SpeedBoost speedBoostPrefab;

    
    public GameObject boostSpawner;

    [Header("FMOD SFX")]
    public string[] SFX;

    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalSprite = sr.sprite;
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
        //GM.destroyedInteractables--;
        isDamaged = false;
        sr.sprite = originalSprite;
        GetComponent<Collider2D>().isTrigger = false;
        gameObject.layer = 10;
        //Swap sprites here. Play audio here.
    }

    //Called on collision with something player tackle
    public void DestroyInteractable()
    {
        if (!isDamaged)
        {
            //Stop tackle animation and/or start tackle hit animation
            BoxBounds boxBounds = boostSpawner.GetComponentInChildren<BoxBounds>();
            GM.destroyedInteractables++;
            GM.AddScore(10);


            GetComponent<Collider2D>().isTrigger = true;

            // Time Boost Drop

            float i = Random.Range(0f, 1f);




            if (GM.destroyedInteractables % 10 == 0)
            {
                Vector2 spawnPoint = boxBounds.RandomPointInBounds();
                Transform powerUp = Instantiate((GameObject)Resources.Load("Prefabs/Time", typeof(GameObject)), spawnPoint, Quaternion.identity).transform;
                //powerUp.GetComponent<PowerUp>().SetPowerType("TimeBoost");

            }

            // Check if is a fruit type shelf
            if (speedBoostPrefab != null)
            {
                Vector2 spawnPoint = boxBounds.RandomPointInBounds();
                // Drop a speed boost same as time boost
                Transform speedBoost = Instantiate(speedBoostPrefab, spawnPoint, Quaternion.identity).transform;

            }


            sr.sprite = damagedSprite;


            //Vector2 posDelta = FindObjectOfType<PlayerLogic>().transform.position - transform.position;
            //Vector2 downVector = (Vector2)transform.position - Vector2.down;
            //posDelta.Normalize();

            //float angle = Vector2.Angle(downVector, posDelta);
            //UnityEngine.Debug.Log(angle);
            foreach(ParticleSystem particle in destroyParticle)
            {
                Instantiate(particle, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }

            //Setting layer to Destroyed for EnemyAI.
            gameObject.layer = 12;
            isDamaged = true;

            int j = Random.Range(0, SFX.Length);
            //Add audio sprite change logic here;
            FMODUnity.RuntimeManager.PlayOneShot(SFX[j], transform.position);
        }

    }
}
