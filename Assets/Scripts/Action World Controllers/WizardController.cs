using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : EnemyController
{
    public float timePerThunder = 5f;

    private GameObject thunder;
    private GameObject player;
    private Animator thunderAnim;

    private float nextThunder;
    // Start is called before the first frame update
    public override void Awake()
    {

        base.Awake();

        nextThunder = timePerThunder;
        thunder = GameObject.Find("Canvas/Other Effects/LightningEffect");
        player = GameObject.Find("Canvas/Player");
        thunderAnim = thunder.GetComponent<Animator>();
        enemyType = "Wizard";
    }

    public IEnumerator Thunder()
    {
        Vector3 pos = thunder.transform.localPosition;
        pos.x = player.transform.localPosition.x;
        thunder.transform.localPosition = pos;
        effectAnim.SetTrigger("circle");
        thunderAnim.SetTrigger("circle");
        yield return new WaitForSeconds(1);
        thunderAnim.SetTrigger("thunder");
    }

    // Update is called once per frame
    void Update()
    {
        nextThunder -= Time.deltaTime;

        if (nextThunder <= 0)
        {
            StartCoroutine(Thunder());
            nextThunder = timePerThunder;
        }
    }
}
