using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator effectAnim;

    protected BoxCollider2D bc2d;
    protected string enemyType;
    protected Animator enemyAnim;
    protected SpriteRenderer sprite;
    protected SpriteFadeController sfc;
    protected bool dead;

    // Start is called before the first frame update
    public virtual void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        enemyAnim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sfc = GetComponent<SpriteFadeController>();
        dead = false;
    }

    public void Die()
    {
        dead = true;
        bc2d.enabled = false;

        sfc.StartFadingIn();
    }
}
