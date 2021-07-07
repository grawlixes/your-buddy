using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator effectAnim;

    protected BoxCollider2D bc2d;
    protected Rigidbody2D rb2d;
    protected string enemyType;
    protected Animator enemyAnim;
    protected SpriteRenderer sprite;
    protected SpriteFadeController sfc;
    protected bool dead;
    protected new Transform transform;
    protected AudioSource deathSound;
    protected EnemyManager manager;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
        enemyAnim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sfc = GetComponent<SpriteFadeController>();
        rb2d = GetComponent<Rigidbody2D>();
        deathSound = GetComponent<AudioSource>();
        manager = GameObject.Find("Canvas/Enemy Manager")
                            .GetComponent<EnemyManager>();

        transform = rb2d.transform;
        dead = false;
    }

    public void PlayAnimation(string animation, bool isEffect)
    {
        if (isEffect)
            effectAnim.SetTrigger(animation);
        else
            enemyAnim.SetTrigger(animation);
    }

    public virtual void Die()
    {
        manager.enemiesOnScreen.Remove(this);

        deathSound.Play();
        dead = true;
        bc2d.enabled = false;

        sfc.StartFadingIn();
    }
}
