using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public CountDownTimer playerHealth;       // Reference to the player's health.


    Animator anim;                          // Reference to the animator component.


    void Awake()
    {
        // Set up the reference.
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        // If the player has run out of health...
        if (playerHealth.countdownTime <= 0)
        {
            // ... tell the animator the game is over.
            anim.SetTrigger("GameOver");
        }
    }
}