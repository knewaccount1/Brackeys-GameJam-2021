using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class EnemyAI : MonoBehaviour
{
    [Header ("References")]
    public GameManager GM;
    public Transform target;
    public Transform playerRef;
    public Transform enemyGFX;

    [Header("Editable Attributes")]
    public float speed = 200f;
    public float nextWaypointDistance = 3;

    Path path;
    int currentWaypoint = 0;
    public bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public int health = 3;
    public int maxHealth = 3;
    public string enemyName;
    public float aggroDistance = 5; //Distance which the an idle unit becomes aggro
    public float chaseDistance = 8; // Distance which an aggro'd unit will continue to chase player
    public float reactionTime = .5f; //Time between idle and aggro
    public float attackSpeed = 2f; //time between attacks;
    public float timeBeforeIdle = 2.5f; // Time an aggro'd enemy will chase a player that is out of chaseDistance

    [HideInInspector] public float reactionTimeCountdown; //An internal countdown for reaction time
    [HideInInspector] public float timeBeforeIdleCountdown; //an internal countdown for idletime
    [HideInInspector] public float timeBtwAttacks;

    public bool hasSpottedPlayer = false; 

    public float attackRange; //Range which a unit will interact with player when aggro'd

    public bool facingRight = false;

    public bool isDying;
    public bool isAttacking;

    public bool enableChase;

    //private Material matWhite;
    //private Material[] matDefault;
    //public SpriteRenderer[] sr;

    public Animator animator;
    public Collider2D triggerCollider;

    //public ParticleSystem deathParticle;

    //public AudioSource audioSource;
    //public AudioClip hitSound;
    //public AudioClip swingSound;

    public EnemyState currentState;
    public enum EnemyState
    {
        INITIALIZING,
        IDLE,
        SAWPLAYER,
        CHASING,
        ATTACKING,
        UNIQUE_CHASE,
        UNIQUE_ATTACK,
        DYING,
        
    }

    private void Awake()
    {
        target = FindObjectOfType<Player>().transform;
        playerRef = target;
        GM = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        reactionTimeCountdown = reactionTime;
        timeBeforeIdleCountdown = timeBeforeIdle;
        timeBtwAttacks = attackSpeed;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.2f);

        currentState = EnemyState.INITIALIZING;

        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.INITIALIZING:
                /*filling in the player reference for easier access*/
                currentState = EnemyState.IDLE;
                break;
            case EnemyState.IDLE:
                Idle();
                break;
            case EnemyState.SAWPLAYER:
                SawPlayer();
                break;
            case EnemyState.CHASING:
                enableChase = true;
                break;
            case EnemyState.ATTACKING:
                Attacking();
                break;
            case EnemyState.UNIQUE_CHASE:
                UniqueChase();
                break;
            case EnemyState.UNIQUE_ATTACK:
                UniqueAttack();
                break;
            default:
                break;
        }
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {

        if (enableChase == true)
        {
            Chasing();
        }
    }

    public virtual void AStarPathFinding()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        rb.velocity = force;
        //rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public virtual void Idle()
    {
    }
    public virtual void SawPlayer()
    {
    }
    public virtual void Chasing()
    {
    }
    public virtual void Attacking()
    {
    }
    public virtual void UniqueChase()
    {
    }

    public virtual void UniqueAttack()
    {
    }

    public void Flip()
    {
        facingRight = !facingRight;

        Vector3 tempScale = enemyGFX.localScale;
        tempScale = new Vector3(-tempScale.x, tempScale.y, tempScale.z);
        enemyGFX.localScale = tempScale;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
}


