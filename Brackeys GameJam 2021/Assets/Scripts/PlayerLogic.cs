using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMOD;
using FMODUnity;

public class PlayerLogic : MonoBehaviour
{
    public Animator playerAnimator;
    public float moveSpeed;
    private float origMoveSPeed;
    private Vector3 moveDirection;
    public Rigidbody2D rb2D;
    public bool isTackling;
    public float tackleCooldown;
    private Vector2 movement;
    [SerializeField]private float hitAnimationTime;
    private bool canMove;
    private SpriteRenderer playerSpriteRenderer;
    private Sprite playerSprite;
    public Camera cameraObject;
    public bool speedBoosted;
    public bool canTackle;

    private GameManager GM;
    public ParticleSystem speedAura;
    [SerializeField][Range (0.1f,5)] private float tackleAccTime;
    [SerializeField][Range (-1,6)] private float tackleDistMul;




    //Added by Ed to test Enemy AI. Can be changed/edited later
    [SerializeField] bool isStunned;

    [Header("FMOD")]
    public string powerUpEvent;

    void Awake()
    {
        // Variables initial values
        origMoveSPeed = moveSpeed;

        canMove = true;
        speedBoosted = false;
        canTackle = true;
        GM = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = this.GetComponent<SpriteRenderer>();
        isTackling = false;
        playerAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check movement and tackle here
        if (!isTackling)
        {
            CheckMovement();
            if (Input.GetKeyDown(KeyCode.Space) && canTackle)
            {
                StartTackleSequence();
            }
        }
        else
        {
            // Player is  currently in a tackle procedure - start animation for tackle

        }
    }

    // FixedUpdate is called on a fixed timer
    void FixedUpdate()
    {
        // perform stuff here
        if (!isTackling)
        {
            if (canMove)
            {
                // if player is not tackling, check if he presses tackle, if he does - start tackle, else: move him.
                MovePlayer(moveDirection);
            }   
        }
    }
    

    //This was added to place a delay between hitting an object and returning tackle state to true
    void SetTackleFalse()
    {
        isTackling = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform collider = collision.collider.transform;
        if (collider.tag == "Interactable")
        {
            if (isTackling)
            {
                Invoke("SetTackleFalse", .5f);
                collider.gameObject.GetComponent<Interactable>().DestroyInteractable();

                GM.ShakeCam();
                Vector2 posDelta = transform.position - collider.transform.position;
                posDelta.Normalize();
                rb2D.AddForce(-moveDirection * 15f, ForceMode2D.Impulse);

                // -------------------THIS CODE MOVED TO INTERACTABLES CLASS--------------------
                //Stop tackle animation and/or start tackle hit animation
                // BoxBounds boxBounds = collider.parent.Find("DropBounds").GetComponent<BoxBounds>();
                // Vector2 spawnPoint = boxBounds.RandomPointInBounds();
                // var powerUp = Instantiate((GameObject)Resources.Load("Prefabs/Boost", typeof(GameObject)), spawnPoint, Quaternion.identity);
                // Debug.LogWarning(spawnPoint);
                // -----------------------------------------------------------------------------

                Sequence playerHitSeq = DOTween.Sequence();
                playerHitSeq.SetId("TackleHit");

                DOTween.Kill("TackleStart");

                playerHitSeq.Insert(0f, cameraObject.transform.DOShakePosition(0.2f, 0.2f, 100, 90f, false));
                playerHitSeq.Insert(0, DOVirtual.DelayedCall(hitAnimationTime, HitAnimationEnded));
            }
        }
        else if (collider.tag == "TimeBoost")
        {

        }
        else if (collider.tag == "SpeedBoost")
        {

        }

    }

    public void BoostSpeedStart(float speedBoostDuration)
    {
        FMODUnity.RuntimeManager.PlayOneShot(powerUpEvent, transform.position);
        moveSpeed = origMoveSPeed * 1.3f;
        Sequence moveSpeedSeq = DOTween.Sequence();
        moveSpeedSeq.Insert(0f, DOVirtual.DelayedCall(speedBoostDuration ,BoostSpeedEnd));
        speedAura.gameObject.SetActive(true);

    }
    public void BoostTime()
    {
        GM.AddTime();
    }
    private void BoostSpeedEnd()
    {
        speedAura.gameObject.SetActive(false);
        moveSpeed = origMoveSPeed;
    }

