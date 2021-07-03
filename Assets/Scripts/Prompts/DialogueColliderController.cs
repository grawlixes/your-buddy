using UnityEngine;

public class DialogueColliderController : MonoBehaviour
{
    public DialogueController dialogueController;
    public OverworldController player;

    private bool inside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inside = false;
    }

    void Update()
    {
        if (inside && player.canTakeDialogue && Input.GetButtonDown("Use"))
        {
            player.canTakeDialogue = false;
            dialogueController.TriggerNextDialogue();
        }
    }
}
