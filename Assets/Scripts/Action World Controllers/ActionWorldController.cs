using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWorldController : MonoBehaviour
{
    private bool canGiveInput;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        canGiveInput = true;
        anim = GetComponent<Animator>();
        anim.SetBool("moving", true);
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
                Debug.Log("Blast");
            }
        }
    }
}
