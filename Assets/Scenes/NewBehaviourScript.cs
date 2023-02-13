using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rb ;
    private void Awake () {
        rb = GetComponent <Rigidbody2D>();
    }
    private void FixedUpdate() {
        rb.velocity = new Vector2 (2f ,2f);
    } 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        rb.velocity = Vector2.up * 10;
        }
    }
} */

public class NewBehaviourScript : MonoBehaviour
{
    public Animator animator;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;

    private bool doubleJump; 

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask groundLayer;

    private enum MovementState { idle, jump, fall };
    private MovementState State;
    
    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                 rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                 doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        

        Flip();
        UpdateAnimation();
    }

    

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
    private void UpdateAnimation()
    {
        MovementState state;
        if (rb.velocity.y > 0f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < 0f)
        {
            state = MovementState.fall;
        }
        else
        {
            state = MovementState.idle;
        }

        animator.SetInteger("State", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);
            doubleJump = true;
        }
    }
}
