using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public GameObject player;
    public Animator anim;
    public new BoxCollider2D collider;

    private const float ACCEPTABLE_DISTANCE = 180f;
    private const float ACCEPTABLE_JUMP_DISTANCE = 90f;
    private const float SPEED_MODIFIER = 2f;
    private const float SPEED_MODIFIER_JUMPING = 5.5f;

    private new Rigidbody2D rigidbody;
    private Rigidbody2D playerRigidbody;
    private bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
    }

    public void JumpOffBed()
    {
        anim.SetTrigger("jumpOffBed");
        jumping = true;

        // todo: should enable this, but we need layers
        //collider.enabled = true;
    }

    private bool FarFromPlayer()
    {
        float distance = ACCEPTABLE_DISTANCE;
        if (jumping)
            distance = ACCEPTABLE_JUMP_DISTANCE;

        return Vector2.Distance(rigidbody.transform.localPosition, playerRigidbody.transform.localPosition) >= distance;
    }

    private void TryToMoveTowardPlayer()
    {
        // move the dog
        Vector3 playerLocation = playerRigidbody.transform.localPosition;
        Vector3 dogLocation = rigidbody.transform.localPosition;

        Vector3 movementTowardPlayer = playerLocation - dogLocation;

        float speedModifier = SPEED_MODIFIER;
        if (jumping)
            speedModifier = SPEED_MODIFIER_JUMPING;

        rigidbody.transform.localPosition += movementTowardPlayer.normalized * speedModifier;

        // flip the dog if necessary
        Vector3 ls = rigidbody.transform.localScale;
        if (playerLocation.x < dogLocation.x)
        {
            ls = new Vector3(-Mathf.Abs(ls.x), ls.y, 1);
        } else
        {
            ls = new Vector3(Mathf.Abs(ls.x), ls.y, 1);
        }

        rigidbody.transform.localScale = ls;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FarFromPlayer())
        {
            TryToMoveTowardPlayer();

            if (!jumping)
                anim.SetInteger("speed", 1);
        } else
        {
            if (jumping)
            {
                anim.SetTrigger("startFollowing");
                jumping = false;
            }
            anim.SetInteger("speed", 0);
        }
    }
}
