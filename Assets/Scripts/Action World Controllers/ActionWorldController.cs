using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWorldController : MonoBehaviour
{
    public Animator effectAnim;
    public int health = 3;
    public int framesToFlicker = 180;
    public int framesPerFlicker = 5;

    private bool canGiveInput;
    private Animator playerAnim;
    private bool flickering;
    private SpriteRenderer sprite;
    private int flickerFramesLeft;
    private int framesToNextFlicker;
    private AudioSource takeDamageSound;

    // Start is called before the first frame update
    void Start()
    {
        canGiveInput = true;
        playerAnim = GetComponent<Animator>();
        flickering = false;
        sprite = GetComponent<SpriteRenderer>();
        takeDamageSound = GetComponent<AudioSource>();

        playerAnim.SetBool("moving", true);
    }

    private void Die()
    {

    }
    public void TakeDamage()
    {
        if (flickering)
            return;

        takeDamageSound.Play();
        health -= 1;

        if (health > 0)
        {
            flickerFramesLeft = framesToFlicker;
            framesToNextFlicker = 1;
            flickering = true;
        }
        else
            Die();
    }

    /* Player can do three moves
     *  - Dash: quickly dash forward, player is invincible for this time and will slowly revert back to their original position.
     *          Player can't dash again until they reach the original position again.
     *  - Swipe: swing their arm to deal damage to enemies in front of them and reflect projectiles.
     *  - Blast: use eye power to blow up all enemies and projectiles. Recharges over time.
     *  
     *  Only one input per frame. It's more convenient this way, plus each attack must occur by itself, so it makes sense.
     */
    private string CheckForInput()
    {
        if (Input.GetButtonDown("Dash"))
        {
            return "Dash";
        } else if (Input.GetButtonDown("Swipe"))
        {
            return "Swipe";
        } else if (Input.GetButtonDown("Blast"))
        {
            return "Blast";
        } else
        {
            return "None";
        }
    }

    private void Flicker()
    {
        framesToNextFlicker -= 1;

        if (framesToNextFlicker == 0)
        {
            flickerFramesLeft -= 1;
            Color32 color = sprite.color;
            color.a = (byte)(255 - color.a);
            sprite.color = color;
            framesToNextFlicker = framesPerFlicker;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canGiveInput)
        {
            string input = CheckForInput();
            if (input == "Dash")
            {
                // dash
                Debug.Log("Dash");
            } else if (input == "Swipe")
            {
                // swipe
                Debug.Log("Swipe");
            }
            else if (input == "Blast")
            {
                // blast
                effectAnim.SetTrigger("blast");
                Debug.Log("Blast");
            }
        }

        if (flickering)
        {
            Flicker();

            if (flickerFramesLeft == 0)
            {
                flickering = false;
            }
        }
    }
}
