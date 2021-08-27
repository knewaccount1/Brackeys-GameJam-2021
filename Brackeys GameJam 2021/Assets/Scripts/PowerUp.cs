using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Power up collision");
        if (collision.CompareTag("Player"))
        {
            //Play pick up audio and add effects to player;

            Destroy(gameObject);

        }
    }
}
