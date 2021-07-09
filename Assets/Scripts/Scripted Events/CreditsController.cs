using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public GameObject end;
    
    private const string PREFAB_PATH = "Prefabs/Credits/";

    private AudioSource creditSong;
    private bool creditsOver = false;
    private string[] prefabs = { "Game", "Me", "Art", "Music", "Special" };
    private int[] secondsToShowEach = { 4, 5, 10, 5, 5};
    // Start is called before the first frame update
    void Start()
    {
        creditSong = GetComponent<AudioSource>();
        StartCoroutine(Credits());
    }

    private IEnumerator Credits()
    {
        yield return new WaitForSeconds(3);

        creditSong.Play();

        GameObject prev = end;
        for (int i = 0; i < prefabs.Length; i++)
        {
            prev.SetActive(false);
            yield return new WaitForSeconds(.5f);
            GameObject text = Resources.Load(PREFAB_PATH + prefabs[i]) as GameObject;
            prev = GameObject.Instantiate(text, gameObject.transform);
            
            yield return new WaitForSeconds(secondsToShowEach[i]);
        }

        prev.SetActive(false);
        GameObject finalText = Resources.Load(PREFAB_PATH + "ThanksExit") as GameObject;
        GameObject.Instantiate(finalText, gameObject.transform);

        creditsOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (creditsOver && Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
