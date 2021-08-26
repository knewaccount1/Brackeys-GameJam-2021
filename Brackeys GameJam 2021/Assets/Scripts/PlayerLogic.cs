using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLogic : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveDirection;
    public Rigidbody2D rb2D;
    public bool isTackling;
    private Vector2 movement;
    private float hitAnimationTime;
    private bool canMove;
    private SpriteRenderer playerSpriteRenderer;
    private Sprite playerSprite;

    [SerializeField][Range (0.1f,5)] private float tackleAccTime;
    [SerializeField][Range (-1,6)] private float tackleDistMul;
    
    void Awake()
    {
        // Variables initial values
        moveSpeed = 10f;
        tackleAccTime = 1.7f;
        tackleDistMul = 4;
        hitAnimationTime = 1f;
        canMove = true;
    }

    void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = this.GetComponent<SpriteRenderer>();
        isTackling = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check movement and tackle here
        if (!isTackling)
        {
            CheckMovement();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartTackleSequence();
            }
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
        else
        {
            // Player is  currently in a tackle procedure - check for collisions - if collided stop the sequence, start animation for hit and destroy object in moveDirection, after animation return control to player

        }
    }
    
    private void OnCollisionEnter2D(Collider2D collider)
    {
        if (isTackling)
        {
            Sequence playerHitSeq = DOTween.Sequence();
            playerHitSeq.SetId("TackleHit");

            Debug.LogWarning("I have collided during tackle!!!!");
            DOTween.Kill("TackleAcceleration");

            playerHitSeq.Insert(0, DOVirtual.DelayedCall(hitAnimationTime, HitAnimationEnded));
        }
        Debug.LogWarning("I have collided!");

    }

    private void StartTackleSequence()
    {
        isTackling = true;

        Sequence playerTackleSeq = DOTween.Sequence();
        playerTackleSeq.SetId("TackleAcceleration");
        
        playerTackleSeq.Insert(0f ,rb2D.DOMove(transform.position + moveDirection * tackleDistMul, tackleAccTime).SetEase(Ease.OutQuint));
        playerTackleSeq.InsertCallback(tackleAccTime, EndTackleSequence);
    }

    private void EndTackleSequence()
    {
        isTackling = false;
        Debug.LogWarning("I have called back!");
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
    }


    private void MovePlayer(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.up)
        {
            playerSprite = (Sprite)Resources.Load("Player Sprites/PlayerStand_Back", typeof(Sprite));
            playerSpriteRenderer.flipX = false;
        }
        else if (moveDirection == Vector3.down)
        {
            playerSprite = (Sprite)Resources.Load("Player Sprites/PlayerStand_Front", typeof(Sprite));
            playerSpriteRenderer.flipX = false;
        }
        else if (moveDirection == Vector3.left)
        {
            playerSprite = (Sprite)Resources.Load("Player Sprites/PlayerStand_Left", typeof(Sprite));
            playerSpriteRenderer.flipX = false;
        }
        else
        {
            playerSprite = (Sprite)Resources.Load("Player Sprites/PlayerStand_Left", typeof(Sprite));
            playerSpriteRenderer.flipX = true;
        }
        playerSpriteRenderer.sprite = playerSprite;
        Vector3 newPosition = transform.position;
        newPosition.x = transform.position.x + moveDirection.x * moveSpeed * Time.deltaTime;
        newPosition.y = transform.position.y + moveDirection.y * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
        canMove = false;
    }

    


}
