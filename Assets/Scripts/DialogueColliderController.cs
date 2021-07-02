using UnityEngine;

public class DialogueColliderController : MonoBehaviour
{
    public DialogueController dialogueController;
    public OverworldController player;

    // todo this is buggy. find out why
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Player" && this.GetComponent<TextBoxController>() == null && Input.GetMouseButtonDown(0))
        {
            dialogueController.TriggerNextDialogue();
        }
    }
}
