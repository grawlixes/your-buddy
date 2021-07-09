using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWorldController : MonoBehaviour
{
    public Animator effectAnim;
    public EnemyManager enemyManager;
    public GameObject dialogueParent;
    public int health = 3;
    public float flickerInvincibleSeconds = 2f;
    public float secondsPerFlick = .1f;
    public float dashDistance = 500f;
    public bool dead;
    public int day;

    private bool canGiveInput;
    private Animator playerAnim;
    private bool flickering;
    private SpriteRenderer sprite;
    private float invincibleSecondsLeft;
    private float nextFlickSeconds;
    private AudioSource takeDamageSound;
    private AudioSource eyeBlastSound;
    private AudioSource dashSound;
    private AudioSource music;
    private Rigidbody2D rb2d;
    private new Transform transform;
    public bool dashCooldown;
    private bool blastCooldown;
    private bool swiping;
    private Transform effectTransform;
    private SpriteFadeController sfc;
    private bool showingExample;

    // Start is called before the first frame update
    void Start()
    {
        canGiveInput = false;
        playerAnim = GetComponent<Animator>();
        flickering = false;
        sprite = GetComponent<SpriteRenderer>();
        takeDamageSound = GetComponent<AudioSource>();
        eyeBlastSound = effectAnim.gameObject.GetComponent<AudioSource>();
        dashSound = GameObject.Find("Canvas/Other Effects/DashSound")
                              .GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        transform = rb2d.transform;
        dashCooldown = false;
        blastCooldown = false;
        swiping = false;
        effectTransform = effectAnim.gameObject.transform;
        sfc = GameObject.Find("Canvas/Black")
                        .GetComponent<SpriteFadeController>();
        music = GameObject.Find("Music/Action Theme")
                          .GetComponent<AudioSource>();
        showingExample = false;

        if (day == 0)
            StartCoroutine(StartTutorial());
        else
            StartCoroutine(StartDialogueAndBegin());
    }

    private DialogueController extractDialogueFromChild(int child)
    {
        return dialogueParent.transform.GetChild(child)
                                       .GetComponent<DialogueController>();
    }

    private void StartGame()
    {
        music.Play();
        blastCooldown = false;
        dashCooldown = false;
        swiping = false;
        playerAnim.SetBool("moving", true);
        enemyManager.inTutorial = false;
        canGiveInput = true;
    }

    private IEnumerator StartDialogueAndBegin()
    {
        yield return new WaitForSeconds(3f);
        playerAnim.SetTrigger("gettingReady");

        DialogueController dc = extractDialogueFromChild(0);
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(2);
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(1);

        StartGame();
        yield break;
    }

    private IEnumerator StartTutorial()
    {
        enemyManager.inTutorial = true;
        canGiveInput = true;

        // welcome to hell
        // there are, like, at least three ways off the top of my head to generalize this garbage.
        // Am I gonna generalize it during this game jam, though? Take a guess
        yield return new WaitForSeconds(3);
        DialogueController dc = extractDialogueFromChild(0);
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(1);

        enemyManager.GetTutorialEnemiesInPosition();
        while (enemyManager.movingTutorialEnemies)
            yield return new WaitForSeconds(1);

        // looks at enemies
        yield return new WaitForSeconds(1);
        playerAnim.SetTrigger("lookAtEnemy");
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);
        // start tutorial
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);
        blastCooldown = true;
        showingExample = true;

        // teleport to skeleton
        while (!dashCooldown)
            yield return new WaitForSeconds(.25f);
        dc.TriggerNextDialogue(false);
        dashCooldown = true;
        swiping = true;

        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);
        swiping = false;
        blastCooldown = true;

        // kill skeleton
        while (!swiping)
            yield return new WaitForSeconds(.5f);
        swiping = true;
        canGiveInput = false;
        dashCooldown = false;
        enemyManager.TutorialThunder();
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);
        swiping = false;
        dashCooldown = false;

        canGiveInput = true;
        while (!dashCooldown)
            yield return new WaitForSeconds(.5f);
        dashCooldown = true;
        swiping = true;
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);
        blastCooldown = false;

        while (!blastCooldown)
            yield return new WaitForSeconds(.5f);
        showingExample = false;
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);
        playerAnim.SetTrigger("gettingReady");
        yield return new WaitForSeconds(2);
        dc.TriggerNextDialogue(false);

        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);

        StartGame();
        yield break;
    }

    private IEnumerator DieFinal()
    {
        effectAnim.SetTrigger("blasted");
        yield return new WaitForSeconds(.25f);
        takeDamageSound.Play();
        sfc.StartFadingOut();

        gameObject.SetActive(false);
        yield break;
    }

    private void Die()
    {
        dead = true;
        blastCooldown = true;
        dashCooldown = true;
        swiping = true;
        enemyManager.enabled = false;

        music.Stop();

        enemyManager.KillAllEnemies(null);

        playerAnim.SetTrigger("dead");

        if (day == 2)
            StartCoroutine(DieFinal());
        else
            sfc.StartFadingOut();
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
     *  - Dash: quickly dash forward or backward depending on position, player is invincible for this 
     *          time and can switch between these positions at will. There is a cooldown though.
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

    private IEnumerator Dash()
    {
        if (dashCooldown || swiping)
            yield break;

        effectAnim.SetTrigger("blast");
        dashSound.Play();
        Vector3 lp = transform.localPosition;
        lp.x += dashDistance;
        transform.localPosition = lp;

        dashCooldown = true;
        // next time, dash backwards
        dashDistance = -dashDistance;

        yield return new WaitForSeconds(.6f);

        dashCooldown = false;
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

    private IEnumerator Blast()
    {
        if (blastCooldown)
            yield break;
        blastCooldown = true;
        playerAnim.SetTrigger("poweredUp");
        effectAnim.SetTrigger("blast");
        eyeBlastSound.Play();

        enemyManager.KillAllEnemies("blasted");

        yield return new WaitForSeconds(5f);
        blastCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canGiveInput && (!enemyManager.inTutorial || showingExample))
        {
            string input = CheckForInput();
            if (input == "Dash")
            {
                StartCoroutine(Dash());
            } else if (input == "Swipe")
            {
                // swipe
                StartCoroutine(Swipe());
            }
            else if (input == "Blast")
            {
                // blast
                StartCoroutine(Blast());
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
}
