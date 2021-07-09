using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DogIntroController : MonoBehaviour
{
    public GameObject nameField;
    public GameObject confirmButton;
    public GameObject player;
    public DialogueController dc;
    public SpriteFadeController sfc;

    private float x;
    private float playerX;
    private bool moving;
    private bool movingPlayer;
    private bool setPlayerName;
    private float speed = 300f;
    private float playerSpeed = 200f;
    private Animator anim;
    private Animator playerAnim;
    private string playerName;
    private AudioSource bark;

    void Start()
    {
        nameField.SetActive(false);
        confirmButton.SetActive(false);
        anim = GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
        bark = GetComponent<AudioSource>();
        moving = false;

        StartCoroutine(Intro());
    }

    public void OnClick()
    {
        playerName = nameField.GetComponent<TMP_InputField>()
                              .text;
        PlayerPrefs.SetString("PNAME", playerName);
        setPlayerName = true;
    }

    private void MoveToX(float x)
    {
        anim.SetInteger("speed", 10);
        this.x = x;
        moving = true;
    }

    private void PlayerMoveToX(float playerX)
    {
        playerAnim.SetInteger("xSpeed", 1);
        this.playerX = playerX;
        movingPlayer = true;
    }

    private IEnumerator Intro()
    {
        MoveToX(0);
        while (moving)
            yield return new WaitForSeconds(.5f);

        dc.TriggerNextDialogue(false);
        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);

        nameField.SetActive(true);
        confirmButton.SetActive(true);

        while (!setPlayerName)
            yield return new WaitForSeconds(.5f);
        nameField.SetActive(false);
        confirmButton.SetActive(false);
        dc.TriggerNextDialogue(false);
        PlayerMoveToX(300);
        var ls = GetComponent<RectTransform>().localScale;
        ls.x *= -1;
        GetComponent<RectTransform>().localScale = ls;

        while (movingPlayer)
            yield return new WaitForSeconds(.5f);
        bark.Play();
        yield return new WaitForSeconds(2);

        ls = player.GetComponent<RectTransform>().localScale;
        ls.x *= -1;
        player.GetComponent<RectTransform>().localScale = ls;

        PlayerMoveToX(800);
        MoveToX(800);

        sfc.StartFadingOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Vector2 dogPos = GetComponent<RectTransform>().localPosition;
            if (x < dogPos.x)
            {
                dogPos.x -= speed * Time.deltaTime;

                if (dogPos.x <= x)
                {
                    moving = false;
                    anim.SetInteger("speed", 0);
                }
            } else
            {
                dogPos.x += speed * Time.deltaTime;

                if (dogPos.x >= x)
                {
                    moving = false;
                    anim.SetInteger("speed", 0);
                }
            }

            GetComponent<RectTransform>().localPosition = dogPos;
        }

        if (movingPlayer)
        {
            Vector2 playerPos = player.GetComponent<RectTransform>().localPosition;
            if (playerX < playerPos.x)
            {
                playerPos.x -= playerSpeed * Time.deltaTime;

                if (playerPos.x <= playerX)
                {
                    movingPlayer = false;
                    playerAnim.SetInteger("xSpeed", 0);
                }
            }
            else
            {
                playerPos.x += playerSpeed * Time.deltaTime;

                if (playerPos.x >= playerX)
                {
                    movingPlayer = false;
                    playerAnim.SetInteger("xSpeed", 0);
                }
            }

            player.GetComponent<RectTransform>().localPosition = playerPos;
        }
    }
}
