using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDisplayScript : MonoBehaviour
{
    public DialogueController helpDialogue;
    public OverworldController player;

    void Start()
    {
        player.canTakeDialogue = false;
        helpDialogue.TriggerNextDialogue(false);
    }
}
