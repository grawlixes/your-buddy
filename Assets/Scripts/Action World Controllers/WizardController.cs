using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : EnemyController
{
    public float timePerThunder = 5f;

    private Animator thunderAnim;

    private float nextThunder;
    // Start is called before the first frame update
    public override void Start()
    {

        base.Start();

        nextThunder = timePerThunder;
        thunderAnim = GameObject.Find("Canvas/Other Effects/LightningEffect")
                                .GetComponent<Animator>();
        enemyType = "Wizard";

    }
    public IEnumerator Thunder()
    {
        effectAnim.SetTrigger("circle");
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
