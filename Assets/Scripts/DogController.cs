using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public GameObject player;
    public Animator anim;
    public AudioSource jumpSound;
    
    public new BoxCollider2D collider;

    private const float ACCEPTABLE_DISTANCE = 20f;
    private const float SPEED_MODIFIER = 3f;
    private const float SPEED_MODIFIER_JUMPING = 6.5f;

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
        jumpSound.enabled = true;
        jumping = true;

        // todo: should enable this, but we need layers
        //collider.enabled = true;
    }

    private Vector3 GetEffectivePlayerPosition()
    {
        Vector3 ret = playerRigidbody.transform.localPosition;
        return new Vector3(ret.x, ret.y - 50f, ret.z);
    }

    private bool FarFromPlayer()
    {
        return Vector2.Distance(rigidbody.transform.localPosition, GetEffectivePlayerPosition()) >= ACCEPTABLE_DISTANCE;
    }

    private void TryToMoveTowardPlayer()
    {
        // move the dog
        Vector3 playerLocation = GetEffectivePlayerPosition();
        // try to move the dog in front of the player
        Vector3 dogLocation = rigidbody.transform.localPosition;
        Vector3 movementTowardPlayer = playerLocation - dogLocation;

        float speedModifier = SPEED_MODIFIER;
        if (jumping)
            speedModifier = SPEED_MODIFIER_JUMPING;

        // z should equal y because it determines which sprites are rendered on top of which
        // if a is lower than b on the screen, a is in front of b
        Vector3 lp = rigidbody.transform.localPosition;
        Vector3 np = lp + (movementTowardPlayer.normalized * speedModifier);
        // add 30 to the Z since the dog is shorter - it should usually be in the back
        np = new Vector3(np.x, np.y, np.y + 40f);
        rigidbody.transform.localPosition = np;

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
