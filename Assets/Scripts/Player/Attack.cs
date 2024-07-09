using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private GameObject slash;

    private GameObject weapon;
    private Vector3 weaponPosition;
    private Vector3 weaponOffset;
    private bool attacking = false;
    private bool isFacingRight;

    
    // setter
    public void setSlash(GameObject slash)
    {
        this.slash = slash;
    }

    public void setWeaponOffset(Vector3 weaponOffset)
    {
        this.weaponOffset = weaponOffset;
    }

    public void setIsFacingRight(bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;
    }


    // getter
    public bool isAttacking()
    {
        return attacking;
    }


    public void handleAttack()
    {
        // set weapon position
        if (isFacingRight)
            weaponPosition = transform.position + weaponOffset;
        else
            weaponPosition = transform.position - weaponOffset;

        // spawn weapon in the right direction
        if (Input.GetKeyDown(KeyCode.J) && !attacking)
        {
            if (!isFacingRight)
            {
                Vector3 localScale = slash.transform.localScale;
                localScale.x = Mathf.Abs(localScale.x) * -1f;
                slash.transform.localScale = localScale;
            }
            else
            {
                Vector3 localScale = slash.transform.localScale;
                localScale.x = Mathf.Abs(localScale.x);
                slash.transform.localScale = localScale;
            }

            weapon = Instantiate(slash, weaponPosition, transform.rotation);
            attacking = true;
        } 
        else if (attacking)
        {
            weapon.transform.position = weaponPosition; // make slash follow player
        }
    }

    public void destroyAttack()
    {
        Destroy(weapon);
        attacking = false;
    }
}
