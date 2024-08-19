using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    GameManager gm;
    SpriteRenderer sprite;
    public float walkSpeed;
    bool facingRight = true;
    public float h;

    [Header("GroundCheck")]
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    
    [Header("Dash")]
    public TrailRenderer trail;
    bool canDash = true;
    bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    

    [Header("CoyoteTime")]
    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    [Header("JumpBuffer")]
    public float jumpBufferTime = 0.2f;
    public float jumpBufferCounter;

    [Header("Jump")]
    public int jump = 2;
    public int jumpCount;
    public float jumpPower = 8f;
    public int boostPower;

   

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        jumpCount = jump;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();

        if (isDashing)
        {
            return;
        }

        if(Input.GetButton("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }

       
        
            Flip();
        
    }

    void FixedUpdate()
    {

        if (isDashing)
        {
            return;
        }

        h = Input.GetAxisRaw("Horizontal");


        
        rigid.velocity = new Vector2(h * walkSpeed, rigid.velocity.y);
            
        
    }

    void Flip()
    {
        //flip
        if (facingRight && h < 0 || !facingRight && h > 0)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public bool isGround()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);

    }

    //jump
    void Jump()
    {
        //coyote
        if (isGround())
        {
            coyoteTimeCounter = coyoteTime;
            jumpCount = jump;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        //////jump buffer
        //if (Input.GetButtonDown("Jump"))
        //{
        //    jumpBufferCounter = jumpBufferTime;
        //}
        //else
        //{
        //    jumpBufferCounter -= Time.deltaTime;
        //}

        //jump
        if (coyoteTimeCounter > 0f && Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rigid.velocity = Vector2.up * jumpPower;
        }

        if (coyoteTimeCounter < 0f && Input.GetButtonDown("Jump")&& jumpCount > 0)
        {
            rigid.velocity = (Vector2.up * jumpPower);
            jumpCount--;
            coyoteTimeCounter = 0f;
            Debug.Log("Jump");
        }
        
    }

    

   
    //dash
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        rigid.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "JumpItem")
        {
            Debug.Log("reset jumpCount");
            jumpCount = 2;
            jumpCount--;
            

            //deactivate
            collision.gameObject.SetActive(false);
        }

        if(collision.gameObject.tag == "boost")
        {
            
            rigid.velocity = new Vector2(rigid.velocity.x, (rigid.velocity.y * boostPower));
            

            collision.gameObject.SetActive(false);
        }
    }
}

















//public bool isGrounded()
//{
//    if (Physics2D.CircleCast(transform.position, circleCastFloat, Vector2.down, circleCastDis))
//        return true;
//    else
//        return false;
//}

/* ////coyote time
if (isGround())
{
   coyoteTimeCounter = coyoteTime;
   jumpCount = jump;
}
else
{
   coyoteTimeCounter -= Time.deltaTime;
}

//jump buffer
if (Input.GetButtonDown("Jump"))
{
   jumpCount--;
   jumpBufferCounter = jumpBufferTime;
}

else
{
   jumpBufferCounter -= Time.deltaTime;
}

//jump
if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && jumpCount > 0)
{
   rigid.velocity = Vector2.up * jumpPower;
   jumpBufferCounter = 0f;

}


if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0f)
{
   rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.5f);
   coyoteTimeCounter = 0f;

}*/