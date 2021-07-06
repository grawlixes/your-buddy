using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageController : MonoBehaviour
{
    private BoxCollider2D bc2D;
    private SpriteRenderer sprite;

    // If the object is the player, this will be non-null.
    private ActionWorldController awc;
    // Otherwise, it's an enemy, and this will be non-null.
    // This isn't very clean - I should use inheritance - but I don't really care.
    private EnemyController ec;

    private bool flickering;

    // Start is called before the first frame update
    void Start()
    {
        bc2D = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        awc = GetComponent<ActionWorldController>();
        ec = GetComponent<EnemyController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (awc != null) {
            awc.TakeDamage();
        } else
        {
            ec.Die();
        }
    }
}
