using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // unity stuff
    private Transform groundCheck;
    private LayerMask groundLayer;


    // setter
    public void setGroundCheck(Transform groundCheck)
    {
        this.groundCheck = groundCheck;
    }

    public void setGroundLayer(LayerMask groundLayer)
    {
        this.groundLayer = groundLayer;
    }


    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
