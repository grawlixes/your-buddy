using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldController : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator anim;

    private const float WALKING_SPEED = 2f;

    private bool canMove = true;
    private bool walking = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            TryToMove();
        }
    }

    private void TryToMove()
    {
        // These will both be -1, 0, or 1 since we're using binary controls (2 directions, pressed or not).
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 speed = WALKING_SPEED * Time.deltaTime * (new Vector2(horizontal, vertical).normalized);
        rigidbody.position += speed;

        if (horizontal != 0)
        {

            transform.localScale = new Vector3(-1 * horizontal * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }

        anim.SetInteger("xSpeed", (int) horizontal);
        anim.SetInteger("ySpeed", (int) vertical);
    }
}
