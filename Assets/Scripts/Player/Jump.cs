using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // unity stuff
    private Rigidbody2D rb;
    private Transform groundCheck;
    private LayerMask groundLayer;

    // variables
    private float jumpingPower;


    // setter
    public void setGroundCheck(Transform groundCheck)
    {
        this.groundCheck = groundCheck;
    }

    public void setGroundLayer(LayerMask groundLayer)
    {
        this.groundLayer = groundLayer;
    }

    public void setJumpingPower(float jumpingPower)
    {
        this.jumpingPower = jumpingPower;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void handleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
