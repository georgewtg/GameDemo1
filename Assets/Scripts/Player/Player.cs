using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // unity stuff
    private Animator animator;

    // other scripts
    private Movement movement;
    private Jump jump;
    private Attack attack;

    // variables
    [SerializeField] private float speed;
    private float horizontal;
    private bool isFacingRight = true;

    private Transform groundCheck;
    private LayerMask groundLayer;
    [SerializeField] private float jumpingPower;

    [SerializeField] private GameObject slash;
    [SerializeField] private Vector3 weaponOffset;
    [SerializeField] private int attackFrame;
    private int attackFrameCounter = 0;

    private bool isFighting = false;
    private LayerMask arenaLayer;
    [SerializeField] private GameObject arenaTrigger;
    [SerializeField] private GameObject door;


    // Start is called before the first frame update
    void Start()
    {
        // get unity stuff
        animator = GetComponent<Animator>();

        // get scripts
        movement = FindObjectOfType<Movement>();
        jump = FindObjectOfType<Jump>();
        attack = FindObjectOfType<Attack>();

        // get variables
        groundCheck = this.transform.Find("GroundCheck");
        groundLayer = LayerMask.GetMask("Ground");
        arenaLayer = LayerMask.GetMask("Arena");

        // set variables
        movement.setSpeed(speed);
        jump.setGroundCheck(groundCheck);
        jump.setGroundLayer(groundLayer);
        jump.setJumpingPower(jumpingPower);
        attack.setSlash(slash);
        attack.setWeaponOffset(weaponOffset);
    }

    // Update is called once per frame
    void Update()
    {
        // set dynamic variables
        attack.setIsFacingRight(isFacingRight);
        checkFightingState();

        handleFlip(); // flip image for left and right
        jump.handleJump(); // jump
        attack.handleAttack(); // slash attack

        // debug commands (to be removed)
        movement.setSpeed(speed);
        jump.setJumpingPower(jumpingPower);
    }

    private void FixedUpdate()
    {
        movement.handleMovement(); // move left and right
        handleAnimation();
    }

    private void handleAnimation()
    {
        // idle <--> walk
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));

        // finish attack animation
        if (attack.isAttacking())
        {
            attackFrameCounter++;

            if (attackFrameCounter >= attackFrame)
            {
                attack.destroyAttack();
                attackFrameCounter = 0;
            }
        }
    }

    private void handleFlip()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (!attack.isAttacking() && isFacingRight && horizontal < 0f || !attack.isAttacking() && !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public bool checkFightingState()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, arenaLayer))
        {
            isFighting = true;
            Destroy(arenaTrigger);
            door.SetActive(true);
        }
        return isFighting;
    }
}
