using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Aggressive : EnemyAI
{
    [Header("Aggressive Unit Specific Parameters")]
    public float slowDuration;

    public override void Idle()
    {
        //animator.SetBool("isWalking", false);

        if (Vector3.Distance(transform.position, target.transform.position) < aggroDistance)
        {
            currentState = EnemyState.SAWPLAYER;
        }
    }

    public override void SawPlayer()
    {

        //currentState = EnemyState.chasing;
        if (!hasSpottedPlayer)
        {
            /*visualisation of enemy animation*/

            animator.SetTrigger("Surprised");
            //enemyGFX.localScale = new Vector3(enemyGFX.localScale.x * 2, enemyGFX.localScale.x * 2, enemyGFX.localScale.x * 2);
            hasSpottedPlayer = true;
        }

        if (reactionTimeCountdown < 0)
        {
            reactionTimeCountdown = reactionTime;
            currentState = EnemyState.CHASING;
        }
        {
            reactionTimeCountdown -= Time.deltaTime;
        }

    }

    public override void Chasing()
    {

        AStarPathFinding();

        float distanceDelta = Vector3.Distance(transform.position, target.transform.position);

        //Animation Spaghetti code
        Vector2 posDelta = target.transform.position - transform.position;
        if (Mathf.Abs(posDelta.x) < 3)
        {
            if (posDelta.y >= 2)
            {
                animator.Play("employee back anim");
            }
            else if (posDelta.y <= -2)
            {
                animator.Play("employee front anim");
            }
            else
            {
                animator.Play("employee side anim");
            }
        }
        else
        {

            animator.Play("employee side anim");
        }

        if (distanceDelta > chaseDistance)
        {
            if(timeBeforeIdleCountdown <= 0)
            {
        
                timeBeforeIdleCountdown = timeBeforeIdle;
                enableChase = false;
                currentState = EnemyState.IDLE;
            }
            else
            {
                timeBeforeIdleCountdown -= Time.deltaTime;
            }

        }
        else
        {
            timeBeforeIdleCountdown = timeBeforeIdle;
        }

    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            collision.GetComponentInParent<PlayerLogic>().SlowEffect(slowDuration);
        }
    }

}
