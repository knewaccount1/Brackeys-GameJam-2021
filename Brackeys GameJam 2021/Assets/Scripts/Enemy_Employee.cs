using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Employee : EnemyAI
{
    [Header ("Employee Specfic Attributes")]

    public float searchDistance = 15f;
    public LayerMask interactableLayer;
    public Interactable interactableToRepair;
    Collider2D[] hit2D;

    public void SearchForInteractables()
    {
        interactableLayer = 1 << 10;
        hit2D = Physics2D.OverlapCircleAll(transform.position, searchDistance, interactableLayer);
        Debug.Log(hit2D.Length);
        //hit2D = Physics2D.CircleCast(transform.position, searchDistance, Vector2.right, 0, interactableLayer);


    }

    public override void Idle()
    {
        //animator.SetBool("isWalking", false);
        SearchForInteractables();

        if (Vector3.Distance(transform.position, target.transform.position) < aggroDistance)
        {
            currentState = EnemyState.SAWPLAYER;
        }
        else if (hit2D != null)
        {
            interactableToRepair = hit2D[0].GetComponent<Interactable>();
            if (interactableToRepair.isDamaged)
            {
                if (Vector3.Distance(transform.position, interactableToRepair.transform.position) < searchDistance)
                {
                    currentState = EnemyState.UNIQUE_CHASE;
                }
            }

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
        Debug.Log(distanceDelta);
        if (distanceDelta > chaseDistance)
        {
            if (timeBeforeIdleCountdown <= 0)
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

    public override void UniqueChase()
    {


        if(interactableToRepair != null)
        {
            if (interactableToRepair.isDamaged)
            {
                target = interactableToRepair.transform;

                AStarPathFinding();

                float posDelta = Vector2.Distance(target.transform.position, transform.position);

                Debug.Log("Moving towards repairable object");
                if (posDelta <= attackRange)
                {
                    currentState = EnemyState.UNIQUE_ATTACK;
                }
            }
            else
            {
                target = playerRef;
                currentState = EnemyState.IDLE;
                
            }

        }
        
        
    }

    public override void UniqueAttack()
    {
        //Repair the object here
        
        interactableToRepair.RepairObject();

        if (!interactableToRepair.isDamaged)
        {
            target = playerRef;
            currentState = EnemyState.IDLE;
        }
    }

    public class DistanceCompare : IComparer
    {
        public int Compare(object x, object y)
        {
            throw new System.NotImplementedException();
        }
    }
}
