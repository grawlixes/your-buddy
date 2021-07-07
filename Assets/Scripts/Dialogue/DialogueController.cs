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
    // If not null, fade to black and load next scene after dialogue.
    public SpriteFadeController sfc;
    // If not null, fade to black after this prompt.
    public int fadeAfterThisPrompt;
    public bool inProgress = false;

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
        // See explanation below for why this line exists.
        int index = Mathf.Min(promptIndex, numUniquePrompts - 1);

        // If this dialogue is dependent on the day, add it to the file name (basically our "key").
        string dayString = "";
        if (dayDependent)
            dayString = day.ToString();

        return dialogueFamily + dayString + '_' + index.ToString() + ".txt";
    }

    // Triggers the "next" batch of dialogue.
    public void TriggerNextDialogue()
    {
        inProgress = true;

        string file = GetFileName();

        TextBoxController tbc = gameObject.AddComponent<TextBoxController>();
        tbc.enabled = false;
        tbc.textFile = file;
        tbc.dc = this;
        // this is a little confusing, but it works and is generic enough.
        bool isFinalDialogueBeforeFade = (fadeAfterThisPrompt == promptIndex && // is this the index we want to fade after?
                                          promptIndex != numUniquePrompts); // are we *on* the dialogue, or are we "past" it (read below)?
        if (isFinalDialogueBeforeFade)
            tbc.sfc = sfc;
        tbc.puzzlePrefabToEnableAfterDialogue = puzzlePrefabToEnableAfterDialogue;
        tbc.enabled = true;

        /* If we're out of prompts, keep showing the last one.
         * Special case: if we pass the final one, keep the index just past the final dialogue,
         * but revert back to the final dialogue for as long as the player initiates it.
         * Why do this instead of just staying at the final dialogue? Because when we extend dialogue
         * sequences in scripted events, this will allow us to just start right from the new dialogue.
         * If we didn't, we would have to show the user the stale dialogue first, which could confuse them.
         */
        promptIndex = Mathf.Min(promptIndex + 1, numUniquePrompts);
    }
}
