using UnityEngine;

public class DogFollowController : MonoBehaviour
{
    public GameObject dog;
    public BoxCollider2D collider;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            DogController dc = dog.GetComponent<DogController>();
            dc.enabled = true;
            dc.JumpOffBed();

            collider.enabled = false;
        }
    }
}
