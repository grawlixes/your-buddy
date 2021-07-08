using UnityEngine;

public class DogFollowController : MonoBehaviour
{
    public GameObject dog;
    public new BoxCollider2D collider;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            StartFollowing();
        }
    }

    public void StartFollowing()
    {
        DogController dc = dog.GetComponent<DogController>();
        dc.enabled = true;
        dc.JumpOffBed();

        if (collider != null)
            collider.enabled = false;
    }
}
