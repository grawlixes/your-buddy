using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unlocks the next conversation on the phone after you beat the puzzle.
public class OpenNewDialogue
{ 
    public static void OpenNew(string pathToDialogue)
    {
        DialogueController dialogue = GameObject.Find("Dialogues/" + pathToDialogue).GetComponent<DialogueController>();
        dialogue.promptIndex = dialogue.numUniquePrompts;
        dialogue.numUniquePrompts += 1;
    }
}
