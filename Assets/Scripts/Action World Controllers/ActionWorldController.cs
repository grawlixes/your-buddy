using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWorldController : MonoBehaviour
{
    public Animator effectAnim;
    public int health = 3;
    public float flickerInvincibleSeconds = 2f;
    public float secondsPerFlick = .1f;
    // speed at which the player returns to the spot they dashed from
    public float returnSpeed = 3f;

    private bool canGiveInput;
    private Animator playerAnim;
    private bool flickering;
    private SpriteRenderer sprite;
    private float invincibleSecondsLeft;
    private float nextFlickSeconds;
    private AudioSource takeDamageSound;
    private Rigidbody2D rb2d;
    private new Transform transform;
    private float originalX;
    private bool returning;
    private bool swiping;
    private Transform effectTransform;

    // Start is called before the first frame update
    void Start()
    {
        canGiveInput = true;
        playerAnim = GetComponent<Animator>();
        flickering = false;
        sprite = GetComponent<SpriteRenderer>();
        takeDamageSound = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        transform = rb2d.transform;
        originalX = transform.localPosition.x;
        returning = false;
        swiping = false;
        effectTransform = effectAnim.gameObject.transform;

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
            invincibleSecondsLeft = flickerInvincibleSeconds;
            nextFlickSeconds = secondsPerFlick;
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
        nextFlickSeconds -= Time.deltaTime;
        invincibleSecondsLeft -= Time.deltaTime;

        if (nextFlickSeconds <= 0)
        {
            Color32 color = sprite.color;
            color.a = (byte)(255 - color.a);
            sprite.color = color;
            nextFlickSeconds = secondsPerFlick;
        }
    }

    private void Dash()
    {
        if (returning || swiping)
            return;

        effectAnim.SetTrigger("blast");
        Vector3 lp = transform.localPosition;
        lp.x += 350f;
        transform.localPosition = lp;
        returning = true;
    }

    private IEnumerator Swipe()
    {
        if (swiping)
            yield break;

        swiping = true;
        Vector3 ls = effectTransform.localScale;
        ls.x *= -1;
        effectTransform.localScale = ls;
        effectAnim.SetTrigger("swipe");

        yield return new WaitForSeconds(.5f);

        ls.x *= -1;
        effectTransform.localScale = ls;
        swiping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canGiveInput)
        {
            string input = CheckForInput();
            if (input == "Dash")
            {
                Dash();
            } else if (input == "Swipe")
            {
                // swipe
                StartCoroutine(Swipe());
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

            if (invincibleSecondsLeft <= 0)
            {
                Color32 color = sprite.color;
                color.a = 255;
                sprite.color = color;
                flickering = false;
            }
        }
    }

    private void FixedUpdate()
    { 
        if (returning)
        {
            Vector3 lp = transform.localPosition;
            lp.x = Mathf.Max(originalX, lp.x - returnSpeed);
            transform.localPosition = lp;

            if (lp.x == originalX)
                returning = false;
        }
    }
}
