using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButtonController : MonoBehaviour
{
    public PuzzleController puzzle;
    public bool active;
    public bool highlighted;

    private int i;
    private int j;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        string name = gameObject.name;
        i = name[6] - '0';
        j = name[8] - '0';

        if (active)
            sprite.color = new Color(0, 255, 0);
    }

    public void OnMouseEnter()
    {
        puzzle.ToggleHovered(i, j);
    }

    public void OnMouseExit()
    {
        puzzle.ToggleHovered(i, j);
    }

    public void OnMouseDown()
    {
        puzzle.Clicked(i, j);
    }

    public void ToggleActivated()
    {
        active = !active;
        int newGreen = 255;
        if (!active)
            newGreen = 0;

        // go back to grey if it's not highlighted or activated
        float newRed = sprite.color.r;
        float newBlue = 0;
        if (!active && !highlighted)
        {
            newRed = 255;
            newGreen = 255;
            newBlue = 255;
        }

        sprite.color = new Color(newRed, newGreen, newBlue);
    }

    public void ToggleHighlight()
    {
        highlighted = !highlighted;
        int newRed = 255;
        if (!highlighted)
            newRed = 0;

        // go back to grey if it's not highlighted or activated
        float newGreen = sprite.color.g;
        float newBlue = 0;
        if (!highlighted && !active)
        {
            newRed = 255;
            newGreen = 255;
            newBlue = 255;
        }

        sprite.color = new Color(newRed, newGreen, newBlue);
    }
}
