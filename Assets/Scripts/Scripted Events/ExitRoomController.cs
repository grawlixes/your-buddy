using UnityEngine;
using UnityEngine.SceneManagement;
 
 
 public class ExitRoomController : MonoBehaviour
{
    public GameObject fader;
    public GameObject player;

    private SpriteFadeController fadeController;
    private void Start()
    {
        fadeController = fader.GetComponent<SpriteFadeController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            fadeController.StartFadingOut();
            player.GetComponent<OverworldController>()
                  .canMove = false;
        }
    }
}
