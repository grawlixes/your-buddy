using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    // rows and columns of the puzzle
    public int n;
    public int m;
    // shape of the selector
    // can be "hLine", "vLine", or "plus"
    public string shape;
    // victory dialogue day
    public int day;

    // plays after you win the puzzle
    private DialogueController victoryDialogue;
    private PuzzleButtonController[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new PuzzleButtonController[n, m];
        string path = "Puzzle" + n.ToString() + "x" + m.ToString() + "(Clone)/";
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                grid[i,j] = GameObject.Find(path + "Button" + i.ToString() + "_" + j.ToString())
                                      .GetComponent<PuzzleButtonController>();
            }
        }
    }

    private int[,] GetNeighbors(int i, int j)
    {
        if (shape == "hLine")
        {
            return new int[,] { { i, j }, { i, j + 1 } };
        } else if (shape == "vLine") {
            return new int[,] { { i - 1, j }, { i, j }, { i + 1, j } };
        } else
        {
            return new int[,] { { i - 1, j }, { i, j - 1 }, { i, j }, { i, j + 1 }, { i + 1, j } };
        }
    }

    private bool Validate(int i, int j)
    {
        return (Mathf.Min(i, j) >= 0) &&
               (i < n) && (j < m);
    }

    private bool DidWin()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (!grid[i, j].active)
                    return false;
            }
        }

        return true;
    }

    public void ToggleHovered(int i, int j)
    {
        int[,] neighbors = GetNeighbors(i, j);
        for (int ni = 0; ni < neighbors.GetLength(0); ni++)
        {
            if (Validate(neighbors[ni, 0], neighbors[ni, 1]))
            {
                int thisI = neighbors[ni, 0];
                int thisJ = neighbors[ni, 1];

                grid[thisI, thisJ].ToggleHighlight();
            }
        }
    }

    public void Clicked(int i, int j)
    {
        int[,] neighbors = GetNeighbors(i, j);

        for (int ni = 0; ni < neighbors.GetLength(0); ni++)
        {
            if (Validate(neighbors[ni, 0], neighbors[ni, 1]))
            {
                // make clicked
                int thisI = neighbors[ni, 0];
                int thisJ = neighbors[ni, 1];
                grid[thisI, thisJ].ToggleActivated();
            }
        }

        if (DidWin())
        {
            GameObject canvas = GameObject.Find("Canvas");

            // todo
            // this is actually disgusting. please find a way to generalize it
            OpenNewDialogue.OpenNew("Phone");

            victoryDialogue = canvas.AddComponent<DialogueController>();
            victoryDialogue.SetFamily("victory", true, day, 1, null);
            victoryDialogue.TriggerNextDialogue();

            Destroy(gameObject);
        }
    }
}
