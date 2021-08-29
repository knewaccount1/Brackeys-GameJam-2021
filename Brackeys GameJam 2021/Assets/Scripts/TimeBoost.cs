using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBoost : PowerUp
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // give the player a speed boost
            player.BoostSpeedStart(5f);

            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

}
