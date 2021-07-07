using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : EnemyController
{
    public float speed = 3f;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        enemyType = "Skeleton";
        enemyAnim.SetBool("running", true);
    }

    private void MoveLeft()
    {
        Vector3 lp = rb2d.transform.localPosition;
        lp.x -= speed;
        rb2d.transform.localPosition = lp;
    }

    public override void Die()
    {
        enemyAnim.SetBool("running", false);
        base.Die();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
            MoveLeft();
    }
}
