using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    private GameObject player;
    private float speed;


    // setter
    public void setPlayer(GameObject player)
    {
        this.player = player;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }


    public void handleChase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
