using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogInteractController : MonoBehaviour
{
    public AudioSource musicToPlay;
    
    private DogController dog;

    private void Start()
    {
        dog = transform.parent.gameObject
                              .GetComponent<DogController>();

        if (dog.sleeping)
            dog.anim.SetTrigger("sleeping");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (false && musicToPlay != null)
            {
                musicToPlay.Play();
                musicToPlay = null;
            }
            dog.playerCanInteract = true;
            dog.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
            dog.playerCanInteract = false;
    }
}
