using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jump;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time has passed since the plaer ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;
    [SerializeField] private int wallJumpPowerX;
    [SerializeField] private int wallJumpPowerY;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;


    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    [SerializeField] private Transform wallCheck;

    private float wallSlidingSpeed = 2f;


    private void Awake()
    {
        //grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //flip player to face direction of movement
        if (horizontalInput>0.01f)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1,1,1);
        }


        //Set animator parameters
        anim.SetBool("Running", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Adjustable Jump Height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2); //When player releases space they will fall faster

       
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter when on ground
                jumpCounter = extraJumps; //Reset extra jumps when on ground
            }
            else
                coyoteCounter -= Time.deltaTime; //start decreasing coyote counter when not on ground
        

        WallSlide();
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && jumpCounter<=0) return; //If coyote counter is 0 or less and don't have extra jumps left don't do anything

        SoundManager.instance.PlaySound(jumpSound);

        
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jump);
            else
            {
                if (onWall())
                {
                    body.velocity = new Vector2(wallJumpPowerX, wallJumpPowerY);
                    jumpCounter--;
                }
                //If not on the ground and coyote counter bigger than 0 do a normal jump
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jump);
                else
                {
                    if (jumpCounter > 0) //If we have extra jumps jump and decrement
                    {
                        body.velocity = new Vector2(body.velocity.x, jump);
                        jumpCounter--;
                    }
                }
            }
            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        
    }

    private void WallSlide()
    {
        if(onWall() && !isGrounded() && horizontalInput != 0f)
        {
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
   
    }

    private bool isGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return rayCastHit.collider != null;
    }

    private bool onWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

}
