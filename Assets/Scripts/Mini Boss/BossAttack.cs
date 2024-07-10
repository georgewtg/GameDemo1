using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // unity stuff
    private Rigidbody2D rb;

    // other scripts
    private GroundCheck groundCheckScript;

    // variables
    private float distance;
    private float threshold;
    private bool attacking = false;
    private bool moving = false;
    private bool channeling = false;
    private bool cooldown = true;
    private bool isFacingRight = true;

    private GameObject slashes;
    private GameObject weapon;

    private Vector3 weaponOffset = new Vector3(1, 0, 0);
    private Vector3 weaponPosition;
    private Vector3 targetPosition;
    private float dashSpeed;

    private int counter = 0;
    private List<int> attackList = new List<int> {1, 2, 3};
    private int attackListCounter = 0;
    private int nextAttack;


    // setter
    public void setDistance(float distance)
    {
        this.distance = distance;
    }

    public void setThreshold(float threshold)
    {
        this.threshold = threshold;
    }

    public void setTargetposition(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public void setDashSpeed(float dashSpeed)
    {
        this.dashSpeed = dashSpeed;
    }

    public void setSlashes(GameObject slashes)
    {
        this.slashes = slashes;
    }

    public void setIsFacingRight(bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;
    }

    public void setCooldown(bool cooldown)
    {
        this.cooldown = cooldown;
    }


    // getter
    public bool isAttacking()
    {
        return attacking;
    }

    public bool isCooldown()
    {
        return cooldown;
    }

    public bool isMoving()
    {
        return moving;
    }

    public bool isChanneling()
    {
        return channeling;
    }


    private void Start()
    {
        // get unity stuff
        rb = GetComponent<Rigidbody2D>();

        // get scripts
        groundCheckScript = FindObjectOfType<GroundCheck>();

        // setup attack
        setupAttack();
        nextAttack = attackList[attackListCounter];
    }


    public void handleAttack()
    {
        if (isFacingRight)
            weaponPosition = transform.position + weaponOffset;
        else
            weaponPosition = transform.position - weaponOffset;

        
        if (!attacking && !cooldown)
        {
            switch (nextAttack)
            {
                case 1:
                    slashAttack();
                    break;
                case 2:
                    dashAttack();
                    break;
                case 3:
                    plungeAttack();
                    break;
                default:
                    break;
            }
        }
        else if (attacking)
        {
            if (weapon != null)
                weapon.transform.position = weaponPosition; // make slashes follow player
        }
    }

    private void slashAttack()
    {
        if (!isFacingRight) // slash right 3 times
        {
            Vector3 localScale = slashes.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * -1f;
            slashes.transform.localScale = localScale;
        }
        else // slash left 3 times
        {
            Vector3 localScale = slashes.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            slashes.transform.localScale = localScale;
        }

        weapon = Instantiate(slashes, weaponPosition, transform.rotation);
        attacking = true;
    }

    private void dashAttack()
    {
        // channeling for 10 counts
        if (counter == 0)
        {
            channeling = true;
            counter++;
        }
        else if (counter == 10)
            channeling = false;
        else if (channeling) counter++;

        // dash to position after channeling
        if (!channeling)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
            moving = true;
        }

        // attack animation at the end of dash
        if (transform.position == targetPosition)
        {
            attacking = true;
            moving = false;
        }
    }

    private void plungeAttack()
    {
        // channeling for 10 counts
        if (counter == 0 && groundCheckScript.isGrounded())
        {
            channeling = true;
            counter++;
        }
        else if (counter == 100)
        {
            channeling = false;
            moving = true;
        }
        else if (channeling) counter++;

        // jump after channeling
        if (moving)
        {
            rb.gravityScale = 0;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition + new Vector3(0, 4.5f, 0), dashSpeed * Time.deltaTime);
        }

        // plunge attack
        if (Vector2.Distance(transform.position, targetPosition + new Vector3(0, 4.5f, 0)) < 0.01f)
        {
            rb.gravityScale = 3;
            attacking = true;
            moving = false;
        }
    }


    private void setupAttack()
    {
        // swap attack order around
        for (int i = 0; i < attackList.Count; i++)
        {
            int randomIndex = Mathf.Abs(Random.Range(0, 3));
            if (randomIndex != i)
            {
                int temp = attackList[i];
                attackList[i] = attackList[randomIndex];
                attackList[randomIndex] = temp;
            }
        }

        // debug stuff
        string attackOrder = "";
        for (int i = 0; i < attackList.Count; i++)
        {
            switch (attackList[i])
            {
                case 1:
                    attackOrder += "slash attack";
                    break;
                case 2:
                    attackOrder += "dash attack";
                    break;
                case 3:
                    attackOrder += "punge attack";
                    break;
                default:
                    break;
            }
            if (i != attackList.Count) attackOrder += " , ";
        }
        Debug.Log("attack order : [ " + attackOrder + " ]");
    }

    public void destroyAttack() // enter cooldown state
    {
        if (weapon != null)
            Destroy(weapon);
        attacking = false;
        cooldown = true;
        counter = 0;

        // setup next attack move
        if (attackListCounter < attackList.Count - 1)
        {
            attackListCounter++;
        }
        else
        {
            attackListCounter = 0;
            setupAttack();
        }

        nextAttack = attackList[attackListCounter];
    }
}
