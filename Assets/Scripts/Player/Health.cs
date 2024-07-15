using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // variables
    private int health;
    private int invulnerabilityFrame;
    private bool invulnerable = false;


    // setter
    public void setHealth(int health)
    {
        this.health = health;
    }


    // spawn health indicator

    // handle heal and damage

    // heal???

    // damage???
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invulnerable && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player hit!");
        }
    }
}
