using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Movement : MonoBehaviour
{
    [Header ("Tuning Parameters")]
    public float hSpeed = 10f;
    public float vSpeed = 0;
    public float jumpForce = 5f;
    public float kbForce = 5f;
    public float tackleDistance = 1f;
    [SerializeField] private LayerMask tackleLayerMask;
    //[Range(0, 1.0f)] [SerializeField] float movementSmothing;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool facingRight = true;
    [SerializeField] private bool isGrounded = true;
    float horizontalMove;
    float verticalMove;




    //References
    [Header("References")]
    [SerializeField ]private BoxCollider2D groundCollider;
    [SerializeField] private LayerMask groundLayerMask;
    private SpriteRenderer sr;
    private Animator animator;
    private Rigidbody2D rb2D;
   

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        //if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("Pressed Space and Is Grounded");
        //    rb2D.velocity = Vector2.up * jumpForce;
        //    //rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //}


        //Handle Animations
        if (IsGrounded())
        {
            if (rb2D.velocity.x == 0)
            {
                animator.Play("isIdle");
            }
            else
            {
                animator.Play("isRunning");
            }
        }
        else
        {
            animator.Play("Jump");
        }


        if (Input.GetButtonDown("Fire1"))
        {
            //Hit();

        }
    }

    private void Hit()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, Vector2.right, tackleDistance, tackleLayerMask);

        if (hitInfo)
        {
            Interactable interactable = hitInfo.transform.GetComponent<Interactable>();
            if (interactable != null)
            {
                Debug.Log("Push object");
                interactable.Knockback(kbForce, this.transform);
            }
        }
    }

    private void FixedUpdate()
    {
        Move(horizontalMove, verticalMove, false);
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D =  Physics2D.BoxCast(groundCollider.bounds.center, groundCollider.bounds.size, 0f, Vector2.down, .1f, groundLayerMask);
        //Debug.Log("raycasthit: " + raycastHit2D.collider);
        return raycastHit2D.collider != null; //returns raycastHit2D if it is not null
    }

    public void Move(float hMove, float vMove, bool jump)
    {
       
        if (canMove)
        {
            Vector2 targetMove = new Vector3(hMove * hSpeed, vMove *vSpeed);

            rb2D.velocity = targetMove;

            //Flip character
            if (hMove > 0 && !facingRight)
                Flip();
            else if (hMove < 0 && facingRight)
                Flip();


            //Set Animation State (walking/Idle)
            if (hMove != 0 || vMove != 0)
                animator.SetBool("isWalking", true);
            else
                animator.SetBool("isWalking", false);
        }


    }


    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 tempScale = transform.localScale;
        tempScale = new Vector3(-tempScale.x, tempScale.y, tempScale.z);
        transform.localScale = tempScale;

        //bool currentSpriteFlip = sr.flipX;
        //sr.flipX = !currentSpriteFlip;

        //transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector2.right) * tackleDistance;
        Gizmos.DrawRay(transform.position, direction);
    }
}
