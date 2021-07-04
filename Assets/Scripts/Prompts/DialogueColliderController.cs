using UnityEngine;

public class DialogueColliderController : MonoBehaviour
{
    public DialogueController dialogueController;
    public OverworldController player;
    public bool shouldDeleteOnActivation;

    private bool inside = false;

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
            dialogueController.TriggerNextDialogue();

            if (shouldDeleteOnActivation)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
