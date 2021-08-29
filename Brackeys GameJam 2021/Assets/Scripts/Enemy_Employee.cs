using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Employee : EnemyAI
{
    [Header ("Employee Specfic Attributes")]

    public float searchDistance = 15f;
    public float slowTime;
    public float repairTime;
    private float timeRepairing;
    public LayerMask destroyedLayer;
    public Interactable interactableToRepair;
    [SerializeField] Collider2D[] hit2D;
    public GameObject hammer;

    

    bool isRepairing;

    public void SearchForInteractables()
    {
        //destroyedLayer = 1 << 10;
        hit2D = Physics2D.OverlapCircleAll(transform.position, searchDistance, destroyedLayer);

        //hit2D = Physics2D.CircleCast(transform.position, searchDistance, Vector2.right, 0, interactableLayer);

    }

    //public override void Initialize()
    //{
    //    hit2D = new Collider2D[0];
    //}

    public override void Idle()
    {
        //animator.SetBool("isWalking", false);
        SearchForInteractables();

        if (Vector3.Distance(transform.position, target.transform.position) < aggroDistance)
        {
            currentState = EnemyState.SAWPLAYER;
        }
        else if (hit2D.Length >0)
        {
            int i = Random.Range(0, hit2D.Length);
            if (hit2D[i].GetComponent<Interactable>() != null)
            { 
                interactableToRepair = hit2D[i].GetComponent<Interactable>();
                if (interactableToRepair.isDamaged)
                {
                    if (Vector3.Distance(transform.position, interactableToRepair.transform.position) < searchDistance)
                    {
                        currentState = EnemyState.UNIQUE_CHASE;
                    }
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

    public override void Attacking()
    {
        timeBtwAttacks -= Time.deltaTime;

        if (timeBtwAttacks <= 0 && !isAttacking)
        {
            
            StartCoroutine(Attack());
        }
    }

    //Employee slows players
    IEnumerator Attack()
    {
        


        yield return new WaitForEndOfFrame();
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
                    timeRepairing = repairTime;
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

        timeRepairing -= Time.deltaTime;
        hammer.SetActive(true);

        if (timeRepairing <= 0)
        {
            interactableToRepair.RepairObject();
            timeRepairing = repairTime;
        }


        if (!interactableToRepair.isDamaged)
        {
            hammer.SetActive(false);
            target = playerRef;
            currentState = EnemyState.IDLE;
        }
    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            //collision.GetComponentInParent<PlayerLogic>().SlowEffect(slowTime);
        }
    }
}
