using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // unity stuff
    private Rigidbody2D rb;

    // variables
    private float speed;
    private Vector2 movement;


    // setter
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void handleMovement()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            movement = Vector2.zero;
        else if (Input.GetKey(KeyCode.A))
            movement = Vector2.left;
        else if (Input.GetKey(KeyCode.D))
            movement = Vector2.right;
        else
            movement = Vector2.zero;

        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }
}
