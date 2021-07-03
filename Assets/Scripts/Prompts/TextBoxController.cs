using System.IO;
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

        player = GameObject.Find("Canvas/Player").GetComponent<OverworldController>();
        player.canMove = false;

        StreamReader sr = new StreamReader("Assets/Resources/Text/" + textFile);
        string rawString = sr.ReadToEnd();
        sr.Close();
        dialogue = rawString.Split('\n');

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
                player.canMove = true;
                StartCoroutine(player.WaitThenEnableDialogue(this.GetComponent<TextBoxController>()));
            }
        }
    }

    private void TextBoxFactory(string textString)
    {
        GameObject textBox = Resources.Load(PATH_TO_BOX_PREFAB) as GameObject;
        currentTextBox = GameObject.Instantiate(textBox);

        GameObject text = Resources.Load(PATH_TO_TEXT_PREFAB) as GameObject;
        text.GetComponent<TextMeshPro>().text = textString;
        currentText = GameObject.Instantiate(text);

        currentText.transform.SetParent(currentTextBox.transform, false);
    }
}
