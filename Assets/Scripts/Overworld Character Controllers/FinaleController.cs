using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleController : MonoBehaviour
{
    public DogController dog;
    public SpriteFadeController sfc;
    public bool isRoom;

    private DialogueController dc;
    private AudioSource knockSound;
    private Vector3 doorWaypoint;
    private Vector3 doorWaypoint2;
    private bool canSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        dc = GetComponent<DialogueController>();
        knockSound = GetComponent<AudioSource>();
        doorWaypoint = new Vector3(120f, -85f, -1);
        doorWaypoint2 = new Vector3(330f, -85f, -1);
        if (dog != null)
        {
            dog.SetWaypoint(doorWaypoint);
            dog.playerIsWaypoint = false;
        }

        if (isRoom)
            StartCoroutine(EndGame());
        else
            StartCoroutine(ChoreographEnd());
    }

    private IEnumerator EndGame()
    {
        AudioSource dogBark = GetComponent<AudioSource>();
        yield return new WaitForSeconds(7f);
        dogBark.Play();
        StartCoroutine(sfc.FadeOutImmediately());
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

        canSwitch = true;
        yield return new WaitForSeconds(3f);

        dc.TriggerNextDialogue(false);
        while (dc.inProgress)
            yield return new WaitForSeconds(.5f);

        yield return new WaitForSeconds(1);

        yield break;
    }

    private IEnumerator SwitchDogWaypoint()
    {
        if (dog.GetWaypoint() == doorWaypoint)
            dog.SetWaypoint(doorWaypoint2);
        else
            dog.SetWaypoint(doorWaypoint);
        dog.barkSound.Play();

        yield return new WaitForSeconds(2);
        canSwitch = true;
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (dog != null && dog.enabled && canSwitch)
        {
            canSwitch = false;
            StartCoroutine(SwitchDogWaypoint());
        }
    }
}
