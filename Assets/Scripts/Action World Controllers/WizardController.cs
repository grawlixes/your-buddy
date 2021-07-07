using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : EnemyController
{
    public float timePerThunder = 5f;

    private GameObject thunder;
    private GameObject player;
    private Animator thunderAnim;
    private AudioSource thunderSound;

    private float nextThunder;
    // Start is called before the first frame update
    public override void Awake()
    {

        base.Awake();

        effectAnim.SetTrigger("circle");
        nextThunder = timePerThunder;
        thunder = GameObject.Find("Canvas/Other Effects/LightningEffect");
        player = GameObject.Find("Canvas/Player");
        thunderAnim = thunder.GetComponent<Animator>();
        thunderSound = effectAnim.gameObject.GetComponent<AudioSource>();

        enemyType = "Wizard";
    }

    public IEnumerator Thunder(bool tutorial)
    {
        Vector3 pos = thunder.transform.localPosition;
        pos.x = player.transform.localPosition.x;
        thunder.transform.localPosition = pos;
        effectAnim.SetTrigger("circle");
        thunderAnim.SetTrigger("circle");
        thunderSound.Play();

        if (tutorial)
        {
            ActionWorldController awc = player.GetComponent<ActionWorldController>();
            while (!awc.dashCooldown)
                yield return new WaitForSeconds(.5f);
        } 
        else
            yield return new WaitForSeconds(1);

        thunderAnim.SetTrigger("thunder");
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.inTutorial)
            return;

        nextThunder -= Time.deltaTime;

        if (nextThunder <= 0)
        {
            StartCoroutine(Thunder(false));
            nextThunder = timePerThunder;
        }
    }
}