    private void StartTackleSequence()
    {
        isTackling = true;
        canTackle = false;

        Sequence tackleDisabled = DOTween.Sequence();
        tackleDisabled.SetId("TackleDisabled");

        Sequence playerTackleSeq = DOTween.Sequence();
        playerTackleSeq.SetId("TackleStart");
        
        playerTackleSeq.Insert(0f ,rb2D.DOMove(transform.position + moveDirection * tackleDistMul, tackleAccTime));
        playerTackleSeq.InsertCallback(tackleAccTime, EndTackleSequence);
        
        tackleDisabled.InsertCallback(tackleCooldown, EnableTackle);
    }
    private void EnableTackle()
    {
        canTackle = true;
        DOTween.Kill("TackleDisabled");
    }

    private void EndTackleSequence()
    {
        isTackling = false;
  
    }

    private void HitAnimationEnded()
    {
        isTackling = false;
    }


    private void CheckMovement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection = Vector3.up;
            canMove = true;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection = Vector3.down;
            canMove = true;
            
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = Vector3.left;
            canMove = true;
            
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = Vector3.right;
            canMove = true;
            
        }
        else
        {
            moveDirection = Vector3.zero;
            canMove = true;
        }
    }


    private void MovePlayer(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.up)
        {
            //playerSprite = (Sprite)Resources.Load("Player Sprites/PlayerStand_Back", typeof(Sprite));

            playerAnimator.Play("kid back anim");

            playerSpriteRenderer.flipX = false;
            //playerAnimator.runtimeAnimatorController = Resources.Load("Prefabs/Animations/kid sprite back_0") as RuntimeAnimatorController;
        }
        else if (moveDirection == Vector3.down)
        {
            //playerSprite = (Sprite)Resources.Load("Player Sprites/PlayerStand_Front", typeof(Sprite));

            playerAnimator.Play("kid front anim");


            playerSpriteRenderer.flipX = false;
            //playerAnimator.runtimeAnimatorController = Resources.Load("Prefabs/Animations/kid sprite front_0") as RuntimeAnimatorController;
        }
        else if (moveDirection == Vector3.left)
        {
           //playerSprite = (Sprite)Resources.Load("Player Sprites/PlayerStand_Left", typeof(Sprite));

            playerAnimator.Play("kid side anim");
            playerSpriteRenderer.flipX = false;
            //playerAnimator.runtimeAnimatorController = Resources.Load("Prefabs/Animations/kid sprite side_0") as RuntimeAnimatorController;
        }
        else if(moveDirection == Vector3.right)
        {
            //playerSprite = (Sprite)Resources.Load("Player Sprites/PlayerStand_Left", typeof(Sprite));
            playerAnimator.Play("kid side anim");
            playerSpriteRenderer.flipX = true;
            //playerAnimator.runtimeAnimatorController = Resources.Load("Prefabs/Animations/kid sprite side_0") as RuntimeAnimatorController;
        }
        else
        {
            playerAnimator.Play("kid idle anim");
            //do nothing
        }

        playerSpriteRenderer.sprite = playerSprite;
        Vector3 newPosition = transform.position;

        rb2D.velocity = moveDirection * moveSpeed;
        //newPosition.x = transform.position.x + moveDirection.x * moveSpeed * Time.deltaTime;
        //newPosition.y = transform.position.y + moveDirection.y * moveSpeed * Time.deltaTime;
        //transform.position = newPosition;
        canMove = false;
    }




    /// <summary>
    /// Added Slow/Stun methods inside of the player logic that is called by the Enemy on collision
    /// Update as needed
    /// </summary>

    public void SlowEffect(float slowTimer)
    {
        StartCoroutine(SlowEffectCoroutine(slowTimer));
    }
    IEnumerator SlowEffectCoroutine(float slowTimer)
    {

        moveSpeed = origMoveSPeed / 3;
        yield return new WaitForSeconds(slowTimer);
        moveSpeed = origMoveSPeed;
    }

    public void StunEffect(float stunTime)
    {
        if (!isStunned)
            StartCoroutine(StunEffectCoroutine(stunTime));
    }

    IEnumerator StunEffectCoroutine(float stunTime)
    {
        canMove = false;
        isStunned = true;
        //Play stun effect/particles here
    
        yield return new WaitForSeconds(stunTime);
        canMove = true;
        yield return new WaitForSeconds(.5f);
        isStunned = false;

    }

}
