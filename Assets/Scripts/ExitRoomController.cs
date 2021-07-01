using UnityEngine;
using UnityEngine.SceneManagement;
 
 
 public class ExitRoomController : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            //The scene number to load (in File->Build Settings)
            SceneManager.LoadScene(1);
        }
    }
}
