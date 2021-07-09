using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleController : MonoBehaviour
{
    public DogController dog;

    private DialogueController dc;
    private AudioSource knockSound;
    private Vector3 doorWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChoreographEnd());

        dc = GetComponent<DialogueController>();
        knockSound = GetComponent<AudioSource>();
        doorWaypoint = new Vector3(120f, -85f, -1);
        dog.SetWaypoint(doorWaypoint);
    }

    private IEnumerator ChoreographEnd()
    {
        yield return new WaitForSeconds(3f);

        knockSound.Play();
        yield return new WaitForSeconds(.5f);

        dc.TriggerNextDialogue(false);
        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);
        knockSound.Play();

        dog.enabled = true;
        while (dog.FarFromWaypoint())
            yield return new WaitForSeconds(.5f);

        dog.barkSound.Play();
        dc.TriggerNextDialogue(false);
        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);
        
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
