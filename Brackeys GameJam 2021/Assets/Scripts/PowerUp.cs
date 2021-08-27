using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUp : MonoBehaviour
{
    public PlayerLogic player;

    private string powerType;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Play pick up audio and add effects to player;

            Destroy(gameObject);

        }
    }

    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerLogic>();

        transform.localScale = Vector3.zero;
        
        Sequence spawnSeq = DOTween.Sequence();
        spawnSeq.Insert(0f, transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        
        // Flashing red
        spawnSeq.Insert(6f, transform.GetComponentInChildren<SpriteRenderer>().DOColor(Color.red, 0.5f).SetEase(Ease.InCubic));
        spawnSeq.Insert(6.5f, transform.GetComponentInChildren<SpriteRenderer>().DOColor(Color.white, 0.5f).SetEase(Ease.OutCubic));

        spawnSeq.Insert(8f, transform.GetComponentInChildren<SpriteRenderer>().DOColor(Color.red, 0.5f).SetEase(Ease.InCubic));
        spawnSeq.Insert(8.5f, transform.GetComponentInChildren<SpriteRenderer>().DOColor(Color.white, 0.5f).SetEase(Ease.OutCubic));
        
        //Popping out then destroying
        spawnSeq.Insert(0f, DOVirtual.DelayedCall(10f, VanishBoost));
    }

    protected void VanishBoost()
    {
        Sequence boostSpawnSeq = DOTween.Sequence();
        boostSpawnSeq.Insert(0f, transform.DOScale(0, 0.3f).SetEase(Ease.InBack));
        boostSpawnSeq.Insert(0.3f, DOVirtual.DelayedCall(0f, DestroyBoost));
    }

    protected void DestroyBoost()
    {
        Destroy(gameObject);
    }


    public void SetPowerType(string powerType)
    {
        this.powerType = powerType;
    }
}
