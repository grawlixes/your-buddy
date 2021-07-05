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
    private const float SPEED_MODIFIER_WAYPOINT = 10f;

    private new Rigidbody2D rigidbody;
    private Rigidbody2D playerRigidbody;
    private bool jumping = false;

    private Vector3 waypoint;
    private bool playerIsWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        waypoint = GetEffectivePlayerPosition();
        playerIsWaypoint = true;
    }

    public void JumpOffBed()
    {
        anim.SetTrigger("jumpOffBed");
        jumpSound.enabled = true;
        jumping = true;
    }

    // Make the dog follow something new
    public void SetWaypoint(Vector3 newWaypoint)
    {
        waypoint = newWaypoint;
        playerIsWaypoint = false;
    }

    private Vector3 GetEffectivePlayerPosition()
    {
        Vector3 ret = playerRigidbody.transform.localPosition;
        // subtract 50f to make the dog stand at the player's feet, not his torso.
        return new Vector3(ret.x, ret.y - 50f, ret.z);
    }

    // Is the dog far away from wherever it's trying to go?
    private bool FarFromWaypoint()
    {
        return Vector2.Distance(rigidbody.transform.localPosition, waypoint) >= ACCEPTABLE_DISTANCE;
    }

    // The dog slows down when he approaches the waypoint. This is fine for now, but not expected.
    // Have to tweak the vector logic to fix this.
    private void TryToMoveTowardWaypoint()
    {
        // try to move the dog in front of the waypoint
        Vector3 dogLocation = rigidbody.transform.localPosition;
        Vector3 movementTowardWaypoint = waypoint - dogLocation;

        float speedModifier = SPEED_MODIFIER;
        if (jumping)
            speedModifier = SPEED_MODIFIER_JUMPING;
        else if (!playerIsWaypoint)
            speedModifier = SPEED_MODIFIER_WAYPOINT;

        // z should equal y because it determines which sprites are rendered on top of which
        // if a is lower than b on the screen, a is in front of b
        Vector3 lp = rigidbody.transform.localPosition;
        Vector3 np = lp + (movementTowardWaypoint.normalized * speedModifier);
        // add 40 to the Z since the dog is shorter - it should usually be in the back
        np = new Vector3(np.x, np.y, np.y + 40f);
        rigidbody.transform.localPosition = np;

        // flip the dog if necessary
        Vector3 ls = rigidbody.transform.localScale;
        if (waypoint.x < dogLocation.x)
        {
            ls = new Vector3(-Mathf.Abs(ls.x), ls.y, 1);
        } else
        {
            ls = new Vector3(Mathf.Abs(ls.x), ls.y, 1);
        }

        rigidbody.transform.localScale = ls;

        if (!jumping)
        {
            anim.SetInteger("speed", (int) speedModifier);
        }
    }

    void FixedUpdate()
    {
        // Move toward the waypoint if we're far away from it.
        if (FarFromWaypoint())
        {
            anim.SetBool("investigating", false);

            TryToMoveTowardWaypoint();

        } else
        {
            // If we've reached the waypoint and we are jumping, then stop jumping and start walking.
            if (jumping)
            {
                anim.SetTrigger("startFollowing");
                jumping = false;
            }

            // If we've reached the waypoint and the player isn't the waypoint, then the dog has found something.
            // Set off the "investigating" (sniff) animation.
            if (!playerIsWaypoint)
            {
                anim.SetBool("investigating", true);
            }

            anim.SetInteger("speed", 0);
        }

        // Have to constantly update the waypoint to the player's location if he's our waypoint.
        if (playerIsWaypoint)
        {
            waypoint = GetEffectivePlayerPosition();
        }
    }
}
