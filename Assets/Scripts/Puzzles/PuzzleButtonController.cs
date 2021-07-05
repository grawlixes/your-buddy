using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButtonController : MonoBehaviour
{
    public PuzzleController puzzle;
    public bool active;
    private bool highlighted = false;

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

        sprite.color = NewColor();
    }

    public void ToggleHighlight()
    {
        highlighted = !highlighted;

        sprite.color = NewColor();
    }

    private Color32 NewColor()
    {
        // if it's not highlighted or active, it should be grey
        if (!highlighted && !active) {
            return new Color32(255, 255, 255, 255);
        // highlighted blocks should be yellow
        } else if (highlighted && !active)
        {
            return new Color32(255, 255, 0, 255);
        // active blocks are dark green
        } else if (active && !highlighted)
        {
            return new Color32(0, 150, 0, 255);
        // active *and* highlighted blocks are light green
        } else
        {
            return new Color32(0, 255, 0, 255);
        }
    }
}
