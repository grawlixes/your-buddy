using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public DogController dog;
    public PuzzleCollider puzzleCollider;
    public Vector3 waypoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            dog.SetWaypoint(waypoint);
            Destroy(this.GetComponent<WaypointController>());
        }
    }
}
