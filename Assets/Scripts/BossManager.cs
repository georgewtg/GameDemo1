using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private Player player;

    private bool isCutscene = false;
    [SerializeField] private int cutsceneFrame;
    [SerializeField] private int cutsceneFrameCounter = 0;


    // Update is called once per frame
    void Update()
    {
        if (player.checkFightingState())
        {
            boss.SetActive(true);
            boss.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (cutsceneFrameCounter == 0)
                isCutscene = true;
        }
    }

    private void FixedUpdate()
    {
        if (player.checkFightingState())
            handleCutscene();
    }


    private void handleCutscene()
    {
        if (isCutscene)
        {
            if (cutsceneFrameCounter == 0)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.GetComponent<Animator>().SetFloat("Speed", 0);
                player.enabled = false;
            }

            if (cutsceneFrameCounter < cutsceneFrame)
                cutsceneFrameCounter++;
            else
                isCutscene = false;
        }
        else
        {
            boss.GetComponent<Rigidbody2D>().gravityScale = 3;
            player.enabled = true;
        }
    }
}
