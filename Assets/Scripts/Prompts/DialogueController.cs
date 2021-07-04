using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    // The name of the family of text files to read from.
    public string dialogueFamily;
    // Whether the text is dependent on the day.
    public bool dayDependent;
    // The day in the game.
    public int day;
    // The number of different prompts you can get.
    public int numUniquePrompts;
    // If not null, enable this puzzle prefab after dialogue.
    public string puzzlePrefabToEnableAfterDialogue;

    private int promptIndex = 0;

    // This lets us programmatically change the family of text to read from.
    // An example where this is useful is if we have a set of lines to read for one interactable object,
    // but after the player finishes a puzzle, we want to read a different set of lines on the same interactable.
    public void SetFamily(string dialogueFamily, bool dayDependent, int day, int numUniquePrompts, string puzzlePrefabToEnableAfterDialogue)
    {
        this.dialogueFamily = dialogueFamily;
        this.day = day;
        this.dayDependent = dayDependent;
        this.numUniquePrompts = numUniquePrompts;
        this.puzzlePrefabToEnableAfterDialogue = puzzlePrefabToEnableAfterDialogue;

        promptIndex = 0;
    }

    // Name format: "family<day>_<numPrompt>.txt"
    // If not day dependent: "family_<numPrompt>.txt"
    private string GetFileName()
    {
        string dayString = "";
        if (dayDependent)
            dayString = day.ToString();

        return dialogueFamily + dayString + '_' + promptIndex.ToString() + ".txt";
    }

    // Triggers the "next" batch of dialogue.
    public void TriggerNextDialogue()
    {
        string file = GetFileName();

        TextBoxController tbc = gameObject.AddComponent<TextBoxController>();
        tbc.enabled = false;
        tbc.textFile = file;
        tbc.puzzlePrefabToEnableAfterDialogue = puzzlePrefabToEnableAfterDialogue;
        tbc.enabled = true;

        // If we're out of prompts, keep showing the last one.
        promptIndex = Mathf.Min(promptIndex + 1, numUniquePrompts - 1);
    }
}
