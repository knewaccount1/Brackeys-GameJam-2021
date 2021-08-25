using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLogic : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Vector3 moveDirection;
    public Rigidbody2D rb2D;
    public bool isTackling;
    private Vector2 movement;
    

    void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
        isTackling = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if player is not tackling, check if he presses tackle, if he does - start tackle, else: move him.
        if (!isTackling)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isTackling = true;
                //initiate tackle procedure
                // dotween player in the move direction for tackle
                
                Sequence playerTackleSeq = DOTween.Sequence();
                playerTackleSeq.Insert(0f ,transform.DOMove(transform.position + moveDirection, 1f));
                playerTackleSeq.InsertCallback(2f, EndTackleSequence);

                //rb2D.AddForce(moveDirection);
            }
            else
                CheckMovement();
        }
        else
        {
            // Check if during tackle procedure a collision was made with an interactable - if so, kill all tweens and initiate end tackle procedure + interactable destruction in move direction
            
        }

        // Input
        // movement.x = Input.GetAxisRaw("Horizontal");
        // movement.y = Input.GetAxisRaw("Vertical");
    }

    private void EndTackleSequence()
    {
        isTackling = false;
        Debug.LogWarning("I have called back!");
    }
    private void CheckMovement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection = Vector3.up;
            MovePlayer(moveDirection);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection = Vector3.down;
            MovePlayer(moveDirection);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = Vector3.left;
            MovePlayer(moveDirection);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = Vector3.right;
            MovePlayer(moveDirection);
        }
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        Vector3 newPosition = transform.position;
        newPosition.x = transform.position.x + moveDirection.x * moveSpeed * Time.deltaTime;
        newPosition.y = transform.position.y + moveDirection.y * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    // FixedUpdate is called on a fixed timer
    void FixedUpdate()
    {
        // Movement
        //rb2D.MovePosition(rb2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


}
