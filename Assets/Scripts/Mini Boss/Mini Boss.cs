using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{
    // unity stuff
    Rigidbody2D rb;
    private Transform groundCheck;
    private LayerMask groundLayer;

    // other scripts
    private BossAttack bossAttack;
    private GroundCheck groundCheckScript;


    // variables
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float jumpingPower;
    private float horizontal;
    private bool isFacingRight = true;

    [SerializeField] private GameObject slashes;

    [SerializeField] private int slashAttackFrame;
    [SerializeField] private int dashAttackFrame;
    [SerializeField] private int plungeAttackFrame;
    private int attackFrameCounter = 0;
    [SerializeField] private int cooldownFrame;
    private int cooldownFrameCounter = 0;

    private float distance;
    private Vector2 direction;


    // Start is called before the first frame update
    void Start()
    {
        // get unity stuff
        rb = GetComponent<Rigidbody2D>();
        groundCheck = this.transform.Find("GroundCheck");
        groundLayer = LayerMask.GetMask("Ground");

        // get scripts
        bossAttack = FindObjectOfType<BossAttack>();
        groundCheckScript = FindObjectOfType<GroundCheck>();

        // set variables
        bossAttack.setDashSpeed(dashSpeed);
        bossAttack.setSlashes(slashes);
        groundCheckScript.setGroundCheck(groundCheck);
        groundCheckScript.setGroundLayer(groundLayer);
    }

    // Update is called once per frame
    void Update()
    {
        // set dynamic variables
        distance = Vector2.Distance(transform.position, player.transform.position); // count distance
        direction = player.transform.position - transform.position; // check direction

        bossAttack.setDistance(distance);
        if (!bossAttack.isChanneling() && !bossAttack.isMoving() && !bossAttack.isAttacking() && groundCheckScript.isGrounded()) // set attack target position before attacking
            bossAttack.setTargetposition(new Vector3(player.transform.position.x, transform.position.y, transform.position.z));
        bossAttack.setIsFacingRight(isFacingRight);

        handleFlip();
    }


    private void FixedUpdate()
    {
        bossAttack.handleAttack(); // attack

        handleAnimation();
        handleCooldown();
    }


    private void handleAnimation()
    {
        // finish attack animation
        if (bossAttack.isAttacking())
        {
            attackFrameCounter++;

            if (attackFrameCounter >= slashAttackFrame)
            {
                bossAttack.destroyAttack();
                attackFrameCounter = 0;
            }
        }
    }

    private void handleCooldown()
    {
        if (bossAttack.isCooldown())
        {
            cooldownFrameCounter++;

            if (cooldownFrameCounter >= cooldownFrame)
            {
                bossAttack.setCooldown(false);
                cooldownFrameCounter = 0;
            }
        }
    }

    private void handleFlip()
    {
        if (!bossAttack.isAttacking() && direction.x > 0)
            isFacingRight = true;
        else if (!bossAttack.isAttacking() && direction.x < 0)
            isFacingRight = false;

        if (!bossAttack.isAttacking() && isFacingRight && horizontal < 0f || !bossAttack.isAttacking() && !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
