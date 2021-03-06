using UnityEngine;
using TMPro;

// Controls a single group of text boxes.
// One or more of these are handled by a single DialogueController for each object.
public class TextBoxController : MonoBehaviour
{
    // Set this to decide which file to read dialogue from.
    public string textFile;
    // This is to stop the player from moving while the text is up.
    public OverworldController player;
    // Instantiate this after the dialogue is over.
    public string puzzlePrefabToEnableAfterDialogue;
    // Fade this to black after the dialogue is over.
    public SpriteFadeController sfc;
    // Use this for events.
    public DialogueController dc;
    public bool comesFromPuzzle;

    private const string PATH_TO_BOX_PREFAB = "Prefabs/TextBox";
    private const string PATH_TO_TEXT_PREFAB = "Prefabs/Text";
    private GameObject currentTextBox;
    private GameObject currentText;

    private string[] dialogue;
    private int i = 0;

    void OnEnable()
    {
        if (textFile == null)
            return;

        GameObject playerGO = GameObject.Find("Canvas/Player");
        if (playerGO != null)
            player = GameObject.Find("Canvas/Player").GetComponent<OverworldController>();
        if (player != null)
            player.canMove = false;

        TextAsset txt = (TextAsset)Resources.Load("Text/" + textFile, 
                                                  typeof(TextAsset));
        dialogue = txt.text.Split('\n');
        TextBoxFactory(dialogue[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Use"))
        {
            GameObject.Destroy(currentTextBox);
            i++;

            if (i != dialogue.Length)
            {
                TextBoxFactory(dialogue[i]);
            } else
            {
                if (puzzlePrefabToEnableAfterDialogue != null && 
                    puzzlePrefabToEnableAfterDialogue.Length != 0)
                {
                    GameObject puzzle = Resources.Load(puzzlePrefabToEnableAfterDialogue) as GameObject;
                    GameObject.Instantiate(puzzle)
                              .GetComponent<PuzzleController>();
                } else if (sfc != null)
                {
                    sfc.StartFadingOut();
                } else
                {
                    if (player != null)
                        player.canMove = true;
                }

                if (player != null)
                    StartCoroutine(player.WaitThenEnableDialogue(this, comesFromPuzzle));
                else
                {
                    dc.inProgress = false;
                    Destroy(this);
                }
            }
        }
    }

    private string ReplacePnameInString(string textString)
    {
        if (textString.Contains("PNAME")) {
            return textString.Replace("PNAME", PlayerPrefs.GetString("PNAME"));
        } 

        return textString;
    }
    private void TextBoxFactory(string textString)
    {
        GameObject textBox = Resources.Load(PATH_TO_BOX_PREFAB) as GameObject;
        currentTextBox = GameObject.Instantiate(textBox);

        GameObject text = Resources.Load(PATH_TO_TEXT_PREFAB) as GameObject;
        text.GetComponent<TextMeshPro>().text = ReplacePnameInString(textString);
        currentText = GameObject.Instantiate(text);

        currentText.transform.SetParent(currentTextBox.transform, false);
    }
}
