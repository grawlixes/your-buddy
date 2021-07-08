using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCollider : MonoBehaviour
{
    public OverworldController player;
    public DialogueController dialogue;
    public string puzzlePrefabName;
    
    private bool inside = false;
    private GameObject puzzle;

    void Start()
    {
        puzzle = Resources.Load("Prefabs/" + puzzlePrefabName) as GameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
            inside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
            inside = false;
    }

    void Update()
    {
        if (inside && player.canTakeDialogue && Input.GetButtonDown("Use"))
        {
            player.canTakeDialogue = false;
            dialogue.TriggerNextDialogue(true);
        }
    }
}
