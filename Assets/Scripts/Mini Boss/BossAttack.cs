using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // unity stuff
    private Rigidbody2D rb;

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
    private float speed;
    private float dashSpeed;
    private float jumpingPower;

    private int counter = 0;


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

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void setJumpingPower(float jumpingPower)
    {
        this.jumpingPower = jumpingPower;
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
        rb = GetComponent<Rigidbody2D>();
    }


    public void handleAttack()
    {
        if (isFacingRight)
            weaponPosition = transform.position + weaponOffset;
        else
            weaponPosition = transform.position - weaponOffset;

        
        if (!attacking && !cooldown)
        {
            //slashAttack();
            //dashAttack();
            plungeAttack();
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
        if (counter == 0)
        {
            channeling = true;
            counter++;
        }
        else if (counter == 100)
            channeling = false;
        else if (channeling) counter++;

        // jump after channeling
        if (!channeling)
        {
            //rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            rb.gravityScale = 0;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition + new Vector3(0, 1, 0), dashSpeed * Time.deltaTime);
            moving = true;
        }

        // plunge attack
        if (transform.position == targetPosition)
        {
            Debug.Log("plunging");
            rb.gravityScale = 3;
            attacking = true;
        }
    }


    public void destroyAttack() // enter cooldown state
    {
        if (weapon != null)
            Destroy(weapon);
        attacking = false;
        cooldown = true;
        counter = 0;
    }
}
