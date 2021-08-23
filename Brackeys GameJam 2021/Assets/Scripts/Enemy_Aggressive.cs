using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Aggressive : EnemyAI
{


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
            enemyGFX.localScale = new Vector3(enemyGFX.localScale.x * 2, enemyGFX.localScale.x * 2, enemyGFX.localScale.x * 2);
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
     
        if(distanceDelta > chaseDistance)
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

}
