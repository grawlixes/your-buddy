using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldController : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator anim;
    public bool canMove = true;
    public bool canTakeDialogue = true;

    private const float WALKING_SPEED = 300f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // todo: replace this once done with the intro scene w/ name input.
        PlayerPrefs.SetString("PNAME", "Bailey");
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            TryToMove();
        }
    }

    public IEnumerator WaitThenEnableDialogue(TextBoxController tbcToDestroy, bool comesFromPuzzle)
    {
        yield return new WaitForSeconds(.5f);
        if (!comesFromPuzzle)
            canTakeDialogue = true;
        Destroy(tbcToDestroy);
    }

    private void TryToMove()
    {
        // These will both be -1, 0, or 1 since we're using binary controls (2 directions, pressed or not).
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // z should equal y because it determines which sprites are rendered on top of which
        // if a is lower than b on the screen, a is in front of b
        Vector3 speed = WALKING_SPEED * Time.deltaTime * (new Vector3(horizontal, vertical, 0).normalized);
        Vector3 lp = rigidbody.transform.localPosition;
        Vector3 np = lp + speed;
        np = new Vector3(np.x, np.y, np.y);
        rigidbody.transform.localPosition = np;

        if (horizontal != 0)
        {
            transform.localScale = new Vector3(-1 * horizontal * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }

        anim.SetInteger("xSpeed", (int) horizontal);
        anim.SetInteger("ySpeed", (int) vertical);
    }
}
