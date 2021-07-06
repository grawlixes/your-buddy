using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public ActionWorldController player;
    public bool belongsToPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!belongsToPlayer && collision.name == "Player")
        {
            player.TakeDamage();
        } else
        {
            EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
            if (ec != null)
                ec.Die();
        }
    }
}
